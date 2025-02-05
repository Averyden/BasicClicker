using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BasicClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private bool _isClicking = false;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        private const uint MOUSEEVENT_LEFTDOWN = 0x02;
        private const uint MOUSEEVENT_RIGHTDOWN = 0x04;

        public MainWindow()
        {
            InitializeComponent();

            // TODO: make a setting to allow the user to change their keybind.
            var keyBind = new KeyGesture(Key.F6);
            var command = new RoutedCommand();
            command.InputGestures.Add(keyBind);
            CommandBindings.Add(new CommandBinding(command, ToggleClicker));

        }

        private void ToggleClicker(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}