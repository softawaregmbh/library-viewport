using softaware.ViewPort.Core;
using softaware.ViewPort.UserInteraction;
using softaware.ViewPort.Wpf;
using softaware.ViewPort.Wpf.UserInteraction;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;

namespace TestProjects.Wpf.Full
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IUIThread uiThread = new WpfUIThread();
            IUserInteractionProvider userInteractionProvider = new WpfUserInteractionProvider();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await uiThread.RunAsync(async () =>
                {
                    CultureInfo.CurrentUICulture = new CultureInfo("en-US");
                    await userInteractionProvider.GetFolderAsync();

                    CultureInfo.CurrentUICulture = new CultureInfo("de-AT");
                    await userInteractionProvider.GetFolderAsync();
                });
            });
        }
    }
}
