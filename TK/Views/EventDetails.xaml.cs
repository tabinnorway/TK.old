using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TK.ViewModels;
using TK.ViewModel;
using System.Windows.Navigation;

namespace TK.Views
{
    public partial class EventDetails : PhoneApplicationPage
    {
        public EventDetails() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            MainViewModel _vm = (new ViewModelLocator()).Main;
            var idString = NavigationContext.QueryString["id"];
            long id = -1L;
            if (long.TryParse(idString, out id)) {
                this.DataContext = _vm.EventsById[id];
            }
        }

        private void MembersLB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                var selectedMes = (MemberEventScoreVM)e.AddedItems[0];
                var targetPage = new Uri("/Views/MemberDetails.xaml?id=" + selectedMes.MemberId, UriKind.Relative);

                // reset selection of ListBox
                ((ListBox)sender).SelectedIndex = -1;

                // change page 
                NavigationService.Navigate(targetPage);
            }
        }
    }
}