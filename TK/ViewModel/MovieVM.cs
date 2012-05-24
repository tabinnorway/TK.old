using GalaSoft.MvvmLight;

namespace TK.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
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
    public class MovieVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MovieVM class.
        /// </summary>
        public MovieVM() {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}
        }

        /// <summary>
        /// Gets the Title property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string TitlePropertyName = "Title";
        private string _title = null;
        public string Title {
            get { return _title; }
            set {
                if (_title == value) {
                    return;
                }
                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// Gets the SubTitle property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string SubTitlePropertyName = "SubTitle";
        private string _subTitle = null;
        public string SubTitle {
            get { return _subTitle; }
            set {
                if (_subTitle == value) {
                    return;
                }
                _subTitle = value;
                RaisePropertyChanged(SubTitlePropertyName);
            }
        }

        /// <summary>
        /// Gets the Points property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public const string PointsPropertyName = "Points";
        private int _points = 0;
        public int Points {
            get { return _points; }
            set {
                if (_points == value) {
                    return;
                }
                _points = value;
                RaisePropertyChanged(PointsPropertyName);
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}