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
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        public MainWindow()
        {
            InitializeComponent();

            // TODO: make a setting to allow the user to change their keybind.
            var keyBind = new KeyGesture(Key.F6);
            var command = new RoutedCommand();
            command.InputGestures.Add(keyBind);
            CommandBindings.Add(new CommandBinding(command, ToggleClicker));

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += (s, e) => actuallyClick();
        }

        private void ToggleClicker(object sender, ExecutedRoutedEventArgs e)
        {
            _isClicking = !_isClicking;

            btnStart.Content = _isClicking ? "Stop (F6)" : "Start (F6)";

            if (_isClicking)
            {
                _timer.Start();
            } else
            {
                _timer.Stop();
            }
        }

        private void btnStart_click(object Sender, RoutedEventArgs e) => ToggleClicker(null, null);

        private void actuallyClick()
        {
            mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
        }
    }
}