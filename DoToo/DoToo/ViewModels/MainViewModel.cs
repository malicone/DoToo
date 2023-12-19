using DoToo.Models;
using DoToo.Repositories;
using DoToo.ViewModels.DoToo.ViewModels;
using DoToo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoToo.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel( TodoItemSQLiteRepo todoItemRepo, CategorySQLiteRepo categoryRepo )
        {
            _todoItemRepo = todoItemRepo;
            _categoryRepo = categoryRepo;
            ShowAll = false;
            SelectedCategory = DefaultCategory;
            
            Task.Run( async () => await LoadCategories() );

            _todoItemRepo.OnEntityAdded += async ( sender, item ) =>
            {
                Items.Add( CreateItemLineViewModel( item ) );
                await LoadItems( SelectedCategory.Id, !ShowAll );
            };
            _todoItemRepo.OnEntityUpdated += ( sender, item ) => Task.Run( async () => await LoadItems( SelectedCategory.Id, !ShowAll ) );
            _todoItemRepo.OnEntityDeleted += ( sender, item ) => Task.Run( async () => await LoadItems( SelectedCategory.Id, !ShowAll ) );

            _categoryRepo.OnEntityAdded += _categoryRepo_OnEntityAddedAsync;
            //_categoryRepo.OnEntityUpdated += _categoryRepo_OnEntityUpdated;
            //_categoryRepo.OnEntityDeleted += _categoryRepo_OnEntityDeleted;
        }

        public ObservableCollection<Category> Categories { get; private set; }
        public readonly Category DefaultCategory = new Category() { Id = -1, Name = "All Categories" };
        public Category SelectedCategory 
        {
            get { return _selectedCategory; }
            set 
            {
                _selectedCategory = value;                
                Device.BeginInvokeOnMainThread( async () => await LoadItems( _selectedCategory.Id, !ShowAll ) );
                RaisePropertyChanged( nameof( SelectedCategory ) );
            } 
        }
        public ObservableCollection<ItemLineViewModel> Items { get; private set; }
        public bool ShowAll { get; set; }        

        public ICommand AddItem => new Command( async () =>
        {
            var itemView = Resolver.Resolve<ItemDetailsView>();
            var viewModel = itemView.BindingContext as ItemDetailsViewModel;
            if ( SelectedCategory != null )
            {
                viewModel.SelectedCategory = SelectedCategory;
            }
            await Navigation.PushAsync( itemView );
        } );
        public ICommand AddCategory => new Command( async () =>
        {
            var categoryView = Resolver.Resolve<CategoryDetailsView>();
            await Navigation.PushAsync( categoryView );
        } );

        public ICommand ToggleFilter => new Command( async () =>
        {
            ShowAll = !ShowAll;
            await LoadItems( SelectedCategory.Id, !ShowAll );
        } );
        public ICommand ShowSettingsList => new Command( async () =>
        {
            var answer = await ActionSheetDialog( "", "Cancel", "", null, _settingsList );
            switch ( answer )
            {
                case _CATEGORIES:
                    var categoriesView = Resolver.Resolve<CategoriesView>();
                    await Navigation.PushAsync( categoriesView );
                    break;
                case _SETTINGS:
                    
                    break;
                default:
                    break;
            }
        } );

        public ItemLineViewModel SelectedItem
        {
            get { return null; }
            set
            {
                Device.BeginInvokeOnMainThread( async () => await NavigateToItem( value ) );                
                RaisePropertyChanged( nameof( SelectedItem ) );
            }
        }
        private void _categoryRepo_OnEntityAddedAsync( object sender, Category category )
        {
            Categories.Add( category );
            SelectedCategory = category;
        }
        private void _categoryRepo_OnEntityUpdated( object sender, Category category )
        {
            //Console.WriteLine( "_categoryRepo_OnEntityUpdated" );            
            if ( Categories != null )
            {
                var foundCategory = Categories.FirstOrDefault( c => c.Id == category.Id );
                if ( foundCategory != null )
                {
                    //var index = Categories.IndexOf( foundCategory );
                    //Categories[ index ] = category;                    
                    foundCategory.Name = "Changed from event";                    
                }
            }            
        }
        private void _categoryRepo_OnEntityDeleted( object sender, Category category )
        {
            if ( Categories != null )
            {
                var foundCategory = Categories.FirstOrDefault( c => c.Id == category.Id );
                if ( foundCategory != null )
                {
                    Categories.Remove( foundCategory );
                }
            }
        }
        private async Task LoadData()
        {
            await LoadItems( SelectedCategory.Id, !ShowAll );
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _categoryRepo.GetAll();
            var orderedCategories = categories.OrderBy( c => c.Name );
            Categories = new ObservableCollection<Category>();
            Categories.Add( DefaultCategory );// we need to add default category first
            foreach ( var currentCategory in orderedCategories )
            {
                Categories.Add( currentCategory );
            }
/*
#if DEBUG
            Categories.Add( new Category() { Id = -2, Name = "Red" } );
            Categories.Add( new Category() { Id = -3, Name = "Green" } );
#endif
*/
        }

        private async Task LoadItems( int categoryId, bool uncompletedOnly )
        {
            List<TodoItem> items = null;
            if ( categoryId == DefaultCategory.Id )
            {
                items = await _todoItemRepo.GetAll();
            }
            else
            {
                items = await _todoItemRepo.GetByCategory( categoryId );
            }
            if ( uncompletedOnly )
            {
                items = items.Where( x => x.Completed == false ).ToList();
            }
            var orderedItems = items.OrderByDescending( i => i.Due );
            var itemViewModels = orderedItems.Select( i => CreateItemLineViewModel( i ) );
            Items = new ObservableCollection<ItemLineViewModel>( itemViewModels );
        }

        private ItemLineViewModel CreateItemLineViewModel( TodoItem item )
        {
            var itemViewModel = new ItemLineViewModel( item );
            itemViewModel.ItemStatusChanged += ItemStatusChanged;
            return itemViewModel;
        }
        private void ItemStatusChanged( object sender, EventArgs e )
        {
            if ( sender is ItemLineViewModel item )
            {
                if ( !ShowAll && item.Item.Completed )
                {
                    Items.Remove( item );
                }
                Task.Run( async () => await _todoItemRepo.Update( item.Item ) );
            }
        }        

        private async Task NavigateToItem( ItemLineViewModel item )
        {
            if ( item == null )
            {
                return;
            }
            var itemView = Resolver.Resolve<ItemDetailsView>();
            var viewModel = itemView.BindingContext as ItemDetailsViewModel;
            viewModel.Item = item.Item;
            await Navigation.PushAsync( itemView );
        }

        private readonly ITodoItemRepo _todoItemRepo;
        private readonly ICategoryRepo _categoryRepo;
        private Category _selectedCategory;

        private const string _CATEGORIES = "Categories";
        private const string _SETTINGS = "Settings";
        private readonly string[] _settingsList = new[] { _CATEGORIES, _SETTINGS };
    }
}
