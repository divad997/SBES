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
        public IIdentity id;

        public IIdentity Identity
        {
            get
            {
                return id;
            }
        }

        public bool IsInRole(string role)
        {

            WindowsIdentity wi = id as WindowsIdentity;
            foreach (IdentityReference u in wi.Groups)
            {

                string toBeSearched = "\\";
                string current_group = (u.Translate(typeof(NTAccount)).ToString()).Substring((u.Translate(typeof(NTAccount)).ToString()).IndexOf(toBeSearched) + toBeSearched.Length);
                if (current_group == Roles.User)
                {
                    if (RolesConfig.KOR.Contains(role))
                    {
                        return true;
                    }
                }
                else if (current_group == Roles.VIP)
                {
                    if (RolesConfig.VIP.Contains(role))
                    {
                        return true;
                    }
                }
                else if (current_group == Roles.Admin)
                {
                    if (RolesConfig.ADM.Contains(role))
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        public MyPrincipal()
        {

        }
        public MyPrincipal(IIdentity wi)
        {
            id = wi;
        }
    }
}
