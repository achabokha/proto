using Embily.Gateways.DHL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Embily.Gateways.DHL
{
    public class DHLAPI
    {
        //httpWebRequest.ContentType = "application/json";
	       // httpWebRequest.Method = "POST";
	       // httpWebRequest.Headers["postmen-api-key"] = "8fc7966b-679b-4a57-911d-c5a663229c9e";

        const string _url = "https://xmlpi-ea.dhl.com/XMLShippingServlet";
        const string _siteId = "v62_1RBRNHTs5X";
        const string _pass = "qejeMaAN1x";
        const string _account = "560564253";

        public DHLAPI()
        {
        }

        /// <summary>
        /// Calculate rates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<dynamic> CalculateRates(RequestCalculateRates request)
        {
            request.PDctRequest.GetQuote.Request.ServiceHeader.SiteId = _siteId;
            request.PDctRequest.GetQuote.Request.ServiceHeader.Password = _pass;
            request.PDctRequest.GetQuote.BkgDetails.PaymentAccountNumber = _account;

            var jsonText = JsonConvert.SerializeObject(request); 
            var postData = JsonConvert.DeserializeXmlNode(jsonText);
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postData.InnerXml);

            var req = (HttpWebRequest)WebRequest.Create(_url);

            req.ContentType = "text/xml";
            req.Method = "POST";
            req.ContentLength = bytes.Length;

            using (Stream os = req.GetRequestStream())
            {
                os.Write(bytes, 0, bytes.Length);
            }

            string response = "";

            using (System.Net.WebResponse resp = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    response = sr.ReadToEnd().Trim();//ReadToEndAsync();//.Trim();/*ReadToEnd()*/
                }
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);

            string responseJson = JsonConvert.SerializeXmlNode(doc);
            
            return JsonConvert.DeserializeObject(responseJson);
        }

    }
}
