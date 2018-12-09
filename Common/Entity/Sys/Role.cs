using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class Role : BaseEntity
    {
        [MaxLength(50)]
        [Required,Column("code")]
        public string Code { get; set; }

        [MaxLength(250)]
        [Required, Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }

        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
    }
}
