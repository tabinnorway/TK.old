using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace TK.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel() {
            _movies = new List<MovieVM> {
                new MovieVM { Title = "Crossing Jordan", SubTitle = "Going across a river", Points = 5, },
                new MovieVM { Title = "Moving heaven and earth", SubTitle = "About a moving company", Points = 5, },
            };
            if (IsInDesignMode) {
                // Code runs in Blend --> create design time data.
            }
            else {
                // Code runs "for real"
            }
        }

        /// <summary>
        /// Gets the ApplicationTitle property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string ApplicationTitlePropertyName = "ApplicationTitle";
        private string _applicationTitle = "torsdagskino";
        public string ApplicationTitle {
            get { return _applicationTitle; }
            set {
                if (_applicationTitle == value) {
                    return;
                }
                _applicationTitle = value;
                RaisePropertyChanged(ApplicationTitlePropertyName);
            }
        }

        /// <summary>
        /// Gets the FirstPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string FirstPageNamePropertyName = "FirstPageName";
        private string _firstPageName = "oversikt";
        public string FirstPageName {
            get { return _firstPageName; }
            set {
                if (_firstPageName == value) {
                    return;
                }
                _firstPageName = value;
                RaisePropertyChanged(FirstPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the SoFarThisYearPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string SoFarThisYearPageNamePropertyName = "SoFarThisYearPageName";
        private string _soFarThisYearPageName = "hittil i år";
        public string SoFarThisYearPageName {
            get { return _soFarThisYearPageName; }
            set {
                if (_soFarThisYearPageName == value) {
                    return;
                }
                _soFarThisYearPageName = value;
                RaisePropertyChanged(SoFarThisYearPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the TotalsPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string TotalsPageNamePropertyName = "TotalsPageName";
        private string _totalsPageName = "totalt";
        public string TotalsPageName {
            get { return _totalsPageName; }
            set {
                if (_totalsPageName == value) {
                    return;
                }
                _totalsPageName = value;
                RaisePropertyChanged(TotalsPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the MembersPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string MembersPageNamePropertyName = "MembersPageName";
        private string _membersPageName = "medlemmer";
        public string MembersPageName {
            get { return _membersPageName; }
            set {
                if (_membersPageName == value) {
                    return;
                }
                _membersPageName = value;
                RaisePropertyChanged(MembersPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the SettingsPageName property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string SettingsPageNamePropertyName = "SettingsPageName";
        private string _settingsPageName = "innstillinger";
        public string SettingsPageName {
            get { return _settingsPageName; }
            set {
                if (_settingsPageName == value) {
                    return;
                }
                _settingsPageName = value;
                RaisePropertyChanged(SettingsPageNamePropertyName);
            }
        }

        /// <summary>
        /// Gets the Welcome property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string WelcomePropertyName = "Welcome";
        private string _welcome = "Welcome to MVVM Light";
        public string Welcome {
            get { return _welcome; }
            set {
                if (_welcome == value) {
                    return;
                }
                _welcome = value;
                RaisePropertyChanged(WelcomePropertyName);
            }
        }

        /// <summary>
        /// Gets the Movies property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string MoviesPropertyName = "Movies";
        private List<MovieVM> _movies = null;
        public List<MovieVM> Movies {
            get { return _movies; }
            set {
                if (_movies == value) {
                    return;
                }
                _movies = value;
                RaisePropertyChanged(MoviesPropertyName);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}