using DoToo.Models;
using DoToo.Repositories;
using DoToo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoToo.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; private set; }

        public CategoriesViewModel( CategorySQLiteRepo categoryRepo )
        {
            _categoryRepo = categoryRepo;
            _categoryRepo.OnEntityAdded += _categoryRepo_OnEntityAdded;
            _categoryRepo.OnEntityUpdated += _categoryRepo_OnEntityUpdated;
            _categoryRepo.OnEntityDeleted += _categoryRepo_OnEntityDeleted;

            Task.Run( async () => await LoadCategories() );
        }

        public Category SelectedCategory
        {
            get { return null; }
            set
            {
                Device.BeginInvokeOnMainThread( async () => await NavigateToCategory( value ) );
                RaisePropertyChanged( nameof( SelectedCategory ) );
            }
        }

        private async Task NavigateToCategory( Category category )
        {
            if ( category == null )
            {
                return;
            }
            var categoryView = Resolver.Resolve<CategoryDetailsView>();
            var viewModel = categoryView.BindingContext as CategoryDetailsViewModel;
            viewModel.CurrentCategory = category;
            await Navigation.PushAsync( categoryView );
        }

        public ICommand Add => new Command( async () =>
        {
            var categoryView = Resolver.Resolve<CategoryDetailsView>();            
            await Navigation.PushAsync( categoryView );
        } );

        private void _categoryRepo_OnEntityAdded( object sender, Category category )
        {
            Categories.Add( category );            
        }
        private void _categoryRepo_OnEntityUpdated( object sender, Category category )
        {
            var foundCategory = Categories.FirstOrDefault( c => c.Id == category.Id );
            if ( foundCategory != null )
            {
                var index = Categories.IndexOf( foundCategory );
                Categories[ index ] = category;
            }
        }
        private void _categoryRepo_OnEntityDeleted( object sender, Category category )
        {
            var foundCategory = Categories.FirstOrDefault( c => c.Id == category.Id );
            if ( foundCategory != null )
            {
                Categories.Remove( foundCategory );
            }
        }

        private async Task LoadCategories()
        {
            var categories = await _categoryRepo.GetAll();
            var orderedCategories = categories.OrderBy( c => c.Name );
            Categories = new ObservableCollection<Category>( orderedCategories );
        }

        private readonly ICategoryRepo _categoryRepo;
    }
}
