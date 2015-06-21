using JellyDb.Core.Engine.Fun;
using JellyDb.Core.Hosting;
using JellyDb.Core.VirtualAddressSpace;
using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class JellyDatabase : IDisposable
    {
        private readonly Dictionary<string, Database> _databases = new Dictionary<string, Database>();
        private IDataStorage _dataStorage;
        private AddressSpaceIndex _addressSpaceIndex;
        private AddressSpaceManager _addressSpaceManager;

        public JellyDatabase(string connectionString)
        {
            _dataStorage = new IoFileManager(connectionString);
            _dataStorage.Initialise();
            _addressSpaceManager = new AddressSpaceManager(_dataStorage);
            var addressSpaceIndexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(AddressSpaceIndex.IndexRootId);
            _addressSpaceIndex = new AddressSpaceIndex(addressSpaceIndexAgent);
            foreach (var metaData in _addressSpaceIndex.MetaData)
            {
                var indexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(metaData.IndexId);
                var index = new Index(indexAgent);
                var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(metaData.DataId);
                var database = new Database(index, dataAgent);
                _databases[metaData.DatabaseName] = database;
            }
        }

        public List<string> GetDatabaseNames()
        {
            return _databases.Keys.OrderBy(k => k).ToList();
        }

        public JellySession CreateSession()
        {
            var session = new JellySession();
            session.LoadRecord += OnLoadRecord;
            session.StoreRecord += OnStoreRecord;
            return session;
        }

        private Database CreateNewDatabase(string name)
        {
            var indexId = Guid.NewGuid();
            var indexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(indexId);
            var databaseId = Guid.NewGuid();
            var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(databaseId);
            var database = new Database(new Index(indexAgent), dataAgent);
            _databases[name] = database;
            _addressSpaceIndex.MetaData.Add(new DatabaseMetaData
            {
                DatabaseName = name,
                IndexId = indexId,
                DataId = databaseId
            });
            return database;
        }

        private void OnStoreRecord(JellyRecord record)
        {
            Database database = null;
            if (!_databases.TryGetValue(record.EntityType, out database))
                database = CreateNewDatabase(record.EntityType);
            //database.Write()
        }        

        private JellyRecord OnLoadRecord(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
