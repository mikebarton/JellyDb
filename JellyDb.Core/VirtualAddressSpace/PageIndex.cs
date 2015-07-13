using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using JellyDb.Core.Configuration;

namespace JellyDb.Core.VirtualAddressSpace
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
                    endOfIndex = summary.DataFileOffset + summary.Size;
                }
            } while (summary != null);
        }

        public IList<PageSummary> this[Guid addressSpaceId]
        {
            get 
            {
                List<PageSummary> index = null;
                indices.TryGetValue(addressSpaceId, out index);
                return index;
            }            
        }

        public long GetEndOfUsedAddressSpaceOffset(Guid addressSpaceId)
        {
            var addressSpace = indices[addressSpaceId];
            if (addressSpace == null) throw new InvalidDataException("AddressSpace does not exist in page index");
            //var ordered = addressSpace.OrderBy(p => p.LocalAddressSpaceOffset);
            //var lastPage = ordered.LastOrDefault();
            //if (lastPage == null) throw new InvalidDataException("AddressSpace exists, but has no pages in it");
            //if (lastPage.Used == lastPage.Size)
            //{
            //    ExpandAddressSpace(addressSpaceId, lastPage.LocalAddressSpaceOffset + lastPage.Size);
            //    return GetEndOfUsedAddressSpaceOffset(addressSpaceId);
            //}
            //return ((ordered.Count() -1) * lastPage.Size) + lastPage.Used;
            return addressSpace.Sum(u => u.Used);
        }

        public bool HasAddressSpace(Guid addressSpaceId)
        {
            return indices.ContainsKey(addressSpaceId);
        }

        public IList<PageSummary> EmptyPages { get { return indices[Guid.Empty]; } }

        public long EndOfPageIndex { get { return endOfIndex; } }

        public void ExpandAddressSpace(Guid addressSpaceId, long localOffset)
        {
            if (!EmptyPages.Any())
            {
                IndexFileGrowth();
            }
            PageSummary summary = EmptyPages[0];
            summary.AddressSpaceId = addressSpaceId;
            summary.LocalAddressSpaceOffset = localOffset;
            AddOrUpdateEntry(summary);
        }   

        public void IndexFileGrowth()
        {
            for (int i = 0; i < DbEngineConfigurationSection.ConfigSection.VfsConfig.PageIncreaseNum; i++)
            {
                PageSummary newSummary = new PageSummary();
                newSummary.AddressSpaceId = Guid.Empty;
                newSummary.Allocated = false;
                newSummary.DataFileOffset = EndOfPageIndex;
                newSummary.Size = DbEngineConfigurationSection.ConfigSection.VfsConfig.PageSizeInKb;
                newSummary.Used = 0;
                AddOrUpdateEntry(newSummary);
            }
        }
                
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
                endOfIndex = Math.Max((summary.DataFileOffset + summary.Size), endOfIndex);
            }
            else
            {
                stream.Position = summary.PageFileIndex.Value;
                if (!indices[summary.AddressSpaceId].Contains(summary))
                {
                    //foreach (var space in indices.Values)
                    //{
                    //    if (space.Contains(summary)) space.Remove(summary);
                    //}
                    EmptyPages.Remove(summary);
                    indices[summary.AddressSpaceId].Add(summary);
                    summary.Allocated = true;
                }
            }

            summary.WriteToStream(writer);
            stream.Flush();
        }

        public void Flush()
        {
            writer.Flush();
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
