using Autofac;
using DoToo.Repositories;
using DoToo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace DoToo
{
    public abstract class Bootstrapper
    {
        protected ContainerBuilder ContainerBuilder
        {
            get; private set;
        }
        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }
        protected virtual void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            ContainerBuilder = new ContainerBuilder();
            var typesToRegister = currentAssembly.DefinedTypes.Where( 
                e => e.IsSubclassOf( typeof( Page ) ) || e.IsSubclassOf( typeof( BaseViewModel ) ) );
            foreach ( var currentType in typesToRegister )
            {
                ContainerBuilder.RegisterType( currentType.AsType() );
            }
            ContainerBuilder.RegisterType<TodoItemSQLiteRepo>().SingleInstance();
            ContainerBuilder.RegisterType<CategorySQLiteRepo>().SingleInstance();
        }
        private void FinishInitialization()
        {
            var container = ContainerBuilder.Build();
            Resolver.Initialize( container );
        }
    }
}
