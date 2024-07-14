using System.Security.Claims;

namespace e_ticaret.Models
{
    public class CustomRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomRedirectMiddleware(RequestDelegate next) {  _next = next; }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                var requestPath = context.Request.Path.ToString().ToLower();

                if (requestPath.Contains("/admin/home"))
                {
                    context.Response.Redirect("/Admin/Login/");
                    return;
                }
                else if (requestPath.Contains("/admin/message/"))
                {
                    context.Response.Redirect("/Admin/Login/");
                    return;
                }
                else if (requestPath.Contains("/admin/product/"))
                {
                    context.Response.Redirect("/Admin/Login/");
                    return;
                }
                else if (requestPath.Contains("/admin/profile/"))
                {
                    context.Response.Redirect("/Admin/Login/");
                    return;
                }
                else if (requestPath.Contains("/admin/category/"))
                {
                    context.Response.Redirect("/Admin/Login/");
                    return;
                }
                else
                {
                    await _next(context);
                    return;
                }
            }
            else
            {
                var requestPath = context.Request.Path.ToString().ToLower();

                if (requestPath.Contains("/admin/home"))
                {
                    var ClaimRole = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                    if (ClaimRole == null)
                    {
                        context.Response.Redirect("/Access/Denied/");
                        return;
                    }
                    var role = ClaimRole.Value;
                    if (role == "Admin")
                    {
                        await _next(context);
                        return;
                    }
                    context.Response.Redirect("/Access/Denied/");
                    return;
                }
                else if (requestPath.Contains("/admin/message/"))
                {
                    var ClaimRole = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                    if (ClaimRole == null)
                    {
                        context.Response.Redirect("/Access/Denied/");
                        return;
                    }
                    var role = ClaimRole.Value;
                    if (role == "Admin")
                    {
                        await _next(context);
                        return;
                    }
                    context.Response.Redirect("/Access/Denied/");
                    return;
                }
                else if (requestPath.Contains("/admin/profile/"))
                {
                    var ClaimRole = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                    if (ClaimRole == null)
                    {
                        context.Response.Redirect("/Access/Denied/");
                        return;
                    }
                    var role = ClaimRole.Value;
                    if (role == "Admin")
                    {
                        await _next(context);
                        return;
                    }
                    context.Response.Redirect("/Access/Denied/");
                    return;
                }
                else if (requestPath.Contains("/admin/category/"))
                {
                    var ClaimRole = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                    if (ClaimRole == null)
                    {
                        context.Response.Redirect("/Access/Denied/");
                        return;
                    }
                    var role = ClaimRole.Value;
                    if (role == "Admin")
                    {
                        await _next(context);
                        return;
                    }
                    context.Response.Redirect("/Access/Denied/");
                    return;
                }
                else if (requestPath.Contains("/admin/product/"))
                {
                    var ClaimRole = context.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role);
                    if (ClaimRole == null)
                    {
                        context.Response.Redirect("/Access/Denied/");
                        return;
                    }
                    var role = ClaimRole.Value;
                    if (role == "Admin")
                    {
                        await _next(context);
                        return;
                    }
                    context.Response.Redirect("/Access/Denied/");
                    return;
                }
                else
                {
                    await _next(context);
                    return;
                }
            }

         
        }
    }
}
