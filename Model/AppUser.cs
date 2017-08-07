using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser
    {
        [MaxLength(256)]
        // Extended Properties
        public string FullName { get; set; }
        [MaxLength(256)]
        public DateTime? BirthDay { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string Avatar { get; set; }

        public decimal Balance { get; set; }
        public string Address { get; set; }

        public bool? Gender { get; set; }
        public bool? Status { get; set; }
    }
}
