using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
    private static Person sut = PersonFactory.CreateTestPerson();
    [SetUp]
    public void Setup()
    { 

    }


    private class MarryTests : PersonTests{
    [Test]
      public void GotMarried_First_NameShouldChange()
      {
          // Arrange
          string newName = "Test-Eleso Pista";
          double salaryBeforeMarriage = sut.Salary;
          var beforeChanges = Person.Clone(sut);

          // Act
          sut.GotMarried(newName);

          // Assert
          Assert.That(sut.Name, Is.EqualTo(newName)); // act = exp

          sut.Name.Should().Be(newName);
          sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));

          //sut.Salary.Should().Be(salaryBeforeMarriage);

          //Assert.AreEqual(newName, sut.Name); // = (exp, act)
          //Assert.AreEqual(salaryBeforeMarriage, sut.Salary);
      }
    
      [Test]
      public void GotMarried_Second_ShouldFail()
      {
          // Arrange
          string newName = "Test-Eleso-Felallo Pista";
          sut.GotMarried("");

          // Act
          var task = Task.Run(() => sut.GotMarried(""));
          try { task.Wait(); } catch { }

          // Assert
          Assert.IsTrue(task.IsFaulted);
      }

      [Test]
      public void GotMerried_Second_ShouldFail()
      {
          // Arrange
          var fixture = new AutoFixture.Fixture();
          fixture.Customize<IPaymentService>(c => c.FromFactory(() => new TestPaymentService()));

          var sut = fixture.Create<Person>();

          string newName = "Test-Eleso-Felallo Pista";
          sut.GotMarried("");

          // Act
          var task = Task.Run(() => sut.GotMarried(""));
          try { task.Wait(); } catch { }

          // Assert
          Assert.IsTrue(task.IsFaulted);
      }
    }

    [Test]
    [TestCaseSource(nameof(SalaryIncreaseTests))]
    public void IncreaseSalary_ReasonableValue_ShouldModifySalary(Person sut, double salaryIncreasePercentage)
    {
        // Arrange
        double initialSalary = sut.Salary;
        // Act

        // Assert
        Action act = () => sut.IncreaseSalary(salaryIncreasePercentage); 
        if (salaryIncreasePercentage > -10)
        {
            act.Invoke();
            sut.Salary.Should().BeApproximately(initialSalary * (100 + salaryIncreasePercentage) / 100, Math.Pow(10, -8), because: "numerical salary calculation might be rounded to conform legal stuff");
            return;
        }
        act.Should().Throw<ArgumentOutOfRangeException>(because: "salary increase should be reasonable");
    }

    public static TestCaseData[] SalaryIncreaseTests =
    {
        new TestCaseData(PersonFactory.CreateTestPerson(), 0),
        new TestCaseData(PersonFactory.CreateTestPerson(), 10),
        new TestCaseData(PersonFactory.CreateTestPerson(), 10),
        new TestCaseData(PersonFactory.CreateTestPerson(), -10.00001),
        new TestCaseData(PersonFactory.CreateTestPerson(), -9.9999),
        new TestCaseData(PersonFactory.CreateTestPerson(), -10),
        new TestCaseData(PersonFactory.CreateTestPerson(), 100),
        new TestCaseData(PersonFactory.CreateTestPerson(), -11),
    };

    [Test]
    public void Constructor_DefaultParams_ShouldBeAbleToEatChocolate()
    {
        // Arrange

        // Act
        Person sut = PersonFactory.CreateTestPerson();

        // Assert
        sut.CanEatChocolate.Should().BeTrue();
    }

    [Test]
    public void Constructor_DontLikeChocolate_ShouldNotBeAbleToEatChocolate()
    {
        // Arrange

        // Act
        Person sut = PersonFactory.CreateTestPerson(fp => fp.CanEatChocolate = false);

        // Assert
        sut.CanEatChocolate.Should().BeFalse();
    }

}
