using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using JellyDb.Core.Configuration;
using System.IO;
using JellyDb.Core.Extensions;
using JellyDb.Core.VirtualAddressSpace.Storage;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class AddressSpaceManager : IDisposable
    {
        private PageIndex pageIndex;
        private const string indexFileFormat = "{0}.pages";
        private string dbFileName;
        private IDataStorage dataStorage;

        public AddressSpaceManager(IDataStorage storage)
        {
            dataStorage = storage;
            dbFileName = VirtualFileSystemConfigurationSection.ConfigSection.vfsFileName;
            pageIndex = new PageIndex(File.Open(string.Format(indexFileFormat, dbFileName), FileMode.OpenOrCreate, FileAccess.ReadWrite));
            long capacity = pageIndex.EndOfPageIndex;
            if (capacity == 0)
            {
                int increaseSize = VirtualFileSystemConfigurationSection.ConfigSection.PageSizeInKb *
                    VirtualFileSystemConfigurationSection.ConfigSection.PageIncreaseNum;
                capacity = (long)increaseSize;
                IndexFileGrowth();
            }
        }

        public Guid CreateVirtualAddressSpace()
        {
            Guid newGuid = Guid.NewGuid();
            ExpandAddressSpace(newGuid);
            return newGuid;
        }

        private void ExpandAddressSpace(Guid addressSpaceId)
        {
            if (pageIndex.EmptyPages.Count() == 0)
            {
                IndexFileGrowth();
            }
            PageSummary summary = pageIndex.EmptyPages[0];
            summary.AddressSpaceId = addressSpaceId;
            pageIndex.AddOrUpdateEntry(summary);            
        }

        private void IndexFileGrowth()
        {
            for (int i = 0; i < VirtualFileSystemConfigurationSection.ConfigSection.PageIncreaseNum; i++)
            {
                PageSummary newSummary = new PageSummary();
                newSummary.Allocated = false;
                newSummary.Offset = pageIndex.EndOfPageIndex;
                newSummary.Size = VirtualFileSystemConfigurationSection.ConfigSection.PageSizeInKb;
                newSummary.Used = 0;
                pageIndex.AddOrUpdateEntry(newSummary);
            }            
        }

        public void Dispose()
        {
            pageIndex.Dispose();
            dataStorage.Dispose();
        }

        public byte[] GetData(Guid addressSpaceId, long storageOffset, int numBytes)
        {            
            byte[] result = new byte[numBytes];
            long localOffset = storageOffset;
            int numProcessed = 0;

            foreach (var summary in pageIndex[addressSpaceId])
            {
                if (summary.Size <= localOffset)
                {
                    localOffset -= summary.Size;
                    continue;
                }
                
                int dataAvailableOnPage = summary.Size - localOffset.TruncateToInt32();//can cast to int since localOffset is iteratively shaved off until it is smaller than page size
                int bytesLeftToProcess = numBytes - numProcessed;
                int amountToProcess = Math.Min(dataAvailableOnPage, bytesLeftToProcess);
                dataStorage.ReadVirtualPage(ref result,
                    numProcessed,
                    (summary.Offset + localOffset),
                    amountToProcess);
                numProcessed += amountToProcess;
                localOffset = 0;
                if (numProcessed == numBytes) break;
            }
            return result;
        }

        public void SetData(Guid addressSpaceId, long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer)
        {
            long localOffset = storageOffset;
            int numProcessed = 0;

            for (int i = 0; i < pageIndex[addressSpaceId].Count; i++)
            {
                var summary = pageIndex[addressSpaceId][i];
                if (summary.Size <= localOffset)
                {
                    localOffset -= summary.Size;
                    continue;
                }

                int dataAvailableOnPage = summary.Size - localOffset.TruncateToInt32();//can cast to int since localOffset is iteratively shaved off until it is smaller than page size
                int bytesLeftToProcess = numBytes - numProcessed;
                int amountToProcess = Math.Min(dataAvailableOnPage, bytesLeftToProcess);
                dataStorage.WriteVirtualPage(ref dataBuffer,
                    (numProcessed + bufferIndex),
                    (summary.Offset + localOffset),
                    amountToProcess);
                numProcessed += amountToProcess;
                localOffset = 0;
                if (numProcessed == numBytes) break;
                else if ((pageIndex[addressSpaceId].IndexOf(summary) + 1) == pageIndex[addressSpaceId].Count)
                {
                    ExpandAddressSpace(addressSpaceId);
                }
            }
        }

        public void Flush()
        {
            dataStorage.Flush();
        }
        
    }
}
