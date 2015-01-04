using Microsoft.Win32;

namespace VirtualMachineClient.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Web.Script.Serialization;
    using System.Windows;
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
            this.ExitCommand = new RelayCommand(this.ExitApplication, () => true);
        }

        private string errorText = String.Empty;
        public string ErrorText
        {
            get
            {
                return errorText;
            }
            set
            {
                errorText = value;
                RaisePropertyChanged("ErrorText");
            }
        }

        private string operatingSystemSearchText = String.Empty;

        public string OperatingSystemSearchText
        {
            get
            {
                return operatingSystemSearchText;
            }
            set
            {
                operatingSystemSearchText = value;
                RaisePropertyChanged("OperatingSystemSearchText");
                Search();
            }
        }

        private string typeSearchText = String.Empty;

        public string TypeSearchText
        {
            get
            {
                return typeSearchText;
            }
            set
            {
                typeSearchText = value;
                RaisePropertyChanged("TypeSearchText");
                Search();
            }
        }

        private VmInfo selectedVmInfo;

        public VmInfo SelectedVmInfo
        {
            get
            {
                return selectedVmInfo;
            }
            set
            {
                selectedVmInfo = value;
                RaisePropertyChanged("SelectedVmInfo");
            }
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
            RestRequest restRequest = new RestRequest("machines", Method.GET);
            IRestResponse getVirtualMachinesResponse = this.restClient.Execute(restRequest);
            SuccessResponse addVmSuccessResponse = this.javaScriptSerializer.Deserialize<SuccessResponse>(getVirtualMachinesResponse.Content);
            if (addVmSuccessResponse.Success)
            {
                this.InstalledVirtualMachines = new ObservableCollection<VmInfo>(addVmSuccessResponse.Data);
            }
            else
            {
                ErrorText = addVmSuccessResponse.ErrorMessage;
            }
        }

        #region mvvm relay commands
        public ICommand UploadNewVm { get; private set; }

        private void UploadNewVmExecute()
        {
            OpenFileDialog dlg = new OpenFileDialog { Filter = "json files (*.json)|*.json" };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string fileContent;
                using (StreamReader str = new StreamReader(dlg.FileName))
                {
                    fileContent = str.ReadToEnd();
                }

                RestRequest restRequest = new RestRequest("machine", Method.POST);
                restRequest.AddJsonBody(fileContent);
                IRestResponse addVmResponse = this.restClient.Execute(restRequest);
                SuccessResponse addVmSuccessResponse = this.javaScriptSerializer.Deserialize<SuccessResponse>(addVmResponse.Content);
                if (addVmSuccessResponse.Success)
                {
                    this.InstalledVirtualMachines = new ObservableCollection<VmInfo>(addVmSuccessResponse.Data);
                }
                else
                {
                    ErrorText = addVmSuccessResponse.ErrorMessage;
                }
            }
        }

        public ICommand ExitCommand { get; private set; }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private void Search()
        {
            RestRequest restRequest = new RestRequest("machine/{operatingsystem}/{type}", Method.GET);
            string operatingSystem = (OperatingSystemSearchText == String.Empty) ? "all" : OperatingSystemSearchText;
            string type = (TypeSearchText == String.Empty) ? "all" : TypeSearchText;
            restRequest.AddUrlSegment("operatingsystem", operatingSystem);
            restRequest.AddUrlSegment("type", type);
            IRestResponse getVirtualMachinesResponse = this.restClient.Execute(restRequest);
            SuccessResponse addVmSuccessResponse = this.javaScriptSerializer.Deserialize<SuccessResponse>(getVirtualMachinesResponse.Content);
            if (addVmSuccessResponse.Success)
            {
                this.InstalledVirtualMachines = new ObservableCollection<VmInfo>(addVmSuccessResponse.Data);
            }
            else
            {
                ErrorText = addVmSuccessResponse.ErrorMessage;
            }
        }
        #endregion
    }
}
