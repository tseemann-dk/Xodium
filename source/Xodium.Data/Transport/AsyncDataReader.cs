using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace Xodium.Data.Sequences
{
    public class AsyncDataReader : IAsyncEnumerable<IDataRecord>, IDisposable
    {
        private readonly Func<IDataRecordReader> recordReaderFactory;

        public AsyncDataReader(Func<IDataRecordReader> recordReaderFactory)
        {
            this.recordReaderFactory = recordReaderFactory ?? throw new ArgumentNullException(nameof(recordReaderFactory));
        }

        public IAsyncEnumerator<IDataRecord> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new AsyncDataEnumerator(recordReaderFactory(), cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
