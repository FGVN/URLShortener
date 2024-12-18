﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using URLShortener.Infrastructure.Data;

namespace URLShortener.Infrastructure.MiddleWares;

public class RedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/" + GlobalConstants.ApiPathPrefix))
        {
            using (var scope = context.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var shortUrl = context.Request.Path.Value?.TrimStart('/');

                var url = await dbContext.URLs.FirstOrDefaultAsync(x => x.Url == shortUrl && !x.IsDeleted);

                if (url != null)
                {
                    context.Response.Redirect(url.OriginUrl);
                    return; 
                }
            }
        }

        await _next(context);
    }
}
