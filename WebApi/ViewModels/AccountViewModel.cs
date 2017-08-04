using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels
{
    public class AccountViewModel
    {

        public string UserName { get; set; }
        public string Password { get; set; }

        public string FullName { get; set; }

        public DateTime? BirthDay { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string Avatar { get; set; }

        public decimal Balance { get; set; }

        public bool? Gender { get; set; }

    }
}
