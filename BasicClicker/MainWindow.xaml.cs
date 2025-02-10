using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);


        private const uint MOUSEEVENT_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        private const int HOTKEY_ID = 9000;
        private const uint MOD_NOREPEAT = 0x4000;
        private const uint VK_F10 = 0x79;


        public MainWindow()
        {
            InitializeComponent();

            lblStatus.Content = "Clicker is stopped.";

            // TODO: make a setting to allow the user to change their keybind.
           

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(30) };
            _timer.Tick += (s, e) => actuallyClick();

            Loaded += (s, e) =>
            {
                IntPtr hWnd = new WindowInteropHelper(this).Handle;
                HwndSource source = HwndSource.FromHwnd(hWnd);
                source.AddHook(WndProc);

                RegisterHotKey(hWnd, HOTKEY_ID, MOD_NOREPEAT, VK_F10);
            };

            Closing += (s, e) =>
            {
                IntPtr hWnd = new WindowInteropHelper(this).Handle;
                UnregisterHotKey(hWnd, HOTKEY_ID);
            };


        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                ToggleClicker();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private void ToggleClicker()
        {
            _isClicking = !_isClicking;

            btnStart.Content = _isClicking ? "Stop (F10)" : "Start (F10)";
            lblStatus.Content = _isClicking ? "Clicker is clicking." : "Clicker is stopped.";
            lblStartStop.Content = _isClicking ? "Press F10 to stop clicker." : "Press F10 to start clicker";

            if (_isClicking)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void btnStart_click(object Sender, RoutedEventArgs e) => ToggleClicker();

        private void actuallyClick()
        {
            mouse_event(MOUSEEVENT_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
        }
    }
}