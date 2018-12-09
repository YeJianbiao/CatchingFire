using System;
using Entity.Base;

namespace Entity.Sys
{
    public class Position : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Sequence { get; set; }

        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
