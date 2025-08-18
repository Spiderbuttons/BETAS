using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;

namespace BETAS.TriggerActions;

public static class WeatherForTomorrow
{
    // Set the weather for a location context for the next day.
    [Action("WeatherForTomorrow")]
    public static bool Action(string[] args, TriggerActionContext context, out string? error)
    {
        if (!TokenizableArgUtility.TryGet(args, 1, out var locationContext, out error, name: "string Location Context ID") ||
            !TokenizableArgUtility.TryGet(args, 2, out var weather, out error, name: "string Weather"))
        {
            return false;
        }

        weather = weather.ToLower() switch
        {
            "wind" => "Wind",
            "festival" => "Festival",
            "greenrain" => "GreenRain",
            "storm" => "Storm",
            "rain" => "Rain",
            "snow" => "Snow",
            "sun" => "Sun",
            "wedding" => "Wedding",
            _ => weather
        };

        if (string.IsNullOrWhiteSpace(weather))
        {
            error = "Invalid weather type.";
            return false;
        }

        if (locationContext.EqualsIgnoreCase("Default"))
        {
            if (!Utility.isFestivalDay(Game1.dayOfMonth + 1, Game1.season))
            {
                Game1.netWorldState.Value.WeatherForTomorrow = (Game1.weatherForTomorrow = weather);
            }
        }
        else
        {
            Game1.netWorldState.Value.GetWeatherForLocation(locationContext).WeatherForTomorrow = weather;
        }

        return true;
    }
}