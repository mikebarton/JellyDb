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
        private string pageIndexFileName;
        private IDataStorage dataStorage;

        public AddressSpaceManager(IDataStorage storage)
        {
            dataStorage = storage;
            var folderName = DbEngineConfigurationSection.ConfigSection.FolderPath;
            pageIndexFileName = Path.Combine(folderName, "dbFile.pageIndex");
            pageIndex = new PageIndex(File.Open(pageIndexFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            if (pageIndex.EndOfPageIndex == 0)
            {
                IndexFileGrowth();
            }
        }

        public AddressSpaceAgent CreateVirtualAddressSpaceAgent(Guid addressSpaceId)
        {
            throw new NotImplementedException();
        }

        public Guid CreateVirtualAddressSpace()
        {
            Guid newGuid = Guid.NewGuid();
            ExpandAddressSpace(newGuid);
            return newGuid;
        }

        private void ExpandAddressSpace(Guid addressSpaceId)
        {
            if (pageIndex.EmptyPages.Any())
            {
                IndexFileGrowth();
            }
            PageSummary summary = pageIndex.EmptyPages[0];
            summary.AddressSpaceId = addressSpaceId;
            pageIndex.AddOrUpdateEntry(summary);            
        }

        private void IndexFileGrowth()
        {
            for (int i = 0; i < DbEngineConfigurationSection.ConfigSection.VfsConfig.PageIncreaseNum; i++)
            {
                PageSummary newSummary = new PageSummary();
                newSummary.Allocated = false;
                newSummary.Offset = pageIndex.EndOfPageIndex;
                newSummary.Size = DbEngineConfigurationSection.ConfigSection.VfsConfig.PageSizeInKb;
                newSummary.Used = 0;
                pageIndex.AddOrUpdateEntry(newSummary);
            }            
        }

        public void ResetAddressSpace(Guid addressSpaceId)
        {
            foreach (var page in pageIndex[addressSpaceId])
            {
                byte[] buffer = new byte[page.Size];
                SetData(addressSpaceId, page.Offset, 0, buffer.Length, buffer);
                page.Allocated = false;
                page.Used = 0;
                pageIndex.AddOrUpdateEntry(page);
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
                dataStorage.ReadData(ref result,
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
                dataStorage.WriteData(ref dataBuffer,
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
