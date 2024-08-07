namespace Siva.Cryptography.Tests
{
    public class RandomKeyGeneratorTests
    {
        [Theory, InlineData(10), InlineData(40)]
        public void TestCreateKey(int expectedKeySize)
        {
            // Arrange
            RandomKeyGenerator keyGenerator = new();
            EnglishCharSource charSource = new();
            string charSourceText = charSource.ToString();

            // Act
            char[] key = keyGenerator.CreateKey(charSource, expectedKeySize);

            // Assert
            key.Should().NotBeNull()
                .And.HaveCount(expectedKeySize)
                .And.AllSatisfy(keyChar => charSourceText.Contains(keyChar));
        }

        [Fact]
        public void TestCreateKey_ThrowsArgumentException()
        {
            // Arrange
            RandomKeyGenerator keyGenerator = new();
            RangeCharSource charSource = new((IEnumerable<(char, char)>)[]);

            Action action = () =>
            {
                // Act
                keyGenerator.CreateKey(charSource, 10);
            };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TestCreateKey_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            RandomKeyGenerator keyGenerator = new();
            EnglishCharSource charSource = new();

            Action action = () =>
            {
                // Act
                keyGenerator.CreateKey(charSource, -1);
            };

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}