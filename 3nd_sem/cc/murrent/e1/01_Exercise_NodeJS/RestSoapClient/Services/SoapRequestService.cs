using System;
using System.IO;
using System.Net;
using System.Xml;

namespace RestSoapClient.Services
{
    public static class SoapRequestService
    {
        public static string CallWebService(string action, string parameter)
        {
            string responseString = "";
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(action, parameter);
            HttpWebRequest webRequest = CreateWebRequest("http://localhost:1337/SOAPWebService", action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            webRequest.BeginGetResponse(delegate(IAsyncResult asynchronousResult)
            {
                HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                responseString = streamRead.ReadToEnd();
                streamResponse.Close();
                streamRead.Close();
                response.Close();
            }, webRequest);

            return responseString;
        }

        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.KeepAlive = false;
            webRequest.Timeout = 300000;
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string action, string parameter)
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""><SOAP-ENV:Header></SOAP-ENV:Header><SOAP-ENV:Body><" + action + ">" + parameter + "</" + action + "></SOAP-ENV:Body></SOAP-ENV:Envelope>");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}
