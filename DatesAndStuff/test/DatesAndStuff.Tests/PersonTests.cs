using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
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

    [Test]
    [CustomPersonCreationAutodataAttribute]
    public void IncreaseSalary_ReasonableValue_ShouldModifySalary(Person sut, double salaryIncreasePercentage)
    {
        // Arrange
        double initialSalary = sut.Salary;

        // Act
        sut.IncreaseSalary(salaryIncreasePercentage);

        // Assert
        sut.Salary.Should().BeApproximately(initialSalary * (100 + salaryIncreasePercentage) / 100, Math.Pow(10, -8), because: "numerical salary calculation might be rounded to conform legal stuff");
    }

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

    [Test]
    public void IncreaseSalary_ZeroPercentIncrease_ShouldNotChange()
    {
        double salaryBeforeIncrease = sut.Salary;

        sut.IncreaseSalary(0);

        //Assert.That(sut.Salary, Is.EqualTo(salaryBeforeIncrease));
        sut.Salary.Should().Be(salaryBeforeIncrease);
    }

    [Test]
    public void IncreaseSalary_NegativeIncrease_ShouldDecrease()
    {
        double percentageIncrease = -9;
        double salaryBeforeIncrease = sut.Salary;
        double expectedSalary = salaryBeforeIncrease + salaryBeforeIncrease * percentageIncrease / 100;

        sut.IncreaseSalary(percentageIncrease);

        //Assert.That(sut.Salary, Is.EqualTo(expectedSalary).Within(0.000001));
        sut.Salary.Should().BeApproximately(expectedSalary, 0.000001);
    }

    [Test]
    public void IncreaseSalary_SmallerThanMinusTenPerc_ShouldFail()
    {
        double percentageIncrease = -10.1;

        //Assert.That(() => sut.IncreaseSalary(percentageIncrease), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());    
        Action act = () => sut.IncreaseSalary(percentageIncrease);
        act.Should().Throw<ArgumentOutOfRangeException>();
    
    }
}
