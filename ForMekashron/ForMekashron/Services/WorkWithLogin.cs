using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ForMekashron.Services
{
    public  class WorkWithLogin
    {
        public static async Task<HttpResponseMessage> CreateRequest(XmlDocument xDoc, string url)
        {
            using (var httpClient = new HttpClient())
            {
                var context = new StringContent(xDoc.InnerXml, Encoding.Default, "text/xml");
                context.Headers.Add("SOAPLogin", "");

                return await httpClient.PostAsync(url, context);
            }
        }
    }
}
