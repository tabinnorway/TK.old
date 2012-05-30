using Microsoft.Phone.Controls;
using System.Windows.Controls;
using System;
using TK.ViewModels;
using System.Windows;
using TK.Views;

namespace TK
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage() {
            InitializeComponent();
        }

        private MemberVM selectedMember = null;
        private void MembersLB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                selectedMember = (MemberVM)e.AddedItems[0];
                var targetPage = new Uri("/Views/OldMemberDetails.xaml", UriKind.Relative);

                // reset selection of ListBox
                ((ListBox)sender).SelectedIndex = -1;

                // change page 
                NavigationService.Navigate(targetPage);
                FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
                root.DataContext = selectedMember;
            }
        }

        private void EventsLB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                var selectedEvent = (EventVM)e.AddedItems[0];
                var targetPage = new Uri("/Views/EventDetails.xaml?id=" + selectedEvent.Id, UriKind.Relative);

                // reset selection of ListBox
                ((ListBox)sender).SelectedIndex = -1;

                // change page 
                NavigationService.Navigate(targetPage);
            }
        }
    }
}
