using Fluxor;
using Shared;
using Shared.Queries;
using UI.Features.Counter.State;

namespace UI.Features.Forecast.State;

// Effects are similar to middleware. When an action is dispatched, if there is an effect that can handle that action, the effect will intercept.
public class ForecastEffects
{
    private readonly IBus _bus;
    private readonly IState<CounterState> _counterState;

    // We are injecting in CounterState so we can watch it for changes
    public ForecastEffects(IBus bus, IState<CounterState> counterState)
    {
        _bus = bus;
        _counterState = counterState;
    }

    // Intercept ForecastGetAction and use the Bus to load the weather forecasts
    [EffectMethod]
    public async Task LoadForecasts(ForecastGetAction _, IDispatcher dispatcher)
    {

        /*
         * 
         *  THE "SERVER" CALL
         * 
         *  Not only are there no direct server calls from the UI, there are no indirect calls either!
         *  Here we are using a Bus to dispatch a request to the Gateway.
         *  
         */

        var forecasts = await _bus.Send(new ForecastQuery(DateTime.UtcNow));

        // update our app state with the results (see ForecastReducers.cs)
        dispatcher.Dispatch(new ForecastSetAction(forecasts.ToList()));
    }

    // Watch CounterState and load new weather forecasts every tenth increment
    [EffectMethod(typeof(CounterIncrementAction))]
    public Task LoadForecastsOnIncrement(IDispatcher dispatcher)
    {
        // load weather forecasts every tenth increment
        if (_counterState.Value.CurrentCount % 10 == 0)
        {
            dispatcher.Dispatch(new ForecastGetAction());
        }

        return Task.CompletedTask;
    }
}
