using System.Collections.Generic;

namespace Domain.Entities
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
        public UserGroupType Type { get; set; }

        public List<ApplicationUser_UserGroup> ApplicationUser_UserGroups { get; set; }
    }

    public enum UserGroupType { Married, Unmarried, Divorced}
}
