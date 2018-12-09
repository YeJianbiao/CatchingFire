/**
* 命名空间: 
* 类    名： 
* 创 建 人：
* 创建时间：
* 用    途：
* 
* 
* Copyright (c) . All rights reserved. 
* 版权所有：　　　　　　　　　　　　　　              
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entity.Base;

namespace Entity.Sys
{
    public class Operation:BaseEntity
    {

        [MaxLength(50)]
        [Required, Column("sequence")]
        public string Code { get; set; }

        [MaxLength(250)]
        [Required, Column("name")]
        public string Name { get; set; }

    }
}
