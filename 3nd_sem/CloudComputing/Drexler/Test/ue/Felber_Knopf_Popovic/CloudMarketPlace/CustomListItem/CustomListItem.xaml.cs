// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomListItem.xaml.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   Interaction logic for UserControl1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomListItem
{
    using CommonModel;

    /// <summary>
    /// Interaction logic for UserControl1
    /// </summary>
    public partial class CustomListItem
    {
        public VirtualMachine Machine { get; set; }

        public VirtualMachineInstance MachineInstance { get; set; }

        public VirtualAppliance Appliance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomListItem"/> class.
        /// </summary>
        /// <param name="machine">
        /// The machine.
        /// </param>
        public CustomListItem(VirtualMachine machine)
        {
            InitializeComponent();
            
            this.Machine = machine;
            
            lblName.Content = machine.ImageId;
            lblDescription.Text = machine.OperatingSystemName;
            lblRating.Content = machine.Rate;
            lblComments.Content = machine.Comment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomListItem"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public CustomListItem(VirtualMachineInstance instance)
        {
            InitializeComponent();

            this.MachineInstance = instance;

            lblName.Content = instance.InstanceId;
            lblDescription.Text = instance.OperatingSystemName;
            lblRating.Content = instance.Rate;
            lblComments.Content = instance.Comment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomListItem"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public CustomListItem(VirtualAppliance appliance)
        {
            InitializeComponent();

            this.Appliance = appliance;

            lblName.Content = appliance.ImageId;
            lblDescription.Text = appliance.OperatingSystemName;
            lblRating.Content = appliance.Rate;
            lblComments.Content = appliance.Comment;
        }
    }
}
