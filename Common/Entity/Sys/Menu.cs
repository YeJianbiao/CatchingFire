using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class Menu : BaseEntity
    {
        [MaxLength(50)]
        [Required, Column("code")]
        public string Code { get; set; }

        [MaxLength(250)]
        [Required, Column("name")]
        public string Name { get; set; }

        [MaxLength(250)]
        [Required, Column("name_en")]
        public string NameEn { get; set; }

        [MaxLength(50)]
        [Required, Column("icon")]
        public string Icon { get; set; }

        [MaxLength(50)]
        [Column("parent_code")]
        public string ParentCode { get; set; }

        [Column("sequence")]
        public int Sequence { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("tag")]
        public string Tag { get; set; }

        [Column("is_common_use")]
        public bool IsCommonUse { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public string status { get; set; }

        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_time")]
        public DateTime? CreatedTime { get; set; }

        [Column("updated_by")]
        public string UpdatedBy { get; set; }

        [Column("updated_time")]
        public DateTime? UpdatedTime { get; set; }

    }
}
