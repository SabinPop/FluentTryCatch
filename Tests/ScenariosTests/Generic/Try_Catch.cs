using FluentAssertions;
using FluentTryCatch.Scenarios;
using System.Reflection;

namespace Tests.ScenariosTests.Generic;

public class Try_Catch
{
	[Fact]
	public void Success_Func_Action()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatch(tryFunc, catchAction);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(1);
	}

	[Fact]
	public void Success_Func_Action_TException()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatch<int?, ArgumentNullException>(tryFunc, catchAction);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(1);
	}

	[Fact]
	public void Success_Func_Func()
	{
		var actionOrder = new List<int>();
		var tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchFunc = () =>
		{
			actionOrder.Add(2);
			return 2;
		};

		var funcToTest = Scenarios.TryCatch(tryFunc, catchFunc);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(1);
	}

	[Fact]
	public void Success_Func_Func_TException()
	{
		var actionOrder = new List<int>();
		var tryFunc = () =>
		{
			actionOrder.Add(1);
			return 1;
		};
		var catchFunc = () =>
		{
			actionOrder.Add(2);
			return 2;
		};

		var funcToTest = Scenarios.TryCatch<int, ArgumentNullException>(tryFunc, catchFunc);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(1);
	}

	[Fact]
	public void Catches_Func_Action()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new Exception();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatch(tryFunc, catchAction);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().BeNull();
		actionOrder.Should().HaveCount(2);
		actionOrder.Should().BeInAscendingOrder();
	}

	[Fact]
	public void Catches_Func_Action_TException()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new ArgumentNullException();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatch<int?, ArgumentNullException>(tryFunc, catchAction);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().BeNull();
		actionOrder.Should().HaveCount(2);
		actionOrder.Should().BeInAscendingOrder();
	}

	[Fact]
	public void Catches_Func_Func()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new Exception();
		};
		Func<int?> catchFunc = () =>
		{
			actionOrder.Add(2);
			return 1;
		};

		var funcToTest = Scenarios.TryCatch(tryFunc, catchFunc);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(2);
		actionOrder.Should().BeInAscendingOrder();
	}

	[Fact]
	public void Catches_Func_Func_TException()
	{
		var actionOrder = new List<int>();
		Func<int?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new ArgumentNullException();
		};
		Func<int?> catchFunc = () =>
		{
			actionOrder.Add(2);
			return 1;
		};

		var funcToTest = Scenarios.TryCatch<int?, ArgumentNullException>(tryFunc, catchFunc);
		funcToTest.Should().NotBeNull();

		var result = funcToTest();
		result.Should().Be(1);
		actionOrder.Should().HaveCount(2);
		actionOrder.Should().BeInAscendingOrder();
	}

	[Fact]
	public void DoesNotCatch_Func_Action_TException()
	{
		var actionOrder = new List<int>();
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new NullReferenceException();
		};
		var catchAction = () => actionOrder.Add(2);

		var funcToTest = Scenarios.TryCatch<object?, ArgumentNullException>(tryFunc, catchAction);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<TargetInvocationException>(funcToTest);
		exception.InnerException.Should().NotBeNull();
		exception.InnerException.Should().BeOfType<NullReferenceException>();
		actionOrder.Should().HaveCount(1);
		actionOrder.Should().BeInAscendingOrder();
	}

	[Fact]
	public void DoesNotCatch_Func_Func_TException()
	{
		var actionOrder = new List<int>();
		Func<object?> tryFunc = () =>
		{
			actionOrder.Add(1);
			throw new NullReferenceException();
		};
		Func<object?> catchFunc = () =>
		{
			actionOrder.Add(2);
			return 1;
		};

		var funcToTest = Scenarios.TryCatch<object?, ArgumentNullException>(tryFunc, catchFunc);
		funcToTest.Should().NotBeNull();

		var exception = Assert.Throws<TargetInvocationException>(funcToTest);
		exception.InnerException.Should().NotBeNull();
		exception.InnerException.Should().BeOfType<NullReferenceException>();
		actionOrder.Should().HaveCount(1);
		actionOrder.Should().BeInAscendingOrder();
	}
}
