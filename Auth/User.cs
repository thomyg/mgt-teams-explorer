﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mgt_teams_explorer.Auth
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}
