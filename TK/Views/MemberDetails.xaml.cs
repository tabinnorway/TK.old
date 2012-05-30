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
using System.Windows.Navigation;
using TK.ViewModel;
using TK.ViewModels;

namespace TK.Views
{
    public partial class MemberDetails : PhoneApplicationPage
    {
        public MemberDetails() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            MainViewModel _vm = (new ViewModelLocator()).Main;
            var idString = NavigationContext.QueryString["id"];
            long id = -1L;
            if (long.TryParse(idString, out id)) {
                this.DataContext = _vm.MembersById[id];
            }
        }

        private void EventsLB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                var selectedMes = (MemberEventScoreVM)e.AddedItems[0];
                var targetPage = new Uri("/Views/EventDetails.xaml?id=" + selectedMes.EventId, UriKind.Relative);

                // reset selection of ListBox
                ((ListBox)sender).SelectedIndex = -1;

                // change page 
                NavigationService.Navigate(targetPage);
            }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            PanoramaItem newPage = e.AddedItems[0] as PanoramaItem;
            this.ApplicationBar.IsVisible = newPage.Header != null && newPage.Header.ToString().ToLower().Equals("detaljer");
        }
    }
}