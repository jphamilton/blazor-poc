using Fluxor;

namespace UI.Features.Forecast.State;

public static class ForecastReducers
{
    // When an action has a payload, we need to use a method with a parameter
    [ReducerMethod]
    public static ForecastState OnSetForecasts(ForecastState state, ForecastSetAction action)
    {
        return state with
        {
            Forecasts = action.Forecasts,
            Initialized = true,
            IsLoading = false
        };
    }


    // Since the action does not have a payload, we can use an attribute
    [ReducerMethod(typeof(ForecastGetAction))]
    public static ForecastState OnGetForecasts(ForecastState state)
    {
        return state with
        {
            IsLoading = true,
            Initialized = false
        };
    }

}