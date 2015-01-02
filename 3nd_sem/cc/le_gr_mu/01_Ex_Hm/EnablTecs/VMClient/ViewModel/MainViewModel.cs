using System;
using System.Collections.Generic;
using VirtualMachineClient.Models;

namespace VirtualMachineClient.ViewModel
{
    using System.Windows.Input;
    using System.Web.Script.Serialization;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using RestSharp;


    public class MainViewModel : ViewModelBase
    {
        private const string EndPoint = "http://localhost:1337";

        private JavaScriptSerializer javaScriptSerializer;

        private RestClient restClient;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.InitRestClient();
            this.InitMethods();
            this.GetVirtualMachines();
        }

        private void InitRestClient()
        {
            this.javaScriptSerializer = new JavaScriptSerializer();
            this.restClient = new RestClient(EndPoint);
        }

        private void InitMethods()
        {
            this.UploadNewVm = new RelayCommand(this.uploadNewVmExecute, () => true);
        }

        private void GetVirtualMachines()
        {
            RestRequest restRequest = new RestRequest("/machines", Method.GET);
            IRestResponse customersResponse = this.restClient.Execute(restRequest);
            List<VmInfo> customersContent = this.javaScriptSerializer.Deserialize<List<VmInfo>>(customersResponse.Content);
            Console.WriteLine("Receive objects");
        }

        #region mvvm relay commands
        public ICommand UploadNewVm { get; private set; }

        private void uploadNewVmExecute()
        {
            RestRequest restRequest = new RestRequest("/machine", Method.POST);
            restRequest.AddFile("vmExample.json", "../../vmExample.json");
            IRestResponse addVmResponse = this.restClient.Execute(restRequest);
           
        }
        #endregion
    }
}