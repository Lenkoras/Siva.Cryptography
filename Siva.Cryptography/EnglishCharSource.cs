namespace Siva.Cryptography
{
    /// <summary>
    /// Represents a character source with digits and all characters of the English alphabet.
    /// </summary>
    public class EnglishCharSource : RangeCharSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnglishCharSource"/> class with digits (0-9), the English alphabet in lowercase (a-z), and uppercase (A-Z).
        /// </summary>
        public EnglishCharSource() : base("09azAZ")
        {
        }
    }
}
