

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class RoleMenu : BaseEntity
    {

        [MaxLength(50)]
        [Required, Column("role_code")]
        public string RoleCode { get; set; }

        [MaxLength(50)]
        [Required, Column("menu_code")]
        public string MenuCode { get; set; }

    }
}
