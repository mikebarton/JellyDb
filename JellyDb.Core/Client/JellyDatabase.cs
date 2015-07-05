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

        private Dictionary<Type, object> _keyGenerators = new Dictionary<Type, object>();
        public void RegisterIdentityProperty<TSource, TKey>(Expression<Func<TSource, TKey>> propertyExpression, bool autoGenerateKey)
        {
            var entityType = typeof(TSource);
            if (_keyGenerators.ContainsKey(entityType))
                throw new InvalidOperationException(string.Format("There is already an identity function registered for for type {0}", entityType.FullName));

            _keyGenerators.Add(entityType, propertyExpression.Compile());
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

        internal void OnStoreRecord<TEntity>(IJellyRecord record)
        {
            Database database = null;
            var entityType = record.GetEntityType();
            
            if (!_databases.TryGetValue(entityType.Name, out database))
                database = CreateNewDatabase(entityType, entityType.Name);

            var keyGenerator = _keyGenerators[entityType];
            database.Write()
            //database.Write()
        }        

        internal IJellyRecord OnLoadRecord<TKey>(TKey id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
