using System.Collections;

namespace Siva.Cryptography
{
    public partial class RangeCharSource
    {
        public IEnumerator<char> GetEnumerator()
        {
            return new RangeCharSourceEnumerator(ranges);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct RangeCharSourceEnumerator((char start, char end)[] ranges) : IEnumerator<char>
        {
            private int rangeIndex;
            private int currentCharOffset = -1;

            public readonly char Current => (char)(ranges[rangeIndex].start + currentCharOffset);

            readonly object IEnumerator.Current => Current;

            public void Dispose()
            {
                ranges = null!;
            }

            public bool MoveNext()
            {
                ObjectDisposedException.ThrowIf(ranges is null, this);

                if (rangeIndex < ranges.Length)
                {
                    var (start, end) = ranges[rangeIndex];
                    int lengthOfRange = MeasureLengthOfCharRange(start, end);
                    if (currentCharOffset + 1 < lengthOfRange)
                    {
                        currentCharOffset++;
                        return true;
                    }
                    else if (rangeIndex + 1 < ranges.Length)
                    {
                        rangeIndex++;
                        currentCharOffset = 0;
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                rangeIndex = 0;
                currentCharOffset = -1;
            }
        }
    }
}
