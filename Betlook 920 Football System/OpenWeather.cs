using Newtonsoft.Json;
using System;

namespace Betlook_920_Football_System
{
    public partial class Coordinate
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        [JsonProperty("cod")]
        public long Cod { get; set; }

        [JsonProperty("coord")]
        public Coord Coord { get; set; }

        [JsonProperty("dt")]
        public long Dt { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sys")]
        public Sys Sys { get; set; }

        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        [JsonProperty("wind")]
        public Wind Wind { get; set; }
    }

    public partial class Clouds
    {
        [JsonProperty("all", NullValueHandling = NullValueHandling.Ignore)]
        public long? All { get; set; }
    }

    public partial class Coord
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }

    public partial class Main
    {
        [JsonProperty("humidity", NullValueHandling = NullValueHandling.Ignore)]
        public long? Humidity { get; set; }

        [JsonProperty("pressure", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pressure { get; set; }

        [JsonProperty("temp", NullValueHandling = NullValueHandling.Ignore)]
        public double? Temp { get; set; }

        [JsonProperty("temp_max", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempMax { get; set; }

        [JsonProperty("temp_min", NullValueHandling = NullValueHandling.Ignore)]
        public double? TempMin { get; set; }
    }

    public partial class Sys
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public double? Message { get; set; }

        [JsonProperty("sunrise", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sunrise { get; set; }

        [JsonProperty("sunset", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sunset { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }
    }

    public partial class Weather
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("main", NullValueHandling = NullValueHandling.Ignore)]
        public string Main { get; set; }
    }

    public partial class Wind
    {
        [JsonProperty("deg", NullValueHandling = NullValueHandling.Ignore)]
        public long? Deg { get; set; }

        [JsonProperty("gust", NullValueHandling = NullValueHandling.Ignore)]
        public double? Gust { get; set; }

        [JsonProperty("speed", NullValueHandling = NullValueHandling.Ignore)]
        public double? Speed { get; set; }
    }
}
