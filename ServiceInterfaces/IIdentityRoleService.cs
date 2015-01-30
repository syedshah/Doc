using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ServiceInterfaces
{
    public interface IIdentityRoleService
    {
        IList<String> GetRoles();
        IList<String> GetRoles(String userId);
        void AddRolesToUser(String userId, IList<String> rolesList);
    }
}
