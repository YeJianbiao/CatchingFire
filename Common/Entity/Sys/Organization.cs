using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class Organization : BaseEntity
    {
        [MaxLength(50)]
        [Required, Column("code")]
        public string Code { get; set; }

        [Required, Column("name")]
        public string Name { get; set; }

        [Column("name_en")]
        public string NameEn { get; set; }

        [Column("parent_code")]
        public string ParentCode { get; set; }

        [Column("sequence")]
        public int Sequence { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [Column("updated_time")]
        public string UpdatedBy { get; set; }

        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
    }
}
