﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserGroup
    {
        public int UserGroupId { get; set; }
        public string GroupName { get; set; }
        public string Type { get; set; }

        public List<ApplicationUser_UserGourp> ApplicationUser_UserGourps { get; set; }
    }
}