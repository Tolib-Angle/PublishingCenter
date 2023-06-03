using System;
using System.Text.Json.Serialization;

namespace PublishingCenter_v2.NHibernate.Data
{
    public class Contracts : Entity
    {
        [JsonPropertyName("id")]
        public virtual int id { get; set; }
        [JsonPropertyName("id_writer")]
        public virtual Writers writer { get; set; }
        [JsonPropertyName("contract_number")]
        public virtual int contract_number { get; set; }
        [JsonPropertyName("date_of_cons_contract")]
        public virtual DateTime date_of_cons_contract { get; set; }
        [JsonPropertyName("term_of_the_contract")]
        public virtual int term_of_the_contract { get; set; }
        [JsonPropertyName("validy_of_the_contract")]
        public virtual bool validy_of_the_contract { get; set; }
        [JsonPropertyName("date_of_terminition_contract")]
        public virtual DateTime date_of_terminition_contract { get; set; }
    }
}
