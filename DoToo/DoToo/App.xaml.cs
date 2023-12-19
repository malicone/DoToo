using DoToo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoToo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new NavigationPage( Resolver.Resolve<MainView>() );
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
