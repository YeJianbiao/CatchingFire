using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class UserInfo:BaseEntity
    {
        [MaxLength(250)]
        [Required,Column("user_code")]
        public string Code { get; set; }

        [MaxLength(250)]
        [Required, Column("name")]
        public string Name { get; set; }


        

    }
}
