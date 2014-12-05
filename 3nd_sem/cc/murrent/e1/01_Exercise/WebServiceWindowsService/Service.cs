using System;
using System.ServiceProcess;
using Microsoft.Web.Services3.Addressing;
using Microsoft.Web.Services3.Messaging;

namespace WebServiceWindowsService
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            SoapReceivers.Add(new EndpointReference(new Uri("soap.tcp://localhost/SOAPWebService")), typeof(SOAPWebService.SOAPWebService));
        }

        protected override void OnStop()
        {
            SoapReceivers.Clear();
        }
    }
}
