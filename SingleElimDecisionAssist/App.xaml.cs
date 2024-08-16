using Microsoft.Extensions.DependencyInjection;
using SingleElimDecisionAssist.Interfaces;
using SingleElimDecisionAssist.Models;
using SingleElimDecisionAssist.ViewModels;
using System;
using System.Windows;

namespace SingleElimDecisionAssist
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<MainWindow>().Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IElimination<string>, SingleElim<string>>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
        }
    }
}
