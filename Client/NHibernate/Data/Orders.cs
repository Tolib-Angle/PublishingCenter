using System;
using System.Text.Json.Serialization;

namespace PublishingCenter_v2.NHibernate.Data
{
    public class Orders : Entity
    {
        [JsonPropertyName("id")]
        public virtual int id { get; set; }
        [JsonPropertyName("Customer")]
        public virtual Customers customer { get; set; }
        [JsonPropertyName("order_number")]
        public virtual int order_number { get; set; }
        [JsonPropertyName("date_of_receipt_order")]
        public virtual DateTime date_of_receipt_order { get; set; }
        [JsonPropertyName("order_completion_date")]
        public virtual DateTime order_completion_date { get; set; }
        [JsonPropertyName("Book")]
        public virtual Books book { get; set; }
        [JsonPropertyName("numbers_of_order")]
        public virtual int numbers_of_order { get; set; }
    }

}