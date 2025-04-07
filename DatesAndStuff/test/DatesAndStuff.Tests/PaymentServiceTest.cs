using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DatesAndStuff.Tests
{
    internal class PaymentServiceTest
    {
        [Test]
        [TestCase(1000.0, true)]
        [TestCase(100.0, false)]
        public void TestPaymentService_ManualMock(double balance, bool expectedResult)
        {
            // Arrange
            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
             new TestPaymentService(),
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             },
             balance
            );

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        [TestCase(500.1, true)]
        [TestCase(499.9, false)]
        public void TestPaymentService_Mock(double balance, bool expectedResult)
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();
            var paymentServiceMock = paymentService.Object;
            

            Person sut = new Person("Test Pista",
                new EmploymentInformation(
                  54,
                  new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                paymentServiceMock
                ,
                new LocalTaxData("4367558"),
                new FoodPreferenceParams()
                {
                CanEatChocolate = true,
                CanEatEgg = true,
                CanEatLactose = true,
                CanEatGluten = true
                },
                balance
                );

            
            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.GetBalance(sut)).Returns(balance);
            if (balance < Person.SubscriptionFee)
                paymentService.InSequence(paymentSequence).Setup(m => m.CancelPayment());
            else
                paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());



            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().Be(expectedResult);
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.GetBalance(sut), Times.Once);
            if (balance < Person.SubscriptionFee)
                paymentService.Verify(m => m.CancelPayment(), Times.Once);
            else
                paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
        }

        [Test]
        [CustomPersonCreationAutodataAttribute]
        public void TestPaymentService_MockWithAutodata(Person sut, Mock<IPaymentService> paymentService)
        {
            // Arrange

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.GetBalance(sut), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
        }
    }
}
