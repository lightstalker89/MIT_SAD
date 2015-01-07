// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MarketPlaceClient
{
    using CommonModel;
    using MarketPlaceService;
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private MarketPlaceServiceClient service;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            service = new MarketPlaceServiceClient();

            updateAll();
        }

        void start_Click(CommonModel.VirtualMachineInstance instance)
        {
            CommonModel.MarketPlaceServiceResponse resp = service.StartInstance(instance);
            if (!resp.Error)
            {
                MessageBox.Show(instance.InstanceId + " started");
            }
        }

        void stop_Click(CommonModel.VirtualMachineInstance instance)
        {
            CommonModel.MarketPlaceServiceResponse resp = service.StopInstance(instance);
            if (!resp.Error)
            {
                MessageBox.Show(instance.InstanceId + " stopped");
            }
        }

        void changeMachine_Click(VirtualMachine machine)
        {
            var dialog = new ChangeMachineWindow(machine);
            if (dialog.ShowDialog() == true)
            {
                MarketPlaceServiceResponse resp = service.ChangeDescriptionOfVirtualMachine(dialog.Machine);

                if (resp.Error)
                {
                    MessageBox.Show(resp.ErrorMessage);
                }
                updateMachines();
            }
        }
        void downloadMachine_Click(VirtualMachine machine)
        {
            var dialog = new DownloadWindow();
            if (dialog.ShowDialog() == true)
            {

                try
                {
                    string TmpFilePath = dialog.Path;

                    DownloadVirtualMachineResponse resp = service.DownloadVirtualMachine(machine);
                    if (resp.Error)
                    {
                        MessageBox.Show(resp.ErrorMessage);
                        return;
                    }
                    using (var fs = new FileStream(TmpFilePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.Write(resp.ByteArray, 0, resp.ByteArray.Length);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        void rateMachine_Click(VirtualMachine machine)
        {
            var dialog = new RateWindow();
            if (dialog.ShowDialog() == true)
            {
                machine.Rate = (byte)dialog.Rate;
                MarketPlaceServiceResponse resp = service.RateVirtualMachine(machine, machine.Rate);
                if (resp.Error)
                {
                    MessageBox.Show(resp.ErrorMessage);
                }
                updateMachines();
            }
        }

        void commentMachine_Click(VirtualMachine machine)
        {
            var dialog = new CommentWindow();
            if (dialog.ShowDialog() == true)
            {
                machine.Comment = dialog.Comment;
                MarketPlaceServiceResponse resp = service.CommentVirtualMachine(machine, machine.Comment);
                if (resp.Error)
                {
                    MessageBox.Show(resp.ErrorMessage);
                }
                updateMachines();
            }
        }

        void changeAppliance_Click(VirtualAppliance appliance)
        {
            var dialog = new ChangeApplianceWindow(appliance);
            if (dialog.ShowDialog() == true)
            {
                MarketPlaceServiceResponse resp = service.ChangeDescriptionOfVirtualAppliance(dialog.Appliance);

                if (resp.Error)
                {
                    MessageBox.Show(resp.ErrorMessage);
                }
                updateAppliances();
            }
        }

        void downloadAppliance_Click(VirtualAppliance appliance)
        {
            var dialog = new DownloadWindow();
            if (dialog.ShowDialog() == true)
            {

                try
                {
                    string TmpFilePath = dialog.Path;

                    DownloadVirtualApplianceResponse resp = service.DownloadVirtualAppliance(appliance);
                    if (resp.Error)
                    {
                        MessageBox.Show(resp.ErrorMessage);
                        return;
                    }
                    using (var fs = new FileStream(TmpFilePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.Write(resp.ByteArray, 0, resp.ByteArray.Length);
                    }

                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        //void rateAppliance_Click(VirtualAppliance appliance)
        //{ 
        //    var dialog = new RateWindow();
        //    if (dialog.ShowDialog() == true)
        //    {
        //        appliance.Rate = (byte)dialog.Rate;
        //        //service.rate
        //    }
        //}

        //void commentAppliance_Click(VirtualAppliance appliance)
        //{
            
        //}

        private void buttonAddVirtualMachine_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddMachine();
            if (dialog.ShowDialog() == true)
            {
                byte[] tmparray;
                using (var fs = new FileStream(dialog.Path, FileMode.Open))
                {
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    tmparray = buffer;
                }

                MarketPlaceServiceResponse marketPlaceServiceResponse = service.UploadVirtualMachine(dialog.Machine, tmparray);

                if (!marketPlaceServiceResponse.Error)
                {
                    updateMachines();
                }
                else
                {
                    MessageBox.Show(marketPlaceServiceResponse.ErrorMessage);
                }
            }
        }

        private void buttonAddVirtualAppliance_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddAppliance();
            if (dialog.ShowDialog() == true)
            {
                byte[] tmparray;
                using (var fs = new FileStream(dialog.Path, FileMode.Open))
                {
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    tmparray = buffer;
                }

                MarketPlaceServiceResponse marketPlaceServiceResponse = service.UploadVirtualAppliance(dialog.Appliance, tmparray);

                if (!marketPlaceServiceResponse.Error)
                {
                    updateAppliances();
                }
                else
                {
                    MessageBox.Show(marketPlaceServiceResponse.ErrorMessage);
                }
            }
        }

        void updateAppliances()
        {
            virtualApplianceItems.Items.Clear();
            foreach (var element in service.GetVirtualAppliances())
            {
                CustomListItem.CustomListItem item = new CustomListItem.CustomListItem(element);
                item.ContextMenu = new ContextMenu();
                MenuItem change = new MenuItem();
                change.Header = "Change";
                change.Click += (sender, e) => changeAppliance_Click(item.Appliance);
                item.ContextMenu.Items.Add(change);
                MenuItem download = new MenuItem();
                download.Header = "Download";
                download.Click += (sender, e) => downloadAppliance_Click(item.Appliance);
                item.ContextMenu.Items.Add(download);
                //MenuItem rate = new MenuItem();
                //rate.Header = "Rate";
                //rate.Click += (sender, e) => rateAppliance_Click(item.Appliance);
                //item.ContextMenu.Items.Add(rate);
                //MenuItem comment = new MenuItem();
                //comment.Header = "Comment";
                //comment.Click += (sender, e) => commentAppliance_Click(item.Appliance);
                //item.ContextMenu.Items.Add(comment);

                virtualApplianceItems.Items.Add(item);
            }
        }
        void updateMachines()
        {
            virtualMachineItems.Items.Clear();
            foreach (var element in service.GetDummyVirtualMachines())
            {
                CustomListItem.CustomListItem item = new CustomListItem.CustomListItem(element);
                item.ContextMenu = new ContextMenu();
                MenuItem change = new MenuItem();
                change.Header = "Change";
                change.Click += (sender, e) => changeMachine_Click(item.Machine);
                item.ContextMenu.Items.Add(change);
                MenuItem download = new MenuItem();
                download.Header = "Download";
                download.Click += (sender, e) => downloadMachine_Click(item.Machine);
                item.ContextMenu.Items.Add(download);
                MenuItem rate = new MenuItem();
                rate.Header = "Rate";
                rate.Click += (sender, e) => rateMachine_Click(item.Machine);
                item.ContextMenu.Items.Add(rate);
                MenuItem comment = new MenuItem();
                comment.Header = "Comment";
                comment.Click += (sender, e) => commentMachine_Click(item.Machine);
                item.ContextMenu.Items.Add(comment);
                virtualMachineItems.Items.Add(item);
            }
        }

        void updateInstances()
        {
            instanceItems.Items.Clear();
            foreach (var element in service.GetVirtualMachineInstances())
            {
                CustomListItem.CustomListItem item = new CustomListItem.CustomListItem(element);
                item.ContextMenu = new ContextMenu();
                MenuItem start = new MenuItem();
                start.Header = "start";
                start.Click += (sender, e) => start_Click(item.MachineInstance);
                item.ContextMenu.Items.Add(start);
                MenuItem stop = new MenuItem();
                stop.Header = "stop";
                stop.Click += (sender, e) => stop_Click(item.MachineInstance);
                item.ContextMenu.Items.Add(stop);
                instanceItems.Items.Add(item);
            }
        }

        void updateAll()
        {
            updateAppliances();
            updateMachines();
            updateInstances();
        }

        private void buttonSearchVirtualMachine_Click(object sender, RoutedEventArgs e)
        {
            virtualMachineItems.Items.Clear();

            var dialog = new SearchMachineWindow();
            if (dialog.ShowDialog() == true)
            {

                foreach (var element in service.SearchForSpecificVirtualMachine(dialog.Machine))
                {
                    CustomListItem.CustomListItem item = new CustomListItem.CustomListItem(element);
                    item.ContextMenu = new ContextMenu();
                    MenuItem change = new MenuItem();
                    change.Header = "Change";
                    change.Click += (sender1, e1) => changeMachine_Click(item.Machine);
                    item.ContextMenu.Items.Add(change);
                    MenuItem download = new MenuItem();
                    download.Header = "Download";
                    download.Click += (sender1, e1) => downloadMachine_Click(item.Machine);
                    item.ContextMenu.Items.Add(download);
                    MenuItem rate = new MenuItem();
                    rate.Header = "Rate";
                    rate.Click += (sender1, e1) => rateMachine_Click(item.Machine);
                    item.ContextMenu.Items.Add(rate);
                    MenuItem comment = new MenuItem();
                    comment.Header = "Comment";
                    comment.Click += (sender1, e1) => commentMachine_Click(item.Machine);
                    item.ContextMenu.Items.Add(comment);
                    virtualMachineItems.Items.Add(item);
                }
            }
        }

        private void buttonSearchVirtualAppliance_Click(object sender, RoutedEventArgs e)
        {
            virtualApplianceItems.Items.Clear();

            var dialog = new SearchApplianceWindow();
            if (dialog.ShowDialog() == true)
            {
                foreach (var element in service.SearchForSpecificVirtualAppliance(dialog.Appliance))
                {
                    CustomListItem.CustomListItem item = new CustomListItem.CustomListItem(element);
                    item.ContextMenu = new ContextMenu();
                    MenuItem change = new MenuItem();
                    change.Header = "Change";
                    change.Click += (sender1, e1) => changeAppliance_Click(item.Appliance);
                    item.ContextMenu.Items.Add(change);
                    MenuItem download = new MenuItem();
                    download.Header = "Download";
                    download.Click += (sender1, e1) => downloadAppliance_Click(item.Appliance);
                    item.ContextMenu.Items.Add(download);

                    virtualApplianceItems.Items.Add(item);
                }
            }
        }
    }
}
