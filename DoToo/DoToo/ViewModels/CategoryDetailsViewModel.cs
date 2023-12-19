using Acr.UserDialogs;
using DoToo.Models;
using DoToo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoToo.ViewModels
{
    public class CategoryDetailsViewModel : BaseViewModel
    {
        public Category CurrentCategory { get; set; }
        public string Title
        {
            get
            {
                if ( IsNewCategory )
                    return "New category";
                else
                    return "Edit category";
            }
        }

        public bool IsNewCategory
        {
            get { return CurrentCategory.Id <= 0; }
        }
        public CategoryDetailsViewModel( CategorySQLiteRepo categoryRepo )
        {           
            _categoryRepo = categoryRepo;
            CurrentCategory = new Category();
        }

        public ICommand Save => new Command( async () =>
        {
            await _categoryRepo.AddOrUpdate( CurrentCategory );
            await Navigation.PopAsync();
        } );

        public ICommand Delete => new Command( async () =>
        {
            if ( IsNewCategory == false )
            {
                var answer = await ConfirmDialog( "Are you sure?", "Delete", "Yes", "No" );
                if ( answer == true )
                {
                    await _categoryRepo.Delete( CurrentCategory.Id );
                    await Navigation.PopAsync();
                }
            }
        } );
        
        private CategorySQLiteRepo _categoryRepo;
    }
}
