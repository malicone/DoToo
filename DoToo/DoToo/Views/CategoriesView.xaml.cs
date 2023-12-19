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
    public partial class CategoriesView : ContentPage
    {
        public CategoriesView( CategoriesViewModel viewModel )
        {
            InitializeComponent();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;

            CategoriesListView.ItemSelected += ( s, e ) => CategoriesListView.SelectedItem = null;
        }
    }
}