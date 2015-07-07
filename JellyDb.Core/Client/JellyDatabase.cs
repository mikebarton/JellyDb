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
using Newtonsoft.Json;

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
                var database = IntialiseDatabase(metaData.KeyType, metaData.IndexId, metaData.DataId);
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

        private Dictionary<Type, IKeyGenerator> _keyGenerators = new Dictionary<Type, IKeyGenerator>();
        public void RegisterIdentityProperty<TSource, TKey>(Expression<Func<TSource, TKey>> propertyExpression, bool autoGenerateKey) where TSource : class
        {
            var t = typeof(TKey);
            if (t != typeof(int) &&
                t != typeof(uint) &&
                t != typeof(long) &&
                t != typeof(ulong))
                throw new InvalidOperationException(string.Format("Key type for id property of entity to be stored must be short, int, long,, string or datetime. Not {0}", t.Name));

            var entityType = typeof(TSource);
            if (_keyGenerators.ContainsKey(entityType))
                throw new InvalidOperationException(string.Format("There is already an identity function registered for for type {0}", entityType.FullName));
            
            var generator = new KeyGenerator<TKey, TSource>(propertyExpression, autoGenerateKey);
            _keyGenerators.Add(entityType, generator);

            if (!DatabaseExists<TSource>()) CreateNewDatabase<TKey, TSource>(autoGenerateKey);
        }

        private Database CreateNewDatabase<TKey, TEntity>(bool autoGenerateKey)
        {
            var keyType = typeof (TKey);
            var name = GetEntityName<TEntity>();
            var indexId = Guid.NewGuid(); 
            var databaseId = Guid.NewGuid();
            var database = IntialiseDatabase(keyType, indexId, databaseId);
            _databases[name] = database;
            _addressSpaceIndex.MetaData.Add(new DatabaseMetaData
            {
                DatabaseName = name,
                IndexId = indexId,
                DataId = databaseId,
                KeyType = keyType
            });
            
            if(autoGenerateKey)
            {
                var autoGen = new AutoGenIdentity() {CurrentUsedId = 0, EntityTypeName = name};
                _keyGenerators[typeof(TEntity)].RegisterAutoGenIdentity(autoGen);
                var typeComparer = TypeComparer<TKey>.GetTypeComparer();
                var autoGenIndexKey = typeComparer.Increment(typeComparer.MinKey);
                autoGen.NextIdentityRetrieved += (sender, args) =>
                    {
                        var updated = (AutoGenIdentity) sender;
                        var databaseToUpdate = _databases[updated.EntityTypeName];
                        var dataText = JsonConvert.SerializeObject(updated);
                        databaseToUpdate.Write(DataKey.CreateKey<TKey>(autoGenIndexKey), dataText);
                    };
            }

            return database;
        }

        private Database IntialiseDatabase(Type keyType, Guid indexId, Guid databaseId)
        {
            var dataAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(databaseId);
            var indexAgent = _addressSpaceManager.CreateVirtualAddressSpaceAgent(indexId);
            var indexType = typeof(Index<>).MakeGenericType(keyType);
            var index = Activator.CreateInstance(indexType, indexAgent) as IIndex;
            var database = new Database(index, dataAgent);
            return database;
        }

        private string GetEntityName<TEntity>()
        {
            var entityType = typeof (TEntity);
            return entityType.Name;
        }

        private bool DatabaseExists<TSource>()
        {
            var entityName = GetEntityName<TSource>();
            return _databases.ContainsKey(entityName);
        }

        internal void OnStoreRecord<TEntity>(JellyRecord<TEntity> record) where TEntity : class
        {
            Database database = null;
            var entityType = typeof (TEntity);
            var entityName = GetEntityName<TEntity>();

            if (!_databases.TryGetValue(entityName, out database))
            {
                database = CreateNewDatabase<long, TEntity>(true);
                RegisterIdentityProperty<TEntity, long>(null, true);
            }

            var dataKey = _keyGenerators[entityType].GenerateKey(record.Entity);

            database.Write(dataKey, record.GetSerializedData());
        }

        internal JellyRecord<TEntity> OnLoadRecord<TKey,TEntity>(TKey id)
        {
            var dataKey = DataKey.CreateKey(id);
            var entityName = GetEntityName<TEntity>();
            Database database = null;
            if(_databases.TryGetValue(entityName,out database))
            {
                var retrieved = database.Read(dataKey);
                return new JellyRecord<TEntity>(retrieved);
            }
            return null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
