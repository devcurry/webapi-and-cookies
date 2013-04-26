using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web;
using System.Collections;

namespace CookiesInWebApi.Handlers
{
    public class RequestStampCookieHandler : DelegatingHandler
    {
        static public string CookieStampToken = "cookie-stamp";
        protected async override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(
         HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            string cookie_stamp;
            var cookie = request.Headers.GetCookies(CookieStampToken).FirstOrDefault();
            if (cookie == null)
            {
                cookie_stamp = "COOKIE_STAMPER_" + Guid.NewGuid().ToString();
            }
            else
            {
                cookie_stamp = cookie[CookieStampToken].Value;
                try
                {
                    Guid guid = Guid.Parse(cookie_stamp.Substring(22));
                }
                catch (FormatException)
                {
                    // Invalid Stamp! Create a new one.
                    cookie_stamp = "COOKIE_STAMPER_" + Guid.NewGuid().ToString();
                }
            }
            // Store the session ID in the request property bag.
            request.Properties[CookieStampToken] = cookie_stamp;
            // Continue processing the HTTP request.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            // Set the session ID as a cookie in the response message.
            response.Headers.AddCookies(new CookieHeaderValue[] {
               new CookieHeaderValue(CookieStampToken, cookie_stamp) 
              });
            return response;
        }
    }
}