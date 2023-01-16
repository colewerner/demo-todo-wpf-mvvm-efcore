using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Todo.Database.Factory;
using TodoWpf.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TodoWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object. This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            //this.InitializeComponent();

            // Register services
            Ioc.Default.ConfigureServices(new ServiceCollection().AddSingleton<ITodoDbContextFactory, TodoDbContextFactory>() //Services
                                                                 .AddTransient<MainViewModel>()
                                                                 .AddTransient<TodoViewModel>()
                                                                 .BuildServiceProvider());
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.DataContext = Ioc.Default.GetService<MainViewModel>();
            Application.Current.MainWindow.Show();
        }
    }
}
