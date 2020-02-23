using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SelecTunes.Backend.Helper.Middleware;
using SelecTunes.Backend.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper.Middleware
{
    public class UserDestroyerMiddleware
    {
        private readonly RequestDelegate _next;

        public UserDestroyerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // How it works: It signs the user out if they have been lockedout.
        public async Task Invoke(HttpContext httpContext,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (signInManager == null)
            {
                throw new ArgumentNullException(nameof(signInManager));
            }

            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (httpContext.User != null && httpContext.User is ClaimsPrincipal p)
            {
                User user = await userManager.GetUserAsync(p).ConfigureAwait(false);

                if (user.LockoutEnd > DateTimeOffset.Now)
                {
                    //Log the user out and redirect back to homepage
                    await signInManager.SignOutAsync().ConfigureAwait(false);
                    httpContext.Response.Redirect("/");
                }
            }

            await _next(httpContext).ConfigureAwait(false);
        }
    }
}

public static class UserDestroyerMiddlewareExtensions
{
    public static IApplicationBuilder UseUserDestroyer(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserDestroyerMiddleware>();
    }
}