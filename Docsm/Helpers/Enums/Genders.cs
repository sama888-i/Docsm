using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Docsm.Helpers.Enums
{
    public enum Genders
    {
        [JsonConverter(typeof(StringEnumConverter))]
        Male =0,
        [JsonConverter(typeof(StringEnumConverter))]
        Female =1
    }
}
