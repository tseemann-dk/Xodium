using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Data.Sequences
{
    public interface IDataRecordReader : IDisposable
    {
        Task<IDataRecord> ReadNextRecord(CancellationToken cancellationToken);
    }
}
