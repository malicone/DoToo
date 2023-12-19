using Acr.UserDialogs;
using DoToo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoToo.Views
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class MainView : ContentPage
    {
        public MainView( MainViewModel viewModel )
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;

            ItemsListView.ItemSelected += ( s, e ) => ItemsListView.SelectedItem = null;
            viewModel.ActionSheetDialog += ActionSheetFunc;
        }

        private async Task<string> ActionSheetFunc( string title, string cancel, string destructive, CancellationToken? cancelToken, string[] buttons )
        {
            return await UserDialogs.Instance.ActionSheetAsync( title, cancel, destructive, cancelToken, buttons );
        }
    }    
}