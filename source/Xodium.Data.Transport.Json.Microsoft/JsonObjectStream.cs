using System;
using System.IO;

namespace Xodium.Data.Transport.Json.Microsoft
{
    public class JsonObjectStream : Stream
    {
        private const char objectStart = '{';
        private const char objectEnd = '}';
        private const char quote = '"';

        private readonly Stream source;
        private int objectLevel;
        private bool insideString;
        private bool atEndOfObject;
        private byte[] prefetched;

        public JsonObjectStream(Stream source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public override bool CanRead => source.CanRead;
        public override bool CanSeek => source.CanSeek;
        public override bool CanWrite => source.CanWrite;

        public override long Length => source.Length;

        public override long Position 
        { 
            get => source.Position; 
            set => source.Position = value; 
        }

        public override void Flush() => source.Flush();
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (atEndOfObject) return 0;
            int result = ReadData(buffer, offset, count);

            for (var i = offset; i < offset + result && !atEndOfObject; i++)
            {
                var ch = (char)buffer[i];

                switch (ch)
                {
                    case objectStart:
                        if (!insideString)
                        {
                            objectLevel++;
                        }
                        break;
                    case objectEnd:
                        if (!insideString && --objectLevel == 0)
                        {
                            atEndOfObject = true;
                            var total = i - offset + 1;
                            var undo = result - total;
                            var remainder = new byte[undo];
                            Array.Copy(buffer[offset..], total, remainder, 0, undo);
                            prefetched = Concatenate(remainder, prefetched);
                            result = total;
                        }
                        break;
                    case quote:
                        insideString = !insideString;
                        break;
                }
            }

            return result;
        }
 
        public override void Write(byte[] buffer, int offset, int count)
        {
            source.Write(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => source.Seek(offset, origin);
        public override void SetLength(long value) => source.SetLength(value);

        public bool SkipUntilObjectStart()
        {
            var buffer = new byte[1];

            while (true)
            {
                if (ReadData(buffer, 0, 1) < 1)
                    return false;

                if (buffer[0] == objectStart)
                {
                    atEndOfObject = false;
                    prefetched = Concatenate(buffer, prefetched);
                    return true;
                }
            }
        }

        private int ReadData(byte[] buffer, int offset, int count)
        {
            if (prefetched != null)
            {
                var length = prefetched.Length;

                if (count < length)
                {
                    prefetched[..count].CopyTo(buffer, offset);
                    prefetched = prefetched[count..];
                    return count;
                }
                else
                {
                    prefetched.CopyTo(buffer, offset);
                    prefetched = null;

                    return count > length
                        ? source.Read(buffer, offset + length, count - length) + length
                        : length;
                }
            }

            return source.Read(buffer, offset, count);
        }

        private static byte[] Concatenate(byte[] a, byte[] b)
        {
            if (b is null)
            {
                return a;
            }
            
            if (a is null)
            {
                return b;
            }
            
            var c = new byte[a.Length + b.Length];
            a.CopyTo(c, 0);
            b.CopyTo(c, a.Length);
            return c;
        }
    }
}
