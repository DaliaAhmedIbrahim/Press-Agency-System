using Press_Agency_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Press_Agency_System.ViewModels
{
    public class UserAndUserRolesViewModel
    {
        public User User { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }

    }
}