using System.IO;
using Microsoft.Win32;

namespace VirtualMachineClient.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web.Script.Serialization;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using RestSharp;
    using Models;


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
            this.UploadNewVm = new RelayCommand(this.UploadNewVmExecute, () => true);
        }

        private ObservableCollection<VmInfo> installedVirtualMachines;

        public ObservableCollection<VmInfo> InstalledVirtualMachines
        {
            get
            {
                return this.installedVirtualMachines;
            }
            set
            {
                this.installedVirtualMachines = value;
                this.RaisePropertyChanged("InstalledVirtualMachines");
            }
        }

        private void GetVirtualMachines()
        {
            RestRequest restRequest = new RestRequest("/machines", Method.GET);
            IRestResponse customersResponse = this.restClient.Execute(restRequest);
            this.InstalledVirtualMachines = this.javaScriptSerializer.Deserialize<ObservableCollection<VmInfo>>(customersResponse.Content);
        }

        #region mvvm relay commands
        public ICommand UploadNewVm { get; private set; }

        private void UploadNewVmExecute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "json files (*.json)|*.json";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                RestRequest restRequest = new RestRequest("/machine", Method.PUT);
                restRequest.AddFile(Path.GetFileName(dlg.FileName), File.ReadAllBytes(dlg.FileName), Path.GetFileName(dlg.FileName), "application/json");
                IRestResponse addVmResponse = this.restClient.Execute(restRequest);
            } 
        }
        #endregion
    }
}
