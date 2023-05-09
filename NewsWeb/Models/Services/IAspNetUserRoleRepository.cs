using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsWeb.Models.Services
{
    public interface IAspNetUserRoleRepository
    {
        string GetRoleId(string userId);
    }
}
