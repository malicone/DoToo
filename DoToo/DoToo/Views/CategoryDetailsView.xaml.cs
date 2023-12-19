using Acr.UserDialogs;
using DoToo.ViewModels;
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
    public partial class CategoryDetailsView : ContentPage
    {
        public CategoryDetailsView( CategoryDetailsViewModel viewModel )
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
            viewModel.ConfirmDialog += ConfirmDialogFunc;
        }

        private async Task<bool> ConfirmDialogFunc( string message, string title, string okText, string cancelText )
        {
            return await UserDialogs.Instance.ConfirmAsync( message, title, okText, cancelText );
        }
    }
}