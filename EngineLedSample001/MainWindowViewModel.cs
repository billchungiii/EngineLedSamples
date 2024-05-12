using EngineLedSample001.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EngineLedSample001
{
    public class MainWindowViewModel : NotifyPropertyBase
    {
        private const string Cylinder = "汽缸";
        public MainWindowViewModel()
        {
            Cylinders = new ObservableCollection<Cylinder>
            {
                new Cylinder { CylinderNumber = 1, IsOn = false },
                new Cylinder { CylinderNumber = 2, IsOn = false },
                new Cylinder { CylinderNumber = 3, IsOn = false },
                new Cylinder { CylinderNumber = 4, IsOn = false },
                new Cylinder { CylinderNumber = 5, IsOn = false },
                new Cylinder { CylinderNumber = 6, IsOn = false },
                new Cylinder { CylinderNumber = 7, IsOn = false },
                new Cylinder { CylinderNumber = 8, IsOn = false },
                new Cylinder { CylinderNumber = 9, IsOn = false }
            };
            Modes = new ObservableCollection<string>();
            Steps = new ObservableCollection<string>();
        }

        private int _modesCount;
        public int ModesCount
        {
            get { return _modesCount; }
            set
            {
                if (SetProperty(ref _modesCount, value))
                {
                    Modes.Clear();
                    for (int i = 0; i < ModesCount; i++)
                    {
                        Modes.Add($"Mode {i + 1}");
                    }
                }
            }
        }

        private ObservableCollection<Cylinder> _cylinders;
        public ObservableCollection<Cylinder> Cylinders
        {
            get { return _cylinders; }
            set { SetProperty(ref _cylinders, value); }
        }

        private ObservableCollection<string> _modes;
        public ObservableCollection<string> Modes
        {
            get { return _modes; }
            set { SetProperty(ref _modes, value); }
        }

        private string _seletedMode;
        public string SelectedMode
        {
            get { return _seletedMode; }
            set
            {
                if (SetProperty(ref _seletedMode, value))
                {
                    Steps.Clear();
                    ResetCylinders();
                }
            }
        }

        private ObservableCollection<string> _steps;
        public ObservableCollection<string> Steps
        {
            get { return _steps; }
            set { SetProperty(ref _steps, value); }
        }

        private ICommand _ledClickCommand;
        public ICommand LedClickCommand
        {
            get
            {
                if (_ledClickCommand == null)
                {
                    _ledClickCommand = new RelayCommand((x) =>
                    {
                        if (x is ToggleButton button)
                        {
                            if (button.IsChecked.GetValueOrDefault())
                            {
                                Steps.Add($"{Cylinder}{button.Content}");
                            }
                            else
                            {
                                Steps.Remove($"{Cylinder}{button.Content}");
                            }
                        }

                    });
                }
                return _ledClickCommand;
            }
        }

        private ICommand _startCommand;
        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(async (x) =>
                    {
                        if (Steps.Count == 0) { return; }
                        if (SelectedMode == null) { return; }
                        ResetCylinders();
                        foreach (var step in Steps)
                        {
                            var cylinder = Cylinders.FirstOrDefault(c => $"{Cylinder}{c.CylinderNumber}" == step);
                            if (cylinder != null)
                            {
                                cylinder.IsOn = null;  
                                await Task.Delay(500);
                            }
                        }
                        ResetCylinders();
                    });
                }
                return _startCommand;
            }
        }

        private void ResetCylinders()
        {
            foreach (var cylinder in Cylinders)
            {
                cylinder.IsOn = false;
            }
        }
    }

    public class Cylinder : NotifyPropertyBase
    {
        private int _cylinderNumber;
        public int CylinderNumber
        {
            get { return _cylinderNumber; }
            set { SetProperty(ref _cylinderNumber, value); }
        }

        private bool? _isOn;
        public bool? IsOn
        {
            get { return _isOn; }
            set { SetProperty(ref _isOn, value); }
        }
    }
}
