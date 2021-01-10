using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Formatter
    {
        /// <summary>
        /// Returns username based on the Windows Logon Name. 
        /// </summary>
        /// <param name="winLogonName"> Windows logon name can be formatted either as a UPN (<username>@<domain_name>) or a SPN (<domain_name>\<username>) </param>
        /// <returns> username </returns>
        
        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };
            

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
               
                if (parts[0].Contains("wcfs"))
                {
                    return parts[0].Replace("wcfs", "WCFS");
                    
                }
                else
                    return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                
                if (parts[1].Contains("wcfs"))
                {
                    return parts[1].Replace("wcfs", "WCFS");
                    
                }
                else
                    return parts[1];
                
            }
            else
            {
                return winLogonName;
            }
        }

        public static string ParseGroup(string name)
        {
            string group = "";


            group = name.Substring(name.IndexOf("OU=")).Split(' ')[0];
            group = group.Substring(group.IndexOf("=") + 1);
            group = group.Remove(group.Length - 1);


            return group;

        }
    }
}
