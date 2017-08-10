using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("Functions")]
    public class Function
    {
        [Key]
        [MaxLength(450)]
        [Column(TypeName = "varchar(450)")]
        public string ID { set; get; }

        [Required]
        [StringLength(50)]
        public string Name { set; get; }

        [Required]
        [MaxLength(256)]
        public string URL { set; get; }

        public int DisplayOrder { set; get; }

        [StringLength(450)]
        [Column(TypeName = "varchar(450)")]
        public string ParentId { set; get; }

        [ForeignKey("ParentId")]
        public virtual Function Parent { set; get; }


        public bool Status { set; get; }

        public string IconCss { get; set; }
    }
}
