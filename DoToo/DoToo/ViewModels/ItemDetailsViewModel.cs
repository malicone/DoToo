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
    namespace DoToo.ViewModels
    {
        public class ItemDetailsViewModel : BaseViewModel
        {
            public TodoItem Item 
            {
                get { return _item; }
                set
                {
                    _item = value;
                    if ( ( Categories != null ) && ( _item != null ) )
                    {
                        SelectedCategory = Categories.FirstOrDefault( c => c.Id == _item.CategoryId );
                    }
                    RaisePropertyChanged( nameof( Item ) );
                }
            }
            public string Title 
            { 
                get
                {
                    if ( IsNewItem )
                        return "New item";
                    else
                        return "Edit item";                    
                }
            }

            public bool IsNewItem
            {
                get { return Item.Id <= 0; }
            }
            public ObservableCollection<Category> Categories { get; private set; }
            private Category _category;
            public Category SelectedCategory 
            { 
                get { return _category; }
                set
                {
                    _category = value;
                    // we need to search the target category in existing list
                    if ( ( Categories != null ) && ( _category != null ) )
                    {
                        _category = Categories.FirstOrDefault( c => c.Id == _category.Id );
                    }
                    RaisePropertyChanged( nameof( SelectedCategory ) );
                }
            }
            public ItemDetailsViewModel( TodoItemSQLiteRepo todoItemRepo, CategorySQLiteRepo categoryRepo )
            {
                _todoItemRepo = todoItemRepo;
                _categoryRepo = categoryRepo;
                Item = new TodoItem() { Due = DateTime.Now.AddDays( _DEFAULT_DAY_SPAN ) };
                Task.Run( async () => await LoadCategories() );

                _categoryRepo.OnEntityAdded += _categoryRepo_OnEntityAddedAsync;
            }

            public ICommand Save => new Command( async () =>
            {
                if ( SelectedCategory != null )
                {
                    Item.CategoryId = SelectedCategory.Id;
                }
                await _todoItemRepo.AddOrUpdate( Item );
                await Navigation.PopAsync();
            } );

            public ICommand Delete => new Command( async () =>
            {
                if ( IsNewItem == false )
                {
                    var answer = await ConfirmDialog( "Are you sure?", "Delete", "Yes", "No" );
                    if ( answer == true )
                    {
                        await _todoItemRepo.Delete( Item.Id );
                        await Navigation.PopAsync();
                    }
                }
            } );
            public ICommand AddCategory => new Command( async () =>
            {
                var categoryView = Resolver.Resolve<CategoryDetailsView>();
                await Navigation.PushAsync( categoryView );
            } );

            private void _categoryRepo_OnEntityAddedAsync( object sender, Category category )
            {
                Categories.Add( category );
                SelectedCategory = category;
            }

            private async Task LoadCategories()
            {
                var categories = await _categoryRepo.GetAll();
                Categories = new ObservableCollection<Category>( categories.OrderBy( c => c.Name ) );
            }

            private TodoItem _item;
            private ITodoItemRepo _todoItemRepo;
            private CategorySQLiteRepo _categoryRepo;
            private const int _DEFAULT_DAY_SPAN = 1;
        }
    }
}