using System.Text.Json.Serialization;

namespace PublishingCenter_v2.NHibernate.Data
{
    public class Writers : Entity
    {
        [JsonPropertyName("id")]
        public virtual int id { get; set; }
        [JsonPropertyName("passport_number")]
        public virtual int passport_number { get; set; }
        [JsonPropertyName("surname")]
        public virtual string surname { get; set; }
        [JsonPropertyName("name")]
        public virtual string name { get; set; }
        [JsonPropertyName("middle_name")]
        public virtual string middle_name { get; set; }
        [JsonPropertyName("address")]
        public virtual string address { get; set; }
        [JsonPropertyName("phone")]
        public virtual string phone { get; set; }
    }
}
