using JellyDb.Core.Engine.Fun;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class AddressSpaceIndex : DataWritableBase
    {
        public AddressSpaceIndex(IDataStorage dataStorage) :base(dataStorage)
        {
            LoadAddressSpaceIndex();
        }

        public static Guid IndexRootId = new Guid(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f });

        public List<DatabaseMetaData> MetaData { get; set; }

        private void LoadAddressSpaceIndex()
        {
            var data = _dataStorage.ReadToEndOfAddressSpace(0);
            if(data != null && data.Length > 0)
            {
                var jsonText = ConvertBytesToData(data);

                if (!string.IsNullOrEmpty(jsonText)) MetaData = JsonConvert.DeserializeObject<List<DatabaseMetaData>>(jsonText);
                else MetaData = new List<DatabaseMetaData>();    
            }else MetaData = new List<DatabaseMetaData>();
        }
    }

    public struct DatabaseMetaData
    {
        public string DatabaseName { get; set; }
        public Guid IndexId { get; set; }
        public Guid DataId { get; set; }
        public Type KeyType { get; set; }
        public bool AutoGenerateId { get; set; }
    }
}
