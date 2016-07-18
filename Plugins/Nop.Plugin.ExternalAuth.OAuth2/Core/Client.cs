using DotNetOpenAuth.AspNet.Clients;
using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public abstract class Client : OAuth2Client
    {
        protected Client(string providerName) : base(providerName)
        {
        }

        public abstract Uri GenerateLocalCallbackUri(IWebHelper webHelper);
        public abstract Uri GenerateServiceLoginUrl(Uri returnUrl);
    }
}
