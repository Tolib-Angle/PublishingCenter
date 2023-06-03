using System.Text.Json.Serialization;

namespace PublishingCenter_v2.NHibernate.Data
{
    public class Customers : Entity
    {
        [JsonPropertyName("id")]
        public virtual int id { get; set; }
        [JsonPropertyName("customer_name")]
        public virtual string customer_name { get; set; }
        [JsonPropertyName("address")]
        public virtual string address { get; set; }
        [JsonPropertyName("phone")]
        public virtual string phone { get; set; }
        [JsonPropertyName("full_name_customer")]
        public virtual string full_name_customer { get; set; }
    }
}
