namespace Siva.Cryptography.Tests
{
    public class RangeCharSourceTests
    {
        [Fact]
        public void TestToString()
        {
            // Arrange
            RangeCharSource charSource = new((start: '1', end: '3'), ('a', 'c'));
            const string expectedResult = "123abc";

            // Act
            var stringRepresentation = charSource.ToString();

            // Assert
            stringRepresentation.Should().BeEquivalentTo(expectedResult);
        }

        [Theory, InlineData("09azAZ", 62)]
        public void TestRangeCharSourceEnumerator(string ranges, int expectedNumberOfChars)
        {
            // Arrange
            RangeCharSource charSource = new(ranges);

            // Act
            var arrayOfChars = charSource.ToArray();

            // Assert
            arrayOfChars.Should().NotBeNull()
                .And.HaveCount(expectedNumberOfChars);
        }

        [Theory, InlineData("09", 5, '5'), InlineData("az", 15, 'p')]
        public void TestGetByIndex(string ranges, int charIndex, char expectedCharResult)
        {
            // Arrange
            RangeCharSource charSource = new(ranges);

            // Act
            var characterByIndex = charSource[charIndex];

            // Assert
            characterByIndex.Should().BeEquivalentTo(expectedCharResult);
        }

        [Fact]
        public void TestGetByIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            RangeCharSource charSource = new("09");
            const int indexOutOfRange = 28;

            Action action = () =>
            {
                // Act
                var value = charSource[indexOutOfRange];
            };

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}