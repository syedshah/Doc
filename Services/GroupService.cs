using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocProcessingRepository.Interfaces;
using Entities;
using ServiceInterfaces;

namespace Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IList<Group> GetGroups()
        {
            return this._groupRepository.GetGroups();
        }
    }
}
