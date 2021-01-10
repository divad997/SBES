using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    class MyAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            WindowsIdentity identity = operationContext.ServiceSecurityContext.WindowsIdentity;
            MyPrincipal principal = new MyPrincipal(identity);
            return true;
        }
    }
}
