using Autofac;
using System;
using System.Reflection;
using TinyMvvm;
using TinyMvvm.Autofac;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyNavigationHelper.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamTrack.Core.Services;
using XamTrack.Core.ViewModels;
using XamTrack.Views;

namespace XamTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var currentAssembly = Assembly.GetExecutingAssembly();

            var navigationHelper = new FormsNavigationHelper();

            navigationHelper.RegisterViewsInAssembly(currentAssembly);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<GeolocationService>().As<IGeolocationService>();
                                   
            containerBuilder.RegisterType<MainPage>();
            containerBuilder.RegisterType<MainViewModel>();

            var container = containerBuilder.Build();

            Resolver.SetResolver(new AutofacResolver(container));

            TinyMvvm.Forms.TinyMvvm.Initialize();

            MainPage = new MainPage();
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
