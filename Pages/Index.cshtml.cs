using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord;
using Disqord.OAuth2;
using EasyCaching.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace discord_oauth_test.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEasyCachingProvider cache;
        public IndexModel(IEasyCachingProvider cache)
        {
            this.cache = cache;
        }

        public bool Authenticated = false;
        public ICurrentUser? CurrentUser;
        public async Task OnGetAsync()
        {
            var token = Request.Cookies["token"];
            if (token != null)
            {
                var clientFactory = HttpContext.RequestServices.GetRequiredService<IBearerClientFactory>();
                IBearerClient client = clientFactory.CreateClient(Token.Bearer(token));
                try
                {
                    var user = await client.FetchCurrentUserAsync();
                    CurrentUser = user;
                    Authenticated = true;
                }
                catch { /* ignored */ }
            }
        }
    }
}
