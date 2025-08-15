using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.GSQs;

public static class WeatherForTomorrow
{
    // Check whether the weather for tomorrow in a given location context is any of the given weather IDs.
    [GSQ("WEATHER_FOR_TOMORROW")]
    public static bool Query(string[] query, GameStateQueryContext context)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(query, 1, out var locationContext, out var error) || !ArgUtilityExtensions.TryGetTokenizable(query, 2, out var _, out error))
        {
            return GameStateQuery.Helpers.ErrorResult(query, error);
        }

        var weatherTomorrow = locationContext.EqualsIgnoreCase("Default") ? Game1.weatherForTomorrow : Game1.netWorldState.Value.GetWeatherForLocation(locationContext).WeatherForTomorrow;

        return ArgUtilityExtensions.AnyArgMatches(query, 2, (weather) => weatherTomorrow.EqualsIgnoreCase(weather));
    }
}