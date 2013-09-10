using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public class PageIndex : IDisposable
    {
        private Dictionary<Guid, List<PageSummary>> indices;
        private Stream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private long endOfIndex;

        public PageIndex(Stream stream)
        {
            endOfIndex = 0;
            this.stream = stream;
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            indices = new Dictionary<Guid, List<PageSummary>>();
            indices[Guid.Empty] = new List<PageSummary>();
            
            PageSummary summary = null;
            do
            {
                summary = PageSummary.ReadFromStream(reader);
                if (summary != null)
                {
                    if (!indices.Keys.Contains(summary.AddressSpaceId))
                    {
                        indices.Add(summary.AddressSpaceId, new List<PageSummary>());
                    }
                    indices[summary.AddressSpaceId].Add(summary);
                    endOfIndex = summary.Offset + summary.Size;
                }
            } while (summary != null);
        }

        public IList<PageSummary> this[Guid addressSpaceId]
        {
            get { return indices[addressSpaceId]; }            
        }

        public IList<PageSummary> EmptyPages { get { return indices[Guid.Empty]; } }

        public long EndOfPageIndex { get { return endOfIndex; } }
                
        public void AddOrUpdateEntry(PageSummary summary)
        {
            if (!indices.Keys.Contains(summary.AddressSpaceId))
            {
                indices[summary.AddressSpaceId] = new List<PageSummary>();
            }
            if (!summary.PageFileIndex.HasValue)
            {                
                stream.Position = stream.Length;
                indices[summary.AddressSpaceId].Add(summary);
                long newIndex = summary.Offset + summary.Size;
                endOfIndex = newIndex > endOfIndex ? newIndex : endOfIndex;
            }
            else
            {
                stream.Position = summary.PageFileIndex.Value;
                if (!indices[summary.AddressSpaceId].Contains(summary))
                {
                    indices[Guid.Empty].Remove(summary);
                    indices[summary.AddressSpaceId].Add(summary);
                    summary.Allocated = true;
                }
            }

            summary.WriteToStream(writer);
            stream.Flush();
        }

        public void Dispose()
        {
            reader.Close();
            writer.Close();
            stream.Close();
        }
    }
}
