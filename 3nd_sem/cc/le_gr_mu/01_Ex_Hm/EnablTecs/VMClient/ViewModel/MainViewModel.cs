namespace VirtualMachineClient.ViewModel
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Web.Script.Serialization;
    using System.Windows;
    using System.Windows.Input;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Microsoft.Win32;
    using Models;
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
            this.UploadNewVm = new RelayCommand(this.UploadNewVmExecute, () => true);
            this.ExitCommand = new RelayCommand(this.ExitApplication, () => true);
            this.SaveRatingCommand = new RelayCommand(this.SaveRating, () => true);
        }

        private string errorText = string.Empty;

        public string ErrorText
        {
            get
            {
                return this.errorText;
            }
            set
            {
                this.errorText = value;
                this.RaisePropertyChanged("ErrorText");
            }
        }

        private string operatingSystemSearchText = string.Empty;

        public string OperatingSystemSearchText
        {
            get
            {
                return this.operatingSystemSearchText;
            }
            set
            {
                this.operatingSystemSearchText = value;
                this.RaisePropertyChanged("OperatingSystemSearchText");
                this.Search();
            }
        }

        private string typeSearchText = string.Empty;

        public string TypeSearchText
        {
            get
            {
                return this.typeSearchText;
            }
            set
            {
                this.typeSearchText = value;
                this.RaisePropertyChanged("TypeSearchText");
                this.Search();
            }
        }

        private VmInfo selectedVmInfo;

        public VmInfo SelectedVmInfo
        {
            get
            {
                return this.selectedVmInfo;
            }
            set
            {
                this.selectedVmInfo = value;
                this.RaisePropertyChanged("SelectedVmInfo");
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
                this.ErrorText = string.Empty;
            }
            else
            {
                this.ErrorText = addVmSuccessResponse.ErrorMessage;
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
                    this.ErrorText = string.Empty;
                }
                else
                {
                    this.ErrorText = addVmSuccessResponse.ErrorMessage;
                }
            }
        }

        public ICommand ExitCommand { get; private set; }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        public ICommand SaveRatingCommand { get; private set; }

        private void SaveRating()
        {
            RestRequest restRequest = new RestRequest("machine/{id}/{rating}/{comment}", Method.POST);
            restRequest.AddUrlSegment("id", SelectedVmInfo.Id);
            restRequest.AddUrlSegment("rating", SelectedVmInfo.Rating);
            restRequest.AddUrlSegment("comment", SelectedVmInfo.RatingDescription);
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

        private void Search()
        {
            RestRequest restRequest = new RestRequest("machine/{operatingsystem}/{type}", Method.GET);
            string operatingSystem = (this.OperatingSystemSearchText == string.Empty) ? "all" : this.OperatingSystemSearchText;
            string type = (this.TypeSearchText == string.Empty) ? "all" : this.TypeSearchText;
            restRequest.AddUrlSegment("operatingsystem", operatingSystem);
            restRequest.AddUrlSegment("type", type);
            IRestResponse getVirtualMachinesResponse = this.restClient.Execute(restRequest);
            SuccessResponse addVmSuccessResponse = this.javaScriptSerializer.Deserialize<SuccessResponse>(getVirtualMachinesResponse.Content);
            if (addVmSuccessResponse.Success)
            {
                this.InstalledVirtualMachines = new ObservableCollection<VmInfo>(addVmSuccessResponse.Data);
                this.ErrorText = string.Empty;
            }
            else
            {
                this.ErrorText = addVmSuccessResponse.ErrorMessage;
            }
        }

        #endregion
    }
}
