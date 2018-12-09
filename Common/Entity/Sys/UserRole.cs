using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class UserRole : BaseEntity
    {
        [MaxLength(50)]
        [Required, Column("user_code")]
        public string UserCode { get; set; }

        [MaxLength(50)]
        [Required, Column("role_code")]
        public string RoleCode { get; set; }

    }
}
