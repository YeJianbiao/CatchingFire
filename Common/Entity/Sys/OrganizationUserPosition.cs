
using Entity.Base;

namespace Entity.Sys
{
    public class OrganizationUserPosition : BaseEntity
    {

        public string OrganizationCode { get; set; }

        public string UserCode { get; set; }

        public string PositionCode { get; set; }

    }
}
