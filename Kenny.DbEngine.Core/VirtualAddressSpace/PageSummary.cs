using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public class PageSummary
    {
        private Guid addressSpaceId;
        private int size;
        private int used;
        private long offset;
        private bool allocated;
        private long? pageFileIndex;
        
        public Guid AddressSpaceId
        {
            get { return addressSpaceId; }
            set { addressSpaceId = value; }
        }

        public long? PageFileIndex
        {
            get { return pageFileIndex; }
            set { pageFileIndex = value; }
        }

        public long Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Used
        {
            get { return used; }
            set { used = value; }
        }

        public bool Allocated
        {
            get { return allocated; }
            set { allocated = value; }
        }

        public static PageSummary ReadFromStream(BinaryReader reader)
        {
            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                return null;
            }            
            PageSummary summary = new PageSummary();
            summary.pageFileIndex = reader.BaseStream.Position;
            summary.addressSpaceId = new Guid(reader.ReadBytes(16));
            summary.size = reader.ReadInt32();
            summary.used = reader.ReadInt32();
            summary.offset = reader.ReadInt64();
            summary.allocated = reader.ReadBoolean();
            return summary;
        }

        public void WriteToStream(BinaryWriter writer)
        {
            this.pageFileIndex = writer.BaseStream.Position;
            writer.Write(addressSpaceId.ToByteArray());
            writer.Write(this.size);
            writer.Write(this.used);
            writer.Write(this.offset);
            writer.Write(this.allocated);
        }
        
    }
}
