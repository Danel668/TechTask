using ForMekashron.Models;
using ForMekashron.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ForMekashron.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel vm)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                            <env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" 
                            xmlns:ns1=""urn:ICUTech.Intf-IICUTech"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                            xmlns:enc=""http://www.w3.org/2003/05/soap-encoding""> 
                                <env:Body>
                                    <ns1:Login env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                                        <UserName xsi:type=""xsd:string""></UserName>
                                        <Password xsi:type=""xsd:string""></Password>
                                        <IPs xsi:type=""xsd:string""></IPs>
                                    </ns1:Login>
                                </env:Body> 
                            </env:Envelope>"

            );

            XmlElement xmlElement = xDoc.DocumentElement;
            if (xmlElement != null)
            {
                XmlNode body = xmlElement.LastChild;

                if (body != null)
                {
                    XmlNode login = body.LastChild;
                    if (login != null)
                    {
                        XmlNodeList list = login.ChildNodes;
                        if (list != null)
                        {
                            foreach (XmlNode node in list)
                            {
                                if (node.Name == "UserName")
                                    node.InnerText = vm.UserName;

                                if (node.Name == "Password")
                                    node.InnerText = vm.Password;
                            }
                        }
                    }
                }

            }
            HttpResponseMessage response = await WorkWithLogin
                .CreateRequest(xDoc, @"http://isapi.icu-tech.com/icutech-test.dll/soap/IICUTech");
            string content = await response.Content.ReadAsStringAsync();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(content);
            XmlElement xRoot = xmlDocument.DocumentElement;

            User Me = null;
            if (xRoot != null)
                Me = JsonSerializer.Deserialize<User>(xRoot.InnerText);
            

            if (Me.ResultCode == 1)
                return RedirectToAction("Result", "Home", Me);

             else
                return RedirectToAction("Error", "Home");
            
        }

        public IActionResult Result(User vm)
        {
            return View(vm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
