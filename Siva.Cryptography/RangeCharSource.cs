using System.Text;

namespace Siva.Cryptography
{
    /// <summary>
    /// Represents a character source based on interaction with character ranges.
    /// </summary>
    public partial class RangeCharSource : ICharSource, IEnumerable<char>
    {
        private readonly (char start, char end)[] ranges;

        /// <inheritdoc/>
        public int Count { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeCharSource"/> class with the specified <paramref name="ranges"/>.
        /// </summary>
        /// <param name="ranges">Enumerable of character ranges</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RangeCharSource(IEnumerable<(char start, char end)> ranges) :
            this(ranges.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeCharSource"/> class with the specified <paramref name="ranges"/>.
        /// </summary>
        /// <param name="ranges">Array of character ranges.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RangeCharSource(params (char start, char end)[] ranges)
        {
            ArgumentNullException.ThrowIfNull(ranges);
            if (ranges.Any(range => range.start > range.end))
            {
                throw new ArgumentOutOfRangeException(nameof(ranges),
                    "One of ranges contains invalid sequence, where start character more than end character.");
            }

            this.ranges = new (char start, char end)[ranges.Length];
            ranges.CopyTo(this.ranges, index: 0);

            Count = this.ranges.Sum(range => MeasureLengthOfCharRange(range.start, range.end));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeCharSource"/> class with ranges that will be taken from the <paramref name="stringOfRanges"/> string sequence.
        /// </summary>
        /// <param name="stringOfRanges">A string as an array of character ranges.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public RangeCharSource(string stringOfRanges) : this(CastToArrayOfRanges(stringOfRanges))
        {
        }

        /// <inheritdoc/>
        public char this[int index] => GetChar(index);

        private char GetChar(int index)
        {
            if (index > -1 && index < Count)
            {
                for (var i = 0; i < ranges.Length; i++)
                {
                    (var start, var end) = ranges[i];

                    var shift = index - MeasureLengthOfCharRange(start, end);

                    if (shift < 0)
                    {
                        return (char)(index + start);
                    }
                    index = shift;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(index), index,
                $"Specified {nameof(index)} was out of range ({index}).");
        }

        /// <summary>
        /// Collects all chars from the specified ranges into a string.
        /// </summary>
        /// <returns>A string that contains all chars from the specified ranges.</returns>
        public override string ToString()
        {
            StringBuilder builder = new(Count);

            foreach ((var start, var end) in ranges)
            {
                var length = MeasureLengthOfCharRange(start, end);
                for (var i = 0; i < length; i++)
                {
                    builder.Append((char)(i + start));
                }
            }

            return builder.ToString();
        }

        private static int MeasureLengthOfCharRange(char startChar, char endChar) =>
            endChar - startChar + 1;

        private static IEnumerable<(char startChar, char endChar)> CastToArrayOfRanges(string ranges)
        {
            for (int i = 0; i < ranges.Length; i += 2)
            {
                yield return (startChar: ranges[i],
                    endChar: i + 1 < ranges.Length ?
                        ranges[i + 1] :
                        ranges[i]);
            }
        }
    }
}
