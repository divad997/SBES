using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class MyPrincipal : IPrincipal
    {
        private GenericIdentity identity = null;
        private string group = string.Empty;

        public MyPrincipal(GenericIdentity genericIdentity)
        {
            this.identity = genericIdentity;

            group = Formatter.ParseGroup(identity.Name);
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string permission)
        {
            string[] permissions;

            if (RoleConfig.GetPermissions(group, out permissions))
            {
                foreach (string permision in permissions)
                {
                    if (permision.Equals(permission))
                        return true;
                }
            }
            return false;
        }
    }
}
