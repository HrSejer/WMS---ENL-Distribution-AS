using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS___ENL_Distribution_AS.Core;

namespace WMS___ENL_Distribution_AS.Viewmodel
{
    class MenuViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }

        public RelayCommand LagerViewCommand { get; set; }
        public RelayCommand OrdrerViewCommand { get; set; }
        public RelayCommand MedarbejderViewCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }

        public LagerViewModel LagerVm { get; set; }
        public OrdrerViewModel OrdrerVm { get; set; }
        public MedarbejderViewModel MedarbejderVm { get; set; }


        private object _currentView;

        public object CurrentView 
        {
            get { return _currentView; }
            set
            { 
                _currentView = value;
                OnPropertyChanged();
            }

        }


        public MenuViewModel()
        {
            HomeVm = new HomeViewModel();
            LagerVm = new LagerViewModel();
            OrdrerVm = new OrdrerViewModel();
            MedarbejderVm= new MedarbejderViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = HomeVm;     
            });

            LagerViewCommand = new RelayCommand(o =>
            {
                CurrentView = LagerVm;
            });

            OrdrerViewCommand = new RelayCommand(o =>
            {
                CurrentView = OrdrerVm;
            });

            MedarbejderViewCommand = new RelayCommand(o =>
            {
                CurrentView = MedarbejderVm;
            });
        }
    }
}
