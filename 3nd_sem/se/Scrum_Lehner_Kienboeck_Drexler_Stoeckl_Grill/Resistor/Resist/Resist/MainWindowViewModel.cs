using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Resist
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        IColorCalculator toolColor = new ColorsByResist();
        IResistCalculator toolCalc = new ResistCalculatorHelper();

        public MainWindowViewModel()
        {
            this.colors = new Dictionary<int, Brush>();
            this.colors.Add(1, Brushes.Red);
            this.colors.Add(2, Brushes.Red);
            this.colors.Add(3, Brushes.Red);
            this.colors.Add(4, Brushes.Red);
            this.colors.Add(5, Brushes.Red);

            this.ResistValue = 0;
        }

        private double resistValue;

        public double ResistValue
        {
            get
            {
                return resistValue;
            }
            set
            {
                resistValue = value;
                if (this.toolColor != null)
                {
                    if (resistValue >= 0.0 && resistValue < 100000.0)
                    {
                        Brush[] bs = this.toolColor.GetColorsByResist(resistValue);
                        this.colors[1] = bs[0];
                        this.colors[2] = bs[1];
                        this.colors[3] = bs[2];
                        this.colors[4] = bs[3];
                        this.colors[5] = bs[4];
                    }
                }

                OnPropertyChanged("ResistValue");
                OnPropertyChanged("CurrentColor1");
                OnPropertyChanged("CurrentColor2");
                OnPropertyChanged("CurrentColor3");
                OnPropertyChanged("CurrentColor4");
                OnPropertyChanged("CurrentColor5");
            }
        }

        private RelayCommand setColor = null;

        public RelayCommand SetColor
        {
            get
            {
                return setColor ?? (setColor = new RelayCommand((x) =>
                {
                    ColorsWindow window = new ColorsWindow();
                    window.ShowDialog();

                    Brush b = Brushes.Tomato;
                    int pos = int.Parse((string)x);

                    switch (window.ReturnValue)
                    {
                        case 0:
                            b = Brushes.Black;
                            break;
                        case 1:
                            b = Brushes.Brown;
                            break;
                        case 2:
                            b = Brushes.Red;
                            break;
                        case 3:
                            b = Brushes.Orange;
                            break;
                        case 4:
                            b = Brushes.Yellow;
                            break;
                        case 5:
                            b = Brushes.Green;
                            break;
                        case 6:
                            b = Brushes.Blue;
                            break;
                        case 7:
                            b = Brushes.Violet;
                            break;
                        case 8:
                            b = Brushes.Gray;
                            break;
                        case 9:
                            b = Brushes.White;
                            break;
                    }

                    colors[pos] = b;
                    if (this.toolCalc != null)
                    {
                        this.ResistValue = this.toolCalc.GetResist(
                            colors[1],
                            colors[2],
                            colors[3],
                            colors[4],
                            colors[5]);
                    };
                    OnPropertyChanged("CurrentColor" + pos.ToString());
                }));
            }
        }

        Dictionary<int, Brush> colors = new Dictionary<int, Brush>();

        public Brush CurrentColor1
        {
            get
            {
                return this.colors[1];
            }
            set
            {
                this.colors[1] = value;
                OnPropertyChanged("CurrentColor1");
            }
        }

        public Brush CurrentColor2
        {
            get
            {
                return this.colors[2];
            }
            set
            {
                this.colors[2] = value;
                OnPropertyChanged("CurrentColor2");
            }
        }

        public Brush CurrentColor3
        {
            get
            {
                return this.colors[3];
            }
            set
            {
                this.colors[3] = value;
                OnPropertyChanged("CurrentColor2");
            }
        }

        public Brush CurrentColor4
        {
            get
            {
                return this.colors[4];
            }
            set
            {
                this.colors[4] = value;
                OnPropertyChanged("CurrentColor2");
            }
        }

        public Brush CurrentColor5
        {
            get
            {
                return this.colors[5];
            }
            set
            {
                this.colors[5] = value;
                OnPropertyChanged("CurrentColor2");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string parameter = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(parameter));
            }
        }
    }
}
