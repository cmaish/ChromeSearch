using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace ChromeSearch.Controllers
{
    [AllowAnonymous]
    public class SearchController : ApiController
    {
        public HttpResponseMessage Get(string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                throw new InvalidOperationException("No query");
            }

            var tlds = @"[^\s]+\.local|[^\s]+\.dev";
            var urlpattern = new Regex(@"^(https?://)?(" + tlds + @")(:[0-9]{1,5})?([/\w \.-]*)*\/?$");

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            if (urlpattern.IsMatch(q))
            {
                if (!q.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                {
                    q = "http://" + q;
                }

                response.Headers.Location = new Uri(q);
            }
            else
            {
                var searchQuery = "https://www.google.com/search?q={0}";
                response.Headers.Location = new Uri(string.Format(searchQuery, HttpUtility.UrlEncode(q)));
            }

            return response;
        }
    }
}
