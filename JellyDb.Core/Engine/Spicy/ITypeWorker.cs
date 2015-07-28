using JellyDb.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public interface ITypeWorker<TKey>
    {
        byte[] GetBytes(TKey input);
        TKey GetTypedObject(byte[] input);
        int GetTypeSize();
        TKey ReadTypeFromDataSource(BinaryReaderWriter readerWriter);
        void WriteTypeToDataSource(BinaryReaderWriter readerWriter, TKey input);
        void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter);
    }

    public class TypeWorkerFactory
    {
        public static ITypeWorker<TKey> GetTypeWorker<TKey>()
        {
            if (typeof(TKey) == typeof(int)) return (ITypeWorker<TKey>)new IntWorker();
            else throw new NotSupportedException("type not supported: " + typeof(TKey).FullName);
        }
    }
}
