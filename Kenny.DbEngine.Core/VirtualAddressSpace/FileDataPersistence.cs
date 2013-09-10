using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using Kenny.DbEngine.Core.Configuration;
using System.IO;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public class FileDataPersistence : IPersistData, IDisposable
    {
        private PageIndex pageIndex;
        private MemoryMappedFile fileMapping;
        private const string indexFileFormat = "{0}.pages";
        private const string memoryMappingName = "kennydb";
        private string dbFileName;
        private IViewManager viewManager;

        public FileDataPersistence()
        {
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
            fileMapping = MemoryMappedFile.CreateFromFile(dbFileName, FileMode.Create, memoryMappingName, capacity);
            viewManager = new ViewManager(fileMapping);
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
                ExpandStorageFile();
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

        private void ExpandStorageFile()
        {
            int increaseSize = VirtualFileSystemConfigurationSection.ConfigSection.PageSizeInKb *
                VirtualFileSystemConfigurationSection.ConfigSection.PageIncreaseNum;
            long capacity = pageIndex.EndOfPageIndex + (long)increaseSize;
            
            fileMapping.Dispose();
            viewManager.Dispose();
            fileMapping = MemoryMappedFile.CreateFromFile(dbFileName, FileMode.OpenOrCreate, memoryMappingName, capacity);
            viewManager = new ViewManager(fileMapping);
        }

        public void Dispose()
        {
            pageIndex.Dispose();
            viewManager.Dispose();
            fileMapping.Dispose();
        }

        public byte[] GetData(Guid addressSpaceId, long offset, long numBytes)
        {            
            byte[] result = new byte[numBytes];
            long localOffset = offset;
            long numProcessed = 0;

            foreach (var summary in pageIndex[addressSpaceId])
            {
                if (summary.Size <= localOffset)
                {
                    localOffset -= summary.Size;
                    continue;
                }
                else
                {
                    long dataAvailableOnPage = summary.Size - localOffset;
                    long bytesLeftToProcess = numBytes - numProcessed;
                    long amountToProcess = dataAvailableOnPage < bytesLeftToProcess ? dataAvailableOnPage : bytesLeftToProcess;
                    viewManager.ReadVirtualPage(ref result,
                        numProcessed,
                        (summary.Offset + localOffset),
                        amountToProcess);
                    numProcessed += amountToProcess;
                    localOffset = 0;
                    if (numProcessed == numBytes) break;
                }
            }
            return result;
        }

        public void SetData(Guid addressSpaceId, long offset, long startIndex, long numBytes, byte[] dataBuffer)
        {
            long localOffset = offset;
            long numProcessed = 0;

            for (int i = 0; i < pageIndex[addressSpaceId].Count; i++)
            {
                var summary = pageIndex[addressSpaceId][i];
                if (summary.Size <= localOffset)
                {
                    localOffset -= summary.Size;
                    continue;
                }
                else
                {
                    long dataAvailableOnPage = summary.Size - localOffset;
                    long bytesLeftToProcess = numBytes - numProcessed;
                    long amountToProcess = dataAvailableOnPage < bytesLeftToProcess ? dataAvailableOnPage : bytesLeftToProcess;
                    viewManager.WriteVirtualPage(ref dataBuffer,
                        (numProcessed + startIndex),
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
        }

        public void Flush()
        {
            viewManager.Flush();
        }
        
    }
}
