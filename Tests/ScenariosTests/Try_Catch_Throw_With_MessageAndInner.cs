using FluentAssertions;
using FluentTryCatch.Scenarios;

namespace Tests.ScenariosTests;

public class Try_Catch_Throw_With_MessageAndInner
{
	[Fact]
	public void Success()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var tryAction = () =>
		{
			actionOrder.Add(1);
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<Exception, ArgumentNullException>(tryAction, catchAction, message);
		actionToTest.Should().NotBeNull();

		actionToTest();
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void Success_WithInner()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var tryAction = () =>
		{
			actionOrder.Add(1);
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<Exception, ArgumentNullException>(tryAction, catchAction, message, true);
		actionToTest.Should().NotBeNull();

		actionToTest();
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void Catches()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw new Exception();
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<Exception, ArgumentNullException>(tryAction, catchAction, message);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<ArgumentNullException>(actionToTest);
		exception.Message.Should().Be(message);
		exception.InnerException.Should().BeNull();
		actionOrder.Should().BeEquivalentTo([1, 2]);
	}

	[Fact]
	public void Catches_WithInner()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var exceptionToThrow = new Exception();
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw exceptionToThrow;
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<Exception, ArgumentNullException>(tryAction, catchAction, message, true);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<ArgumentNullException>(actionToTest);
		exception.Message.Should().Be(message);
		exception.InnerException.Should().NotBeNull();
		exception.InnerException.Should().BeSameAs(exceptionToThrow);
		actionOrder.Should().BeEquivalentTo([1, 2]);
	}

	[Fact]
	public void DoesNotCatch()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw new InvalidOperationException();
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<InvalidCastException, ArgumentNullException>(tryAction, catchAction, message);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<InvalidOperationException>(actionToTest);
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void DoesNotCatch_WithInner()
	{
		var actionOrder = new List<int>();
		var message = "message";
		var tryAction = () =>
		{
			actionOrder.Add(1);
			throw new InvalidOperationException();
		};
		var catchAction = () => actionOrder.Add(2);

		var actionToTest = Scenarios.TryCatchThrowWithMessageAndInner<InvalidCastException, ArgumentNullException>(tryAction, catchAction, message, true);
		actionToTest.Should().NotBeNull();

		var exception = Assert.Throws<InvalidOperationException>(actionToTest);
		exception.InnerException.Should().BeNull();
		actionOrder.Should().BeEquivalentTo([1]);
	}
}
