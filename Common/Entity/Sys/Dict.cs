using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class Dict:BaseEntity
    {
        [MaxLength(50)]
        [Required, Column("code")]
        public string Code { get; set; }

        [MaxLength(250)]
        [Column("value")]
        public string Value { get; set; }

        [MaxLength(250)]
        [Column("value_1")]
        public string Value1 { get; set; }

        [MaxLength(250)]
        [Column("value_2")]
        public string Value2 { get; set; }

        [MaxLength(250)]
        [Column("value_3")]
        public string Value3 { get; set; }


    }
}
