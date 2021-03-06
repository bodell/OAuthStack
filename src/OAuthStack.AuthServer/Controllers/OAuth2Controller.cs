﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using OAuthStack.AuthServer.OAuth2;
using OAuthStack.Common.Security;
using OAuthStack.FakeServices;

namespace OAuthStack.AuthServer.Controllers {
    public class OAuth2Controller : Controller {
        private readonly AuthorizationServer authServer;

        public OAuth2Controller() {
            // In this example, we're just newing up an auth server. A real implementation would use an IOC container
            // to resolve the dependencies and inject the auth server into our controller.
            var authServerKeys = CryptoKeyPair.LoadCertificate(HostingEnvironment.MapPath("~/bin/Certificates/auth-server.pfx"), "p@ssw0rd");
            var dataServerKeys = CryptoKeyPair.LoadCertificate(HostingEnvironment.MapPath("~/bin/Certificates/data-server.pfx"), "p@ssw0rd");
            var exampleAuthServer = new ExampleAuthorizationServer(new FakeCryptoKeyStore(),
                authServerKeys, dataServerKeys, new FakeOAuth2ClientStore(), new FakeUserStore());
            this.authServer = new AuthorizationServer(exampleAuthServer);
        }

        public ActionResult Token() {
            var result = this.authServer.HandleTokenRequest(this.Request);
            return (result.AsActionResult());
        }
    }
}
