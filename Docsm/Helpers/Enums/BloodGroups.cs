using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Docsm.Helpers.Enums
{
    public enum BloodGroups

    {
        [JsonConverter(typeof(StringEnumConverter))]
        O_Positive = 0,
        [JsonConverter(typeof(StringEnumConverter))]
        O_Negative = 1,
        [JsonConverter(typeof(StringEnumConverter))]
        A_Positive = 2,
        [JsonConverter(typeof(StringEnumConverter))]
        A_Negative = 3,
        [JsonConverter(typeof(StringEnumConverter))]
        B_Positive = 4,
        [JsonConverter(typeof(StringEnumConverter))]
        B_Negative = 5,
        [JsonConverter(typeof(StringEnumConverter))]
        AB_Positive = 6,
        [JsonConverter(typeof(StringEnumConverter))]
        AB_Negative = 7 
    }
}
