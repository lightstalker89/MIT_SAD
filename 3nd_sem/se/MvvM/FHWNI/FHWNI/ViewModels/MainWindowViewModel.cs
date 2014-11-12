using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Media;
using FHWNI.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FHWNI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            MobilePhoneColors = new List<MobilePhoneColor>
            {
                new MobilePhoneColor { ColorName = "Yellow", FillColor = new SolidColorBrush(Colors.Yellow)},
                 new MobilePhoneColor { ColorName = "Gray", FillColor = new SolidColorBrush(Colors.Gray)},
                  new MobilePhoneColor { ColorName = "Red", FillColor = new SolidColorBrush(Colors.Red)},
                  new MobilePhoneColor { ColorName = "Green", FillColor = new SolidColorBrush(Colors.Green)},
                  new MobilePhoneColor { ColorName = "Blue", FillColor = new SolidColorBrush(Colors.Blue)},
                  new MobilePhoneColor { ColorName = "Silver", FillColor = new SolidColorBrush(Colors.Silver)},
                  new MobilePhoneColor { ColorName = "Black", FillColor = new SolidColorBrush(Colors.Black)},
            };
        }

        IList<MobilePhoneColor> mobilePhoneColors = new List<MobilePhoneColor>();
        public IList<MobilePhoneColor> MobilePhoneColors
        {
            get { return mobilePhoneColors; }
            set
            {
                mobilePhoneColors = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MobilePhone> mobilePhones = new ObservableCollection<MobilePhone>();
        public ObservableCollection<MobilePhone> MobilePhones
        {
            get { return mobilePhones; }
            set
            {
                mobilePhones = value;
                RaisePropertyChanged();
            }
        }

        private string model;
        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                RaisePropertyChanged(() => Model);
            }
        }

        private string manufacurer;
        public string Manufacturer
        {
            get { return manufacurer; }
            set
            {
                manufacurer = value;
                RaisePropertyChanged(() => Manufacturer);
            }
        }

        private RelayCommand addModelCommand;

        public RelayCommand AddModelCommand
        {
            get { return addModelCommand ?? (new RelayCommand(AddModel)); }
        }

        private MobilePhoneColor selectedMobilePhoneColor;

        public MobilePhoneColor SelectedMobilePhoneColor
        {
            get { return selectedMobilePhoneColor; }
            set
            {
                selectedMobilePhoneColor = value;
                RaisePropertyChanged(() => SelectedMobilePhoneColor);
            }
        }

        private void AddModel()
        {
            MobilePhones.Add(new MobilePhone { Manufacturer = Manufacturer, MobilePhoneColor = SelectedMobilePhoneColor.FillColor, Model = Model });
        }
    }
}
