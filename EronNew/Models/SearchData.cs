using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EronNew.Models
{
    public class SearchDataResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public SearchDataResolver()
        {
            PropertyMappings = new Dictionary<string, string>
            {
                {"SaleCategory","Category is"
                },{"TypeDesc","Type = "
                },{"SubType","SubType = "
                },{"Area","County/Region = "
                },{"SubArea","Municipality/District = "
                },{"EnergyEfficiency","Energy Class = "
                },{"Bathroom","Bathroom"
                },{"Bedroom","Bedroom"
                },{"squaresFrom","Squares starts from = "
                },{"squaresTo","Squares ends to = "
                },{"priceFrom","Price starts from = "
                },{"priceTo","Price ends to = "
                },{"conYearFrom","Construction Year starts from = "
                },{"conYearTo","Construction Year ends to = "
                },{"renYearFrom","Renovation Year starts from = "
                },{"renYearTo","Renovation Year ends to = "
                },{"PetAllowed","Pet Allows filter is"
                },{"ParkingArea","Parking Area filter is"
                },{"AirCondition","Air Condition filter is"
                },{"Bbq","Bbq filter is"
                },{"Elevator","Elevator filter is"
                },{"Fireplace","Fireplace filter is"
                },{"Garden","Garden filter is"
                },{"Gym","Gym filter is"
                },{"Hall","Hall filter is"
                },{"Heating","Heating filter is"
                },{"HeatingSystem","Heating System filter is"
                },{"Kitchen","Kitchen filter is"
                },{"Livingroom","Living Room filter is"
                },{"Maidroom","Maidroom filter is"
                },{"Master","Master filter is"
                },{"RoofFloor","Penthouse filter is"
                },{"SemiOutdoor","Semi Outdoor filter is"
                },{"Storageroom","Storage Room filter is"
                },{"Swimmingpool","Swimming Pool filter is"
                },{"Wc","WC filter is"
                },{"SecureDoor","Secure Door filter is"
                },{"AluminumFrames","Aluminum Frames filter is"
                },{"HomeAlarm","Home Alarm filter is"
                },{"FTTH","Internet Fiber filter is"
                },{"HouseKeepingMoney","HouseKeeping Money filter is"
                }
            };
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            var resolved = PropertyMappings.TryGetValue(property.PropertyName, out string resolvedName);
            property.PropertyName = resolved ? resolvedName : property.PropertyName;
            property.UnderlyingName = resolved ? resolvedName : property.PropertyName;

            return property;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            var resolved = PropertyMappings.TryGetValue(propertyName, out string resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
    public class SearchDataConverter : JsonConverter
    {
        public SearchDataConverter()
        {
        }


        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(SearchData))
                return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return "";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {


                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                o.Remove("numberOfPage");
                o.Remove("SortList");
                List<string> toBeRemoved = new List<string>();
                var properties = o.Properties();
                foreach (JProperty item in properties)
                {
                    if (item.Value.GetType().Name == typeof(JValue).Name)
                    {
                        if (item.Value.Value<string>() == null)
                        {
                            toBeRemoved.Add(item.Name);
                        }
                    }
                    else if (item.Value.GetType().Name == typeof(JArray).Name)
                    {
                        if (item.Value.Value<IEnumerable<object>>().ToList().Count == 0)
                        {
                            toBeRemoved.Add(item.Name);
                        }
                    }

                }
                foreach (var item in toBeRemoved)
                {
                    o.Remove(item);
                }
                o.WriteTo(writer);
            }
        }
    }
    public class SearchData
    {
        public SearchData()
        {
            numberOfPage = 1;
            SubType = new List<int>();
            SubArea = new List<long>();
        }
        public string SaleCategory { get; set; }
        public string TypeDesc { get; set; }
        public List<int> SubType { get; set; }
        public int Area { get; set; }
        public List<long> SubArea { get; set; }
        public string EnergyEfficiency { get; set; }
        public AspNetUserProfile CardProfile { get; set; }
        public int? Bathroom { get; set; }
        public int? Bedroom { get; set; }
        public int? squaresFrom { get; set; }
        public int? squaresTo { get; set; }
        public int? priceFrom { get; set; }
        public int? priceTo { get; set; }
        public int? conYearFrom { get; set; }
        public int? conYearTo { get; set; }
        public int? renYearFrom { get; set; }
        public int? renYearTo { get; set; }
        public string PetAllowed { get; set; }
        public string ParkingArea { get; set; }
        public string AirCondition { get; set; }
        public string Bbq { get; set; }
        public string Elevator { get; set; }
        public string Fireplace { get; set; }
        public string Garden { get; set; }
        public int? GardenSpace { get; set; }
        public string Gym { get; set; }
        public string Hall { get; set; }
        public string Heating { get; set; }
        public string HeatingSystem { get; set; }
        public string Kitchen { get; set; }
        public string Livingroom { get; set; }
        public string Maidroom { get; set; }
        public string Master { get; set; }
        public string RoofFloor { get; set; }
        public string SemiOutdoor { get; set; }
        public int? SemiOutdoorSquare { get; set; }
        public string Storageroom { get; set; }
        public int? StorageroomSquare { get; set; }
        public string Swimmingpool { get; set; }
        public string Wc { get; set; }
        public string SecureDoor { get; set; }
        public string AluminumFrames { get; set; }
        public string HomeAlarm { get; set; }
        public string FTTH { get; set; }
        public string HouseKeepingMoney { get; set; }

        [JsonIgnore]
        public int numberOfPage { get; set; }
        [JsonIgnore]
        public string SortList { get; set; }
    }
}
