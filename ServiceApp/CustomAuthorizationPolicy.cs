using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        string id = Guid.NewGuid().ToString();

        public string Id
        {
            get { return this.id; }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object list = null;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }

            IIdentity identityToReturn = identities[0];

            evaluationContext.Properties["Principal"] = new MyPrincipal(identityToReturn);

            return true;
        }
    }
}
