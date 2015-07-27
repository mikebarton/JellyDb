using JellyDb.Core.Engine.Fun;
using JellyDb.Core.VirtualAddressSpace;
using JellyDb.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Hosting
{
    public class HostingInstance
    {
        private readonly HostingConfiguration _hostingConfig;
        //private readonly Dictionary<string, Database> _databases = new Dictionary<string, Database>();
        private IDataStorage _dataStorage;
        private AddressSpaceIndex _addressSpaceIndex;

        public HostingInstance(HostingConfiguration config)
        {
            _hostingConfig = config;
        }

        public void Initialise()
        {            
            switch (_hostingConfig.HostingType)
            {
                case HostingType.FileBased:
                    InitialiseFileBasedDatabase();
                    break;
                case HostingType.ServerBased:
                    InitialiseServerBasedDatabase();
                    break;                
            }
        }

        private void InitialiseServerBasedDatabase()
        {
            throw new NotImplementedException();
        }

        private void InitialiseFileBasedDatabase()
        {
            _dataStorage = new IoFileManager(_hostingConfig.ConnectionString);
            _dataStorage.Initialise();
            var dataManager = new AddressSpaceManager(_dataStorage);
            var addressSpaceIndexAgent = dataManager.CreateVirtualAddressSpaceAgent(AddressSpaceIndex.IndexRootId);
            _addressSpaceIndex = new AddressSpaceIndex(addressSpaceIndexAgent);
            foreach (var metaData in _addressSpaceIndex.MetaData)
            {
                //var indexAgent = dataManager.CreateVirtualAddressSpaceAgent(metaData.IndexId);
                //var index = new Index(indexAgent);
                //var dataAgent = dataManager.CreateVirtualAddressSpaceAgent(metaData.DataId);
                //var database = new Database(index, dataAgent);
                //_databases[metaData.DatabaseName] = database;
            }
        }
    }
}
