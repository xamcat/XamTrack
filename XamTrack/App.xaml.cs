using System;
using System.Reflection;
using TinyIoC;
using TinyMvvm;
using TinyMvvm.IoC;
using TinyMvvm.TinyIoC;
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
            var container = TinyIoCContainer.Current;

            container.AutoRegister();
            
            //Register<INavigationHelper>(navigationHelper)

            //container.RegisterType<GeolocationService>().As<IGeolocationService>();
            //container.RegisterType<IoTDeviceClientService>().As<IIoTDeviceClientService>();

          //  TinyIoCContainer.Current.Register<MainPage>();
            TinyIoCContainer.Current.Register<MainViewModel>();
          
            Resolver.SetResolver(new TinyIoCResolver());

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
