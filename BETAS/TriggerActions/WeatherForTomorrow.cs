using BETAS.Attributes;
using BETAS.Helpers;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.Extensions;
using StardewValley.Tools;

namespace BETAS.Actions;

public static class WeatherForTomorrow
{
    // Set the weather for a location context for the next day.
    [Action("WeatherForTomorrow")]
    public static bool Action(string[] args, TriggerActionContext context, out string error)
    {
        if (!ArgUtilityExtensions.TryGetTokenizable(args, 1, out var locationContext, out error) || !ArgUtilityExtensions.TryGetTokenizable(args, 2, out var weather, out error))
        {
            error = "Usage: Spiderbuttons.BETAS_WeatherForTomorrow <Context ID> <Weather>";
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