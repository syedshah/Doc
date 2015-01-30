using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DocProcessingRepository.Interfaces
{
    public interface IGroupRepository
    {
        IList<Group> GetGroups();
    }
}
