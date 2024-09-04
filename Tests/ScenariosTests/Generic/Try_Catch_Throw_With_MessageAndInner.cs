using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch_Throw_With_MessageAndInner
{
	[Fact]
	public void Success()
	{
		var actionOrder = new List<int>();
		var message = "message";
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, Exception, ArgumentNullException>(tryFunc, catchAction, message);
		funcToTest.Should().NotBeNull();

		funcToTest();
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void Success_WithInner()
	{
		var actionOrder = new List<int>();
		var message = "message";
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, Exception, ArgumentNullException>(tryFunc, catchAction, message, true);
		funcToTest.Should().NotBeNull();

		funcToTest();
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void Catches()
	{
		var actionOrder = new List<int>();
		var message = "message";
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new Exception();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, Exception, ArgumentNullException>(tryFunc, catchAction, message);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<ArgumentNullException>(funcToTest);
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
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw exceptionToThrow;
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, Exception, ArgumentNullException>(tryFunc, catchAction, message, true);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<ArgumentNullException>(funcToTest);
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
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new InvalidOperationException();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, InvalidCastException, ArgumentNullException>(tryFunc, catchAction, message);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<TargetInvocationException>(funcToTest);
		exception.InnerException.Should().NotBeNull();
		exception.InnerException.Should().BeOfType<InvalidOperationException>();
		actionOrder.Should().BeEquivalentTo([1]);
	}

	[Fact]
	public void DoesNotCatch_WithInner()
	{
		var actionOrder = new List<int>();
		var message = "message";
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new InvalidOperationException();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatchThrowWithMessageAndInner<object?, InvalidCastException, ArgumentNullException>(tryFunc, catchAction, message, true);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<TargetInvocationException>(funcToTest);
		exception.InnerException.Should().NotBeNull();
		exception.InnerException.Should().BeOfType<InvalidOperationException>();
		actionOrder.Should().BeEquivalentTo([1]);
	}
}
