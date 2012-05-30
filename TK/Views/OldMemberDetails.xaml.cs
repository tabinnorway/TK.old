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
using TK.ViewModel;
using System.Windows.Navigation;

namespace TK.Views
{
    public partial class OldMemberDetails : PhoneApplicationPage
    {
        public OldMemberDetails() {
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
    }
}