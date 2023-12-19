using Acr.UserDialogs;
using DoToo.ViewModels.DoToo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoToo.Views
{
    [XamlCompilation( XamlCompilationOptions.Compile )]
    public partial class ItemDetailsView : ContentPage
    {
        public ItemDetailsView( ItemDetailsViewModel viewModel )
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
            viewModel.ConfirmDialog += ConfirmDialogFunc;
        }

        private async Task<bool> ConfirmDialogFunc( string message, string title = null, string okText = null, string cancelText = null )
        {
            return await UserDialogs.Instance.ConfirmAsync( message, title, okText, cancelText );
        }
    }
}