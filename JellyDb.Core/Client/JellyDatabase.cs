using System.Linq.Expressions;
using System.Reflection;
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
                var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(metaData.DataId);
                var database = CreateDatabase(metaData.KeyType, indexAgent, dataAgent);
                _databases[metaData.DatabaseName] = database;
            }
        }

        public List<string> GetDatabaseNames()
        {
            return _databases.Keys.OrderBy(k => k).ToList();
        }

        public JellySession CreateSession()
        {
            var session = new JellySession(this);
            return session;
        }

        
        public void RegisterIdentityProperty<TSource, TKey>(Expression<Func<TSource, TKey>> propertyExpression)
        {
            
        }

        private Database CreateNewDatabase<TKey>(string name)
        {
            var indexId = Guid.NewGuid(); 
            var indexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(indexId);
            var databaseId = Guid.NewGuid();
            var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(databaseId);
            var database = CreateDatabase(typeof(TKey), indexAgent, dataAgent);
            _databases[name] = database;
            _addressSpaceIndex.MetaData.Add(new DatabaseMetaData
            {
                DatabaseName = name,
                IndexId = indexId,
                DataId = databaseId,
                KeyType = typeof(TKey)
            });
            return database;
        }

        private Database CreateDatabase(Type keyType, IDataStorage indexStorage, IDataStorage dataStorage)
        {
            var indexType = typeof(Index<>).MakeGenericType(keyType);
            var index = Activator.CreateInstance(indexType, indexStorage) as IIndex;
            var database = new Database(index, dataStorage);
            return database;
        }

        internal void OnStoreRecord<TKey>(JellyRecord record)
        {
            Database database = null;
            if (!_databases.TryGetValue(record.EntityType, out database))
                database = CreateNewDatabase<TKey>(record.EntityType);

            //database.Write()
        }        

        internal JellyRecord OnLoadRecord<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
