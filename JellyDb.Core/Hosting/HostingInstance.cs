using JellyDb.Core.Engine.Fun;
using JellyDb.Core.VirtualAddressSpace;
using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Hosting
{
    public class HostingInstance
    {
        private HostingConfiguration _hostingConfig;
        private Dictionary<string, Database> _databases = new Dictionary<string, Database>();
        private IDataStorage _dataStorage;

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
        }
    }
}
