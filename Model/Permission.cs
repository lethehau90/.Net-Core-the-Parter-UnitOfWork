using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int ID { get; set; }

        [StringLength(450)]
        public string RoleId { get; set; }

        [StringLength(450)]
        [Column(TypeName = "varchar(450)")]
        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        [ForeignKey("RoleId")]
        public AppRole AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public Function Function { get; set; }
    }
}
