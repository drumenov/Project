using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Plumbing.Middlewares.SeedPartTypes
{
    public static class Extensions
    {
        public static IApplicationBuilder UseSeedPartTypesMiddleware(this IApplicationBuilder builder) {
            builder.UseMiddleware<SeedPartTypesMiddleware>();
            return builder;
        }
    }
}
