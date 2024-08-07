namespace Siva.Cryptography
{
    /// <summary>
    /// Represents a cryptographic key generator based on an input source of chars.
    /// </summary>
    public interface IRandomKeyGenerator
    {
        /// <summary>
        /// Returns an array of characters with random characters. 
        /// The source of the characters is the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source of the characters from which an array will be populated.</param>
        /// <param name="size">Required key length.</param>
        /// <returns>An array populated with random characters from the <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public char[] CreateKey(ICharSource source, int size);
    }
}
