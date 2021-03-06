﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ContactBook.WebApi.Common;
using ContactBook.Domain.Contexts;
using ContactBook.Domain.IoC;

namespace ContactBook.WebApi.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        
        public ApplicationOAuthProvider()
        {
            _publicClientId = "Self";
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserManager<IdentityUser> userManager = context.OwinContext.GetUserManager<UserManager<IdentityUser>>())
            {
                IdentityUser user = await userManager.FindAsync(context.UserName, context.Password);
                
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                else
                {
                    bool emailConfirmed = await userManager.IsEmailConfirmedAsync(user.Id);
                    if (!emailConfirmed)
                    {
                        context.SetError("invalid_grant", "Email not confirmed. Please verify your email address.");
                        return;
                    }
                    else
                    {

                        user.Claims.Add(new IdentityUserClaim() { ClaimType = "BookId", ClaimValue = "101" });
                    }
                }

                ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
                    context.Options.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                    CookieAuthenticationDefaults.AuthenticationType);
                AuthenticationProperties properties = CreateProperties(user.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
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
            if (context.ClientId == null)
            {
                context.Validated();
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
            data.Add("bookId", GetContactBookNumber(userName));
        
            return new AuthenticationProperties(data);
        }

        public static string GetContactBookNumber(string userName)
        {
            var cbContext = DependencyFactory.Resolve<IContactBookContext>();
            var cbInfo = cbContext.GetContactBook(userName);
            if (cbInfo != null)
            {
                return cbInfo.BookId.ToString();
            }
            return "0";
        }
    }
}