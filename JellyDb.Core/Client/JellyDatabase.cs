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
        private RecordIdentityService _identityService = new RecordIdentityService();
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

        private Dictionary<Type, object> _keyGenerators = new Dictionary<Type, object>();
        public void RegisterIdentityProperty<TSource, TKey>(Expression<Func<TSource, TKey>> propertyExpression, bool autoGenerateKey) where TSource : class
        {
            _identityService.RegisterTypeIdentity<TSource, TKey>(propertyExpression, autoGenerateKey);
        }

        private Database CreateNewDatabase(Type keyType, string name)
        {
            var indexId = Guid.NewGuid(); 
            var indexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(indexId);
            var databaseId = Guid.NewGuid();
            var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(databaseId);
            var database = CreateDatabase(keyType, indexAgent, dataAgent);
            _databases[name] = database;
            _addressSpaceIndex.MetaData.Add(new DatabaseMetaData
            {
                DatabaseName = name,
                IndexId = indexId,
                DataId = databaseId,
                KeyType = keyType
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

        internal void OnStoreRecord<TEntity>(JellyRecord<TEntity> record)
        {
            Database database = null;
            var entityType = typeof(TEntity);
            
            if (!_databases.TryGetValue(entityType.Name, out database))
                database = CreateNewDatabase(entityType, entityType.Name);

            var dataKey = _identityService.LoadIdentity<TEntity>(record);

            database.Write(dataKey, record.GetSerializedData());
        }        

        internal JellyRecord<TEntity> OnLoadRecord<TKey,TEntity>(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
