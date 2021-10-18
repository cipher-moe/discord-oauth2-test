using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace discord_oauth_test.Pages
{
    public class Login : PageModel
    {
        private readonly HttpClient httpClient;

        public Login(HttpClient client)
        {
            httpClient = client;
        }

        public async Task<IActionResult> OnGet()
        {
            var code = HttpContext.Request.Query["code"].First();
            if (string.IsNullOrWhiteSpace(code))
            {
                return Redirect("/");
            }


            var content = new FormUrlEncodedContent(new KeyValuePair<string?, string?>[]
            {
                new("client_id", Environment.GetEnvironmentVariable("CLIENT_ID")),
                new("client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET")),
                new("grant_type", "authorization_code"),
                new("code", code),
                new("redirect_uri", Environment.GetEnvironmentVariable("REDIRECT_URI"))
            });
            content.Headers.Clear();
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            try
            {
                var res = await httpClient.PostAsync("https://discord.com/api/oauth2/token", content);
                var json = await res.Content.ReadAsStringAsync();
                var token = JObject.Parse(json)["access_token"]!.ToString();
                HttpContext.Response.Cookies.Append("token", token,
                    new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
                return Page();
            }
            catch
            {
                return StatusCode((int) HttpStatusCode.Forbidden);
            }
        }
    }
}