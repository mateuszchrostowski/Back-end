using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET23.Model.Identities
{
    public class ApplicationUser : IdentityUser
    {
        public string? MyProperty { get; set; }
    }
}
