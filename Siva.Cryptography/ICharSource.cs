namespace Siva.Cryptography
{
    /// <summary>
    /// Represents the source of the characters.
    /// </summary>
    public interface ICharSource
    {
        /// <summary>
        /// Gets the number of all characters.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the character at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the character to get.</param>
        /// <returns>A character at the specified <paramref name="index"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If the <paramref name="index"/> is less than 0 or greater than the value of <see cref="Count"/>, 
        /// then this should throw an exception.
        /// </exception>
        char this[int index] { get; }
    }
}
