using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.ViewModels.System
{
    public class AppUserViewModel
    {

        public string Id { set; get; }
        public string FullName { set; get; }
        public string BirthDay { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public string Address { get; set; }
        public string PhoneNumber { set; get; }
        public string Avatar { get; set; }
        public bool Status { get; set; }
        public bool Gender { get; set; }
        public string Location { get; set; }

        public ICollection<string> Roles { get; set; }

    }
}
