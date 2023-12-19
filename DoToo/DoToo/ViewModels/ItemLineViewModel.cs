using DoToo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DoToo.ViewModels
{
    public class ItemLineViewModel : BaseViewModel
    {
        public ItemLineViewModel( TodoItem item ) => Item = item;
        public event EventHandler ItemStatusChanged;
        public TodoItem Item { get; private set; }        

        public ICommand ToggleCompleted => new Command( ( arg ) =>
        {
            Item.Completed = !Item.Completed;
            ItemStatusChanged?.Invoke( this, new EventArgs() );
        } );
    }
}
