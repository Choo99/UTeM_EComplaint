using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApi.Models;
using System.Web;





namespace WebApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context) 
        {
            try
            {
                var client = context.OwinContext.Get<ApplicationClient>("oauth:client");
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
  
                    if (SQLAuth.sqlvalidateUser2(context.UserName.TrimEnd(), context.Password))
                    {
                        if (client.Id == "kk")
                        {
                            // login using other than myutem
                            if (SQLAuth.updatedatabaru_notmyutem(context.UserName, client.Id, client.ClientSecretHash, client.Name) > 0)
                            {
                                // context.Rejected();

                                // SQLAuth.updatedata(context.UserName, client.Id, client.ClientSecretHash, client.Name);
                            }
                            else
                            {
                                try
                                {
                                    var userx = new ApplicationUser() { UserName = context.UserName, Email = context.UserName, clientip = client.Name };
                                    //  IdentityResult result = await userManager.CreateAsync(userx, context.Password);
                                    IdentityResult result = await userManager.CreateAsync(userx, "Abc*&^%$#@!de!1&&" + context.UserName + "084230948jisdhifs!@#$%^&*");
                                }
                                catch (Exception e)
                                {
                                    context.SetError("invalid_grant", "Invalid login domain error.");
                                    return;

                                }


                            }
                        }
                        else
                        {
                            //if (SQLAuth.IsAspUserExist(context.UserName) == true)
                            if (SQLAuth.updatedatabaru(context.UserName, client.Id, client.ClientSecretHash, client.Name) > 0)
                            {
                                // context.Rejected();

                                // SQLAuth.updatedata(context.UserName, client.Id, client.ClientSecretHash, client.Name);
                            }
                            else
                            {
                                try
                                {
                                    var userx = new ApplicationUser() { UserName = context.UserName, Email = context.UserName, clientid = client.Id, sessionid = client.ClientSecretHash, clientip = client.Name };
                                    //  IdentityResult result = await userManager.CreateAsync(userx, context.Password);
                                    IdentityResult result = await userManager.CreateAsync(userx, "Abc*&^%$#@!de!1&&" + context.UserName + "084230948jisdhifs!@#$%^&*");
                                }
                                catch (Exception e)
                                {
                                    context.SetError("invalid_grant", "Invalid login domain error.");
                                    return;

                                }


                            }
                        }
                    }
                    else
                    {
                        context.SetError("invalid_grant", "Invalid login domain.");
                        return;
                    }
              
                try
                {
                    ApplicationUser user = await userManager.FindAsync(context.UserName, "Abc*&^%$#@!de!1&&" + context.UserName + "084230948jisdhifs!@#$%^&*");
                    try
                    {
                        ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                        OAuthDefaults.AuthenticationType);
                        try
                        {
                            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                            CookieAuthenticationDefaults.AuthenticationType);
                            AuthenticationProperties properties = CreateProperties(user.UserName);
                            try
                            {
                                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                                context.Validated(ticket);
                                context.Request.Context.Authentication.SignIn(cookiesIdentity);
                            }
                            catch (Exception e)
                            {
                                context.SetError("invalid_grant", "Invalid login domain error.");
                                return;

                            }
                        }
                        catch (Exception e)
                        {
                            context.SetError("invalid_grant", "Invalid login domain error.");
                            return;

                        }
                    }
                    catch (Exception e)
                    {
                        context.SetError("invalid_grant", "Invalid login domain error.");
                        return;

                    }
                }
                catch (Exception e)
                {
                    context.SetError("invalid_grant", "Invalid login domain error.");
                    return;

                }


            }
            catch (Exception e)
            {
                context.SetError("invalid_grant", "Invalid login domain error.");
                return;

            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            try
            {
                var clientId = string.Empty;
            var clientSecret = string.Empty;


            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Rejected();
                context.SetError("invalid_client", "Client credentials could not be retrieved through the Authorization header.");
                return Task.FromResult<object>(null);
            }


               // if (clientId == "MyApp" && clientSecret == "MySecret")
               // {
                    string clientAddress = "empty";
                clientAddress = GetCustomerIP(); // HttpContext.Current.Request.UserHostAddress;
                                                 //  ClientSecretHash = new PasswordHasher().HashPassword(clientSecret),
                var client = new ApplicationClient
                    {
                        Id = clientId,
                        AllowedGrant = OAuthGrant.ResourceOwner,
                        ClientSecretHash = clientSecret,
                        Name = clientAddress,
                        CreatedOn = DateTimeOffset.UtcNow
                    };

                    context.OwinContext.Set<ApplicationClient>("oauth:client", client);
                    context.Validated(clientId);
                //}
                //else
                //{
                //    // Client could not be validated.
                //    context.SetError("invalid_client", "Client credentials are invalid.");
                //    context.Rejected();
                //}
            }
            catch (Exception)
            {
                context.SetError("server_error");
                context.Rejected();
            }



            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }


        public string GetCustomerIP()


        {


            string CustomerIP = "";


            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)


            {


                CustomerIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();


            }


            else


            {


                CustomerIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();


            }


            return CustomerIP;


        }

    }
}