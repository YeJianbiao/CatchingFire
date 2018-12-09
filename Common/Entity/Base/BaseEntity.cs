using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

    }
}
