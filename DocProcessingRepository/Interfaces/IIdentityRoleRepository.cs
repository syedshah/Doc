using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DocProcessingRepository.Interfaces
{
    public interface IIdentityRoleRepository
    {
        IList<String> GetRoles();
    }
}
