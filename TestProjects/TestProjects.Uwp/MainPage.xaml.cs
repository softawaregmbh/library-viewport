using softaware.ViewPort.Core;
using softaware.ViewPort.UserInteraction;
using softaware.ViewPort.Uwp.UserInteraction;
using softaware.ViewPort.WinRT;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestProjects.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            IUIThread uiThread = new UwpUIThread();
            IUserInteractionProvider userInteractionProvider = new UwpUserInteractionProvider();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await uiThread.RunAsync(async () =>
                {
                    await userInteractionProvider.GetUserInputAsync("Test", "English button labels", UserInputOption.YesNoCancel, UserInputResult.Cancel);
                    ResourceContext.SetGlobalQualifierValue("Language", "de-AT");
                    await userInteractionProvider.GetUserInputAsync("Test", "German button labels", UserInputOption.YesNoCancel, UserInputResult.Cancel);
                });
            });
        }
    }
}
