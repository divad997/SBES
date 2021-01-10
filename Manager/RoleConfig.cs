﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class RoleConfig
    {
        public static bool GetPermissions(string rolename, out string[] permissions)
        {
            permissions = new string[10];
            string permissionString = string.Empty;

            permissionString = (string)RolesConfig.ResourceManager.GetObject(rolename);
            if (permissionString != null)
            {
                permissions = permissionString.Split(',');
                return true;
            }
            return false;

        }
    }
}
