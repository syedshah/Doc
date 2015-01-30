using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocProcessingRepository.Interfaces;
using Exceptions;
using IdentityWrapper.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceInterfaces;

namespace Services
{
    public class IdentityRoleService : IIdentityRoleService
    {
        private readonly IIdentityRoleRepository _identityRoleRepository;
        private readonly IUserManagerProvider _userManagerProvider;

        public IdentityRoleService(IIdentityRoleRepository identityRoleRepository, IUserManagerProvider userManagerProvider)
        {
            this._identityRoleRepository = identityRoleRepository;
            _userManagerProvider = userManagerProvider;
        }

        public IList<String> GetRoles()
        {
            try
            {
                return this._identityRoleRepository.GetRoles();
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve roles", e);
            }
        }

        public IList<string> GetRoles(string userId)
        {
            try
            {
                return this._userManagerProvider.GetRoles(userId);
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to retrieve roles", e);
            }
        }


        public void AddRolesToUser(string userId, IList<string> rolesList)
        {
            try
            {
                this._userManagerProvider.GetRoles(userId).ToList().ForEach(r => this._userManagerProvider.RemoveFromRole(userId, r));
                rolesList.ToList().ForEach(r => this._userManagerProvider.AddToRole(userId, r));
            }
            catch (Exception e)
            {
                throw new DocProcessingException("Unable to add roles to the user.", e);
            }
        }
    }
}
