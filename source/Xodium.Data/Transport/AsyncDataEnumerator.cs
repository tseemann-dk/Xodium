using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Data.Sequences
{
    public class AsyncDataEnumerator : IAsyncEnumerator<IDataRecord>
    {
        private readonly IDataRecordReader recordReader;
        private readonly CancellationToken cancellationToken;
        private IDataRecord currentRecord;

        public IDataRecord Current => currentRecord;

        public AsyncDataEnumerator(IDataRecordReader recordReader, CancellationToken cancellationToken)
        {
            this.recordReader = recordReader ?? throw new ArgumentNullException(nameof(recordReader));
            this.cancellationToken = cancellationToken;
        }

        public virtual ValueTask DisposeAsync()
        {
            recordReader.Dispose();
            return default;
        }

        public virtual async ValueTask<bool> MoveNextAsync()
        {
            try
            {
                currentRecord = await recordReader.ReadNextRecord(cancellationToken);
                return currentRecord != null;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                throw;
            }
        }
    }
}
