using System.Buffers;
using System.Security.Cryptography;

namespace Siva.Cryptography
{
    /// <summary>
    /// Represents a cryptographic key generator based on an input source of chars.
    /// </summary>
    public class RandomKeyGenerator : IRandomKeyGenerator
    {
        /// <inheritdoc/>
        public char[] CreateKey(ICharSource charSource, int size)
        {
            ArgumentNullException.ThrowIfNull(charSource);

            if (charSource.Count < 1)
            {
                throw new ArgumentException(message:
                    $"The specified \"{nameof(charSource)}\" parameter was empty. Cannot create a key from the empty source.",
                    nameof(charSource));
            }

            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size),
                    $"The specified \"{nameof(size)}\" parameter for {nameof(RandomKeyGenerator)}.{nameof(CreateKey)} was less than zero (0).");
            }

            const int shiftSize = sizeof(int);

            const int targetMaxNumberOfCharsPerIteration = 30;
            var bytes = ArrayPool<byte>.Shared.Rent(Math.Min(size * shiftSize, targetMaxNumberOfCharsPerIteration * shiftSize));
            var lengthOfUsedPartOfBytes = bytes.Length % shiftSize == 0 ? bytes.Length : bytes.Length / shiftSize * shiftSize;

            var numberOfBytesLeftToFill = size * shiftSize;

            var destinationArray = new char[size];
            var destinationCharacterIndex = 0;

            for (var i = 0; i < size; i += lengthOfUsedPartOfBytes / shiftSize)
            {
                var numberOfBytesToFill = Math.Min(lengthOfUsedPartOfBytes, numberOfBytesLeftToFill);
                RandomNumberGenerator.Fill(bytes.AsSpan(0, numberOfBytesToFill));
                numberOfBytesLeftToFill -= numberOfBytesToFill;

                for (int byteOffset = 0; byteOffset < numberOfBytesToFill; byteOffset += shiftSize)
                {
                    var randomNumber = BitConverter.ToUInt32(bytes, byteOffset);
                    var sourceCharacterIndex = (int)(randomNumber % charSource.Count);

                    destinationArray[destinationCharacterIndex++] = charSource[sourceCharacterIndex];
                }
            }

            Array.Fill(bytes, (byte)0);
            ArrayPool<byte>.Shared.Return(bytes);

            return destinationArray;
        }
    }
}
