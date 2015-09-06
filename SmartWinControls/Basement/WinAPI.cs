using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SmartWinControls.Basement
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-17
    /// @Desc：Windows API 函数
    /// </summary>
    public static class WinAPI
    {
        #region const
        public static readonly IntPtr TRUE = new IntPtr(1);
        public static readonly IntPtr FALSE = IntPtr.Zero;
        #endregion

        #region Windows Messages(系统消息)

        public enum WindowsMessages
        {
            WM_MOVE = 0x0003,//移动窗口
            WM_SIZE = 0x0005,//改变窗口大小
            WM_ACTIVATE = 0x0006,//一个窗体被激活或者失去激活状态
            WM_ACTIVATEAPP = 0x001C,//当某个窗口将被激活时，将被激活窗口和当前活动（即将失去激活）窗口会收到此消息，发此消息给应用程序哪个窗口是激活的，哪个是非激活的
            WM_SETCURSOR = 0x0020,//如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，发消息给该窗口
            WM_MOUSEACTIVATE = 0x0021,//当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给当前窗口
            WM_GETMINMAXINFO = 0x24,//当窗口要将要改变大小或位置时，发送此消息给该窗口
            WM_WINDOWPOSCHANGING = 0x0046,//当调用SetWindowPos()函数改变窗口的大小和位置后，发送此消息给该窗口
            WM_WINDOWPOSCHANGED = 0x0047,//发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数

            // non client area
            WM_NCCREATE = 0x0081,//当某个窗口第一次被创建时，此消息在WM_CREATE消息被发送前发送
            WM_NCDESTROY = 0x0082,//此消息通知某个窗口，正在销毁非客户区
            WM_NCCALCSIZE = 0x0083,//当计算某个窗口的客户区大小和位置时发送此消息
            WM_NCHITTEST = 0x84,//移动鼠标，按住或释放鼠标时产生此消息
            WM_NCPAINT = 0x0085,//当某个窗口的框架必须被绘制时，应用程序发送此消息给该窗口
            WM_NCACTIVATE = 0x0086,//通过改变某个窗口的非客户区来表示窗口是处于激活还是非激活状态时，此消息被发送给该窗口

            // non client mouse
            WM_NCMOUSEMOVE = 0x00A0,//当光标在窗口的非客户区（窗口标题栏及边框）内移动时发送此消息给该窗口
            WM_NCLBUTTONDOWN = 0x00A1,//当光标在窗口的非客户区并按下鼠标左键时发送此消息
            WM_NCLBUTTONUP = 0x00A2,//当光标在窗口的非客户区并释放鼠标左键时发送此消息
            WM_NCLBUTTONDBLCLK = 0x00A3,//当光标在窗口的非客户区并双击鼠标左键时发送此消息
            WM_NCRBUTTONDOWN = 0x00A4,//当光标在窗口的非客户区并按下鼠标右键时发送此消息
            WM_NCRBUTTONUP = 0x00A5,//当光标在窗口的非客户区并释放鼠标右键时发送此消息
            WM_NCRBUTTONDBLCLK = 0x00A6,//当光标在窗口的非客户区并双击鼠标右键时发送此消息
            WM_NCMBUTTONDOWN = 0x00A7,//当光标在窗口的非客户区并按下鼠标中键时发送此消息
            WM_NCMBUTTONUP = 0x00A8,//当光标在窗口的非客户区并释放鼠标中键时发送此消息
            WM_NCMBUTTONDBLCLK = 0x00A9,//当光标在窗口的非客户区并双击鼠标中键时发送此消息

            WM_SYSCOMMAND = 0x0112,//选择窗口菜单项或选择最大化或最小化时，发送此消息给该窗口
            WM_PARENTNOTIFY = 0x0210,//当MDI子窗口被创建或被销毁，或当光标位于子窗口上且用户按了一下鼠标键时，发送此消息给它的父窗口

            WM_MDINEXT = 0x224,//应用程序发送此消息给MDI客户窗口激活下一个或前一个窗口
        }
        #endregion

        #region WindowStyle
        [Flags]
        public enum WindowStyle : uint
        {
            WS_OVERLAPPED = 0x00000000,//重叠的窗口
            WS_POPUP = 0x80000000,//弹出窗口
            WS_CHILD = 0x40000000,//子窗口  必须有一个父窗口
            WS_MINIMIZE = 0x20000000,//窗口最小化
            WS_VISIBLE = 0x10000000,//窗口可见
            WS_DISABLED = 0x08000000,//窗口禁用
            WS_CLIPSIBLINGS = 0x04000000,//
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,//窗口最大化
            WS_CAPTION = 0x00C00000,//窗口标题
            WS_BORDER = 0x00800000,//窗口边框
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,//纵向滚轮
            WS_HSCROLL = 0x00100000,//横向滚轮
            WS_SYSMENU = 0x00080000,//系统菜单
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                                    WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),
            WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),
            WS_CHILDWINDOW = (WS_CHILD)
        }
        #endregion

        #region WindowStyleEx
        [Flags]
        public enum WindowStyleEx
        {
            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,
            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,
            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,
            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,
            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
            WS_EX_LAYERED = 0x00080000,
            WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
            WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000,
        }
        #endregion

        #region Scrollbar
        public enum ScrollBar
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3,
        }
        #endregion

        #region NCHITTEST
        /// <summary>
        /// Location of cursor hot spot returnet in WM_NCHITTEST.
        /// </summary>
        public enum NCHITTEST
        {
            /// <summary>
            /// On the screen background or on a dividing line between windows 
            /// (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).
            /// </summary>
            HTERROR = (-2),
            /// <summary>
            /// In a window currently covered by another window in the same thread 
            /// (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).
            /// </summary>
            HTTRANSPARENT = (-1),
            /// <summary>
            /// On the screen background or on a dividing line between windows.
            /// </summary>
            HTNOWHERE = 0,
            /// <summary>In a client area.</summary>
            HTCLIENT = 1,
            /// <summary>In a title bar.</summary>
            HTCAPTION = 2,
            /// <summary>In a window menu or in a Close button in a child window.</summary>
            HTSYSMENU = 3,
            /// <summary>In a size box (same as HTSIZE).</summary>
            HTGROWBOX = 4,
            /// <summary>In a menu.</summary>
            HTMENU = 5,
            /// <summary>In a horizontal scroll bar.</summary>
            HTHSCROLL = 6,
            /// <summary>In the vertical scroll bar.</summary>
            HTVSCROLL = 7,
            /// <summary>In a Minimize button.</summary>
            HTMINBUTTON = 8,
            /// <summary>In a Maximize button.</summary>
            HTMAXBUTTON = 9,
            /// <summary>In the left border of a resizable window 
            /// (the user can click the mouse to resize the window horizontally).</summary>
            HTLEFT = 10,
            /// <summary>
            /// In the right border of a resizable window 
            /// (the user can click the mouse to resize the window horizontally).
            /// </summary>
            HTRIGHT = 11,
            /// <summary>In the upper-horizontal border of a window.</summary>
            HTTOP = 12,
            /// <summary>In the upper-left corner of a window border.</summary>
            HTTOPLEFT = 13,
            /// <summary>In the upper-right corner of a window border.</summary>
            HTTOPRIGHT = 14,
            /// <summary>	In the lower-horizontal border of a resizable window 
            /// (the user can click the mouse to resize the window vertically).</summary>
            HTBOTTOM = 15,
            /// <summary>In the lower-left corner of a border of a resizable window 
            /// (the user can click the mouse to resize the window diagonally).</summary>
            HTBOTTOMLEFT = 16,
            /// <summary>	In the lower-right corner of a border of a resizable window 
            /// (the user can click the mouse to resize the window diagonally).</summary>
            HTBOTTOMRIGHT = 17,
            /// <summary>In the border of a window that does not have a sizing border.</summary>
            HTBORDER = 18,

            HTOBJECT = 19,
            /// <summary>In a Close button.</summary>
            HTCLOSE = 20,
            /// <summary>In a Help button.</summary>
            HTHELP = 21,
        }

        #endregion

        #region struct

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }

            public override string ToString()
            {
                return "{ Left:" + this.Left + ", Top:" + this.Top
                    + ", Width:" + (this.Right - this.Left).ToString()
                    + ", Height:" + (this.Bottom - this.Top).ToString() + "}";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hWndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NCCALCSIZE_PARAMS
        {
            /// <summary>
            /// Contains the new coordinates of a window that has been moved or resized, that is, it is the proposed new window coordinates.
            /// </summary>
            public RECT rectNewForm;
            /// <summary>
            /// Contains the coordinates of the window before it was moved or resized.
            /// </summary>
            public RECT rectOldForm;
            /// <summary>
            /// Contains the coordinates of the window's client area before the window was moved or resized.
            /// </summary>
            public RECT rectOldClient;
            /// <summary>
            /// Pointer to a WINDOWPOS structure that contains the size and position values specified in the operation that moved or resized the window.
            /// </summary>
            public WINDOWPOS lpPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public Int32 cx;
            public Int32 cy;

            public SIZE(Int32 x, Int32 y)
            {
                cx = x;
                cy = y;
            }

            public SIZE(System.Drawing.Size size)
            {
                cx = size.Width;
                cy = size.Height;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }

        public enum BlendOp : byte
        {
            AC_SRC_OVER = 0x00,
            AC_SRC_ALPHA = 0x01,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;

            public POINT(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT reserved;
            public SIZE maxSize;
            public POINT maxPosition;
            public SIZE minTrackSize;
            public SIZE maxTrackSize;
        }

        #endregion

        #region enum

        // update-layered-window
        public enum ULWPara
        {
            ULW_COLORKEY = 0x00000001,
            ULW_ALPHA = 0x00000002,
            ULW_OPAQUE = 0x00000004,
            ULW_EX_NORESIZE = 0x00000008,
        }

        // get-wondow-long
        public enum GWLPara
        {
            GWL_WNDPROC = -4,
            GWL_HINSTANCE = -6,
            GWL_HWNDPARENT = -8,
            GWL_STYLE = -16,
            GWL_EXSTYLE = -20,
            GWL_USERDATA = -21,
            GWL_ID = -12,
        }

        // set-window-position
        public enum SWPPara : uint
        {
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
        }

        #endregion

        #region non-dll method

        public static int LOWORD(int value)
        {
            return value & 0xFFFF;
        }

        public static int HIWORD(int value)
        {
            return value >> 16;
        }

        #endregion

        #region dll-import method

        [DllImport("user32.dll")]
        public static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(IntPtr hWnd, int Index);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(IntPtr hWnd, int Index, int Value);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd
            , IntPtr hdcDst
            , ref POINT pptDst
            , ref SIZE psize
            , IntPtr hdcSrc
            , ref POINT pptSrc
            , uint crKey
            , [In] ref BLENDFUNCTION pblend
            , uint dwFlags
            );

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg,
                                    IntPtr wParam, IntPtr lParam);

        #endregion

    }
}
