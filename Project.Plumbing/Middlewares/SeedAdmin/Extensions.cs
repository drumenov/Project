using Microsoft.AspNetCore.Builder;

namespace Project.Plumbing.Middlewares.SeedAdmin
{
    public static class Extensions
    {
        public static IApplicationBuilder UseSeedAdminMiddleware(this IApplicationBuilder builder) {
            builder.UseMiddleware<SeedAdminMiddleware>();
            return builder;
        }
    }
}
