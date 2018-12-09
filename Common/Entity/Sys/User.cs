using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;
using Newtonsoft.Json;

namespace Entity.Sys
{
    public class User:BaseEntity
    {
        [JsonProperty("Name")]
        [MaxLength(250)]
        [Required, Column("name")]
        public string Name { get; set; }

        [JsonProperty("pwd")]
        [Required, Column("pwd")]
        public string Pwd { get; set; }

        [JsonProperty("UserCode")]
        [Column("user_code")]
        public string UserCode { get; set; }

        [JsonProperty("Status")]
        [Column("status")]
        public string Status { get; set; }

        [JsonProperty("Description")]
        [Column("description")]
        public string Description { get; set; }

        [JsonProperty("CreatedBy")]
        [Column("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("CreatedTime")]
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("UpdatedBy")]
        [Column("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("UpdatedTime")]
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }

    }
}
