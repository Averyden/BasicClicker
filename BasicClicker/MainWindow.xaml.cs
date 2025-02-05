using System.Runtime.InteropServices;
using System.Windows;
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

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}