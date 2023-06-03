using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using PublishingCenter_v2.NHibernate;

namespace PublishingCenter_v2.NHibernate.Data
{
    public class Books : Entity
    {
        [JsonPropertyName("id")]
        public virtual int id { get; set; }
        [JsonPropertyName("cipher_of_the_book")]
        public virtual int cipher_of_the_book { get; set; }
        private IList<Writers> _writers;
        [JsonPropertyName("writers")]
        public virtual IList<Writers> writers
        {
            get
            {
                if (_writers == null)
                    _writers = new List<Writers>();
                return _writers;
            }
            set => _writers = value;
        }
        [JsonPropertyName("name")]
        public virtual string name { get; set; }
        [JsonPropertyName("title")]
        public virtual string title { get; set; }
        [JsonPropertyName("circulation")]
        public virtual int circulation { get; set; }
        [JsonPropertyName("release_date")]
        public virtual DateTime release_date { get; set; }
        [JsonPropertyName("cost_price")]
        public virtual float cost_price { get; set; }
        [JsonPropertyName("sale_price")]
        public virtual float sale_price { get; set; }
        [JsonPropertyName("fee")]
        public virtual float fee { get; set; }
    }
}
