using Fluxor;

namespace UI.Features.Counter.State;

// Immutable state. State is never modified, only replaced with a new version of state
public record CounterState
{
    public int CurrentCount { get; init; }
}

// Feature class sets initial state and provides a name for the feature (used in Redux DevTools)
public class CounterFeature : Feature<CounterState>
{
    public override string GetName() => "Counter";

    protected override CounterState GetInitialState()
    {
        return new CounterState
        {
            CurrentCount = 0
        };
    }
}

// Actions signal that the user intends to change state
public record CounterIncrementAction();

// Reducers are pure functions that take the current state and an action, and return a new state
public static class CounterReducers
{
    [ReducerMethod]
    public static CounterState OnIncrement(CounterState state, CounterIncrementAction _)
    {
        return state with
        {
            CurrentCount = state.CurrentCount + 1
        };
    }
}
