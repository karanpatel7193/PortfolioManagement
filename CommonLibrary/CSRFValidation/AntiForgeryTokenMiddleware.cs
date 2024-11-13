using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CommonLibrary.CSRFValidation
{
    public class AntiForgeryTokenMiddleware : IMiddleware
    {
        private readonly IAntiforgery _antiforgery;
        private readonly IConfiguration _configuration;

        public AntiForgeryTokenMiddleware(IAntiforgery antiforgery,IConfiguration configuration)
        {
            _antiforgery = antiforgery;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (MyConvert.ToBoolean(_configuration["IgnoreCSRF"])){
                await next(context);
                return;
            }
            List<string> urls = (string.IsNullOrEmpty(_configuration["IgnoreUrls"]))?new List<string>():_configuration["IgnoreUrls"].Split(',').ToList();

            var isGetRequest = true;
            for (int i = 0; i < urls.Count; i++)
            {
                if (context.Request.Path.ToString().ToLower().Contains(urls[i].ToLower()))
                {
                    isGetRequest = false;
                }
            }
            if (isGetRequest)
            {
                try
                {
                    //bool isValid = await _antiforgery.IsRequestValidAsync(context);
                    await _antiforgery.ValidateRequestAsync(context);
                    //if (isValid) {
                        await next(context);
                    //}
                    //else
                    //{
                    //    Log.WriteLogFile("Invalidate Url -> " + context.Request.Path);
                    //}
                }
                catch (Exception ex)
                {
                    Log.WriteLogFile("context.Request.Path => " + context.Request.Path);
                    Log.WriteLogFile("Referer -> " + context.Request.Headers["Referer"].ToString() + " | " + "User-Agent -> " + context.Request.Headers["User-Agent"].ToString() + " | " + "Cookie -> " + context.Request.Headers["Cookie"].ToString() + " | " + "XSRF-Token -> " + context.Request.Headers["X-XSRF-TOKEN"].ToString() + " | " + "Origin -> " + context.Request.Headers["Origin"].ToString() + " | " + "Failed Url -> " + context.Request.Path);
                    
                    context.Response.StatusCode = 400;
                    var response = new { message = "Invalid XSRF Token" };
                    await context.Response.WriteAsJsonAsync(response);
                }

            }
            else
            {
                await next(context);
            }

        }
    }
}