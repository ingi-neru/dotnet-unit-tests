namespace DatesAndStuff.Tests
{
    public sealed class SimulationTimeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetupStuff()
        {
            // 
        }

        [SetUp]
        public void Setup()
        {
            // minden teszt felteheti, hogz elotte lefutott ez
        }

        [TearDown]
        public void TearDown()
        {
            // minden teszt utan lefut
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        private SimulationTime sut;

        private SimulationTime GetSut()
        {
            return sut;
        }

        private class ConstructorTests 
        {
            [Test]
            // Default time is not current time.
            public void NoArguments_Constructor_ShouldNotCurrentTime()
            {
                throw new NotImplementedException();
            }
        }

        private class OperatorTests 
        {
            [Test]
            // equal
            // not equal
            // <
            // >
            // <= different
            // >= different 
            // <= same
            // >= same
            // max
            // min
            public void DifferentValues_Operators_ShouldBehaveCorrectly()
            {
                throw new NotImplementedException();
            }
        }

        private class TimeSpanArithmeticTests  
        {
            [Test]
            // TimeSpanArithmetic
            // add
            // substract
            // Given_When_Then
            public void TimeSpan_Addition_ShouldShiftTime()
            {
                // UserSignedIn_OrderSent_OrderIsRegistered
                // DBB, specflow, cucumber, gherkin

                // Arrange
                DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                SimulationTime sut = new SimulationTime(baseDate);

                var ts = TimeSpan.FromMilliseconds(4544313);

                // Act
                var result = sut + ts;

                // Assert
                var expectedDateTime = baseDate + ts;
                Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
            }

            [Test]
            //Method_Should_Then
            public void Subtraction_SimulationTime_Shifted()
            {
                // code kozelibb
                // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
                throw new NotImplementedException();
            }
        }

        private class TimeDifferenceTests 
        {
            [Test]
            // simulation difference timespane and datetimetimespan is the same
            public void DifferentTimes_SubtractingTwoInstances_ShouldReturnCorrectTimeSpan()
            {
                throw new NotImplementedException();
            }
        }

        private class MillisecondRepresentationTests 
        {
            [Test]
            // millisecond representation works
            public void OneMillisecondPerTick_MillisecondRepresentation_ShouldBehaveCorrectly()
            {
                //var t1 = SimulationTime.MinValue.AddMilliseconds(10);
                throw new NotImplementedException();
            }

            [Test]
            // next millisec calculation works /MST
            public void NextMillisec_Increment_ShouldReturnNextMillisecond()
            {
                //Assert.AreEqual(t1.TotalMilliseconds + 1, t1.NextMillisec.TotalMilliseconds);
                throw new NotImplementedException(); 
            }
        }

        private class EqualityAndAdditionTests 
        {
            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void SameMilliseconds_AddingMilliseconds_ShouldRemainEqual()
            {
                throw new NotImplementedException();
            }

            [Test]
            // the same as before just with seconds
            public void SameSeconds_AddingSeconds_ShouldRemainEqual()
            {
                throw new NotImplementedException();
            }

            [Test]
            // same as before just with timespan
            public void SameTimeSpan_AddingTimeSpan_ShouldRemainEqual()
            {
                throw new NotImplementedException();
            }
        }

        private class StringRepresentationTests 
        {
            [Test]
            // check string representation given by ToString
            public void Instance_ToString_ShouldReturnCorrectStringRepresentation()
            {
                throw new NotImplementedException();
            }
        }
    }
}
