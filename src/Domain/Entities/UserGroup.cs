using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
        public UserGroupType Type { get; set; }

        public List<ApplicationUserUserGroup> ApplicationUserUserGroups { get; set; }
    }

    public enum UserGroupType { Married, Unmarried, Divorced}
}
