using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace DoToo.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public delegate Task<bool> ConfirmDialogDelegate( string message, string title = null, string okText = null, string cancelText = null );
        public delegate Task<string> ActionSheetDelegate( string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons );

        public ConfirmDialogDelegate ConfirmDialog { get; set; }
        public ActionSheetDelegate ActionSheetDialog { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged( params string[] propertyNames )
        {
            foreach ( var propertyName in propertyNames )
            {
                PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
        public INavigation Navigation { get; set; }
    }
}
