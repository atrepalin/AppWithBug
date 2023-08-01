using System.Runtime.InteropServices;

namespace WinLib
{
    public partial class WinForm : Form, IContainerForm
    {
        public FormBorderStyle BorderStyle
        {
            get => (FormBorderStyle)FormBorderStyle;
            set => FormBorderStyle = (System.Windows.Forms.FormBorderStyle)value;
        }

        public WinForm(bool darkTheme = false)
        {
            DarkTitleBar.UseImmersiveDarkMode(Handle, darkTheme);
            SizeChanged += WinForm_SizeChanged!;
        }

        private static class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetParent(IntPtr hWnd);

            public const int GWL_STYLE = -16;

            [DllImport("User32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
            public static extern uint GetWindowLongU(IntPtr hwnd, int nIndex);

            [DllImport("User32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
            public static extern uint SetWindowLongU(IntPtr hwnd, int nIndex, uint dwNewLong);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        }

        [Flags]
        public enum WindowStyles : uint
        {
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,
            WS_CHILD = 0x40000000,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZE = 0x20000000,
            WS_MINIMIZEBOX = 0x20000,
            WS_OVERLAPPED = 0x0,

            WS_OVERLAPPEDWINDOW =
                WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUP = 0x80000000u,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_SIZEFRAME = 0x40000,
            WS_SYSMENU = 0x80000,
            WS_TABSTOP = 0x10000,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000
        }

        #region IContainerForm members

        public IntPtr EnclosedHandle { get; private set; }

        public void Enclose(IntPtr hWndToContain)
        {
            if (Handle == IntPtr.Zero)
                throw new InvalidOperationException(
                    "Cannot enclose window because current object's window is not created.");
            var windowStyles = (WindowStyles)NativeMethods.GetWindowLongU(hWndToContain, NativeMethods.GWL_STYLE);
            windowStyles &= ~WindowStyles.WS_OVERLAPPEDWINDOW;
            windowStyles &= ~WindowStyles.WS_POPUP;
            windowStyles &= ~WindowStyles.WS_CAPTION;
            windowStyles |= WindowStyles.WS_CHILD;
            _ = NativeMethods.SetWindowLongU(hWndToContain, NativeMethods.GWL_STYLE, (uint)windowStyles);

            NativeMethods.SetParent(hWndToContain, Handle);
            EnclosedHandle = hWndToContain;
            OnSizeChanged(EventArgs.Empty);
        }

        public string TitleText
        {
            get => Text;
            set => Text = value;
        }

        private event EventHandler? formClosedSimple;

        public event EventHandler FormClosedSimple
        {
            add => formClosedSimple += value;
            remove => formClosedSimple -= value;
        }

        private void FireFormClosedSimple()
        {
            formClosedSimple?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            FireFormClosedSimple();
        }

        #endregion

        public static bool HasParent(IntPtr hWnd)
        {
            return NativeMethods.GetParent(hWnd) != IntPtr.Zero;
        }

        private void WinForm_SizeChanged(object sender, EventArgs e)
        {
            var width = Width;
            var height = Height;
            if (width == 0 || height == 0 || WindowState == FormWindowState.Minimized)
                return;
            if (EnclosedHandle != IntPtr.Zero)
                NativeMethods.MoveWindow(EnclosedHandle, 0, 0, ClientSize.Width, ClientSize.Height, true);
            
        }
    }
}