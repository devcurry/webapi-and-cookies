using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace CookiesInWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        //// GET api/values
        //public HttpResponseMessage Get()
        //{
        //    HttpResponseMessage respMessage = new HttpResponseMessage();
        //    respMessage.Content = new ObjectContent<string []>(new string[] { "value1", "value2" }, new JsonMediaTypeFormatter());
            
        //    CookieHeaderValue cookie = new CookieHeaderValue("session-id", "12345");
        //    cookie.Expires = DateTimeOffset.Now.AddDays(1);
        //    cookie.Domain = Request.RequestUri.Host;
        //    cookie.Path = "/";
        //    respMessage.Headers.AddCookies(new CookieHeaderValue[] { cookie });
        //    return respMessage;
        //}

        /// <summary>
        /// Sample to send multiple values in a single cookie
        /// </summary>
        /// <returns></returns>

        public HttpResponseMessage Get()
        {
            HttpResponseMessage respMessage = new HttpResponseMessage();
            respMessage.Content = new ObjectContent<string[]>(new string[] { "value1", "value2" }, new JsonMediaTypeFormatter());

            var nv = new NameValueCollection();
            nv["sessid"] = "1234";
            nv["3dstyle"] = "flat";
            nv["theme"] = "red";
            var cookie = new CookieHeaderValue("session", nv);

            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";
            respMessage.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            return respMessage;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            string sessionId = "";
            string style = "";
            string theme = "";

            CookieHeaderValue cookie = Request.Headers.GetCookies("session").FirstOrDefault();
            if (cookie != null)
            {
                CookieState cookieState = cookie["session"];

                sessionId = cookieState["sessid"];
                style = cookieState["3dstyle"];
                theme = cookieState["theme"];
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}