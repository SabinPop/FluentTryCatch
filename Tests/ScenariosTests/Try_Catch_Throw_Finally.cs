using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch_Throw_Finally
{
	[Fact]
	public void Success()
	{
		var actionOrder = new List<int>();
		var tryAction = () => actionOrder.Add(1);
		var catchAction = () => actionOrder.Add(2);
		var finalAction = () => actionOrder.Add(3);
		var actionToTest = Scenarios.TryCatchThrowFinally<ArgumentException, Exception>(tryAction, catchAction, finalAction);
		actionToTest.Should().NotBeNull();

		actionToTest();

		actionOrder.Should().BeEquivalentTo([1, 3]);
	}

	[Fact]
	public void Catches_And_Throws_TException()
	{
		var actionOrder = new List<int>();
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw new ArgumentException();
		};
		var catchAction = () => actionOrder.Add(2);
		var finalAction = () => actionOrder.Add(3);
		var actionToTest = Scenarios.TryCatchThrowFinally<ArgumentException, Exception>(tryAction, catchAction, finalAction);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<Exception>(actionToTest);
		exception.InnerException.Should().BeNull();
		actionOrder.Should().BeEquivalentTo([1, 2, 3]);
	}

	[Fact]
	public void DoesNotCatch_TException()
	{
		var actionOrder = new List<int>();
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw new ArgumentNullException();
		};
		var catchAction = () => actionOrder.Add(2);
		var finalAction = () => actionOrder.Add(3);
		var actionToTest = Scenarios.TryCatchThrowFinally<InvalidCastException, Exception>(tryAction, catchAction, finalAction);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<ArgumentNullException>(actionToTest);
		exception.InnerException.Should().BeNull();
		actionOrder.Should().BeEquivalentTo([1, 3]);
	}
}
