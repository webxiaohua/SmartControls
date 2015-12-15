using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using SmartWinControls.Common;
using SmartWinControls.Painter;
using SmartWinControls.Basement;
using System.Runtime.InteropServices;

namespace SmartWinControls.SmartControls.ComboBox
{
    [ToolboxItem(false)]
    public class SmartToolStripDropDown : ToolStripDropDown
    {
        #region constructors

        public SmartToolStripDropDown()
        {
            ResizeRedraw = true;
            DoubleBuffered = true;
            base.MinimumSize = new Size(18, 18);
        }

        #endregion

        #region inner property

        private Rectangle ResizeRect
        {
            get
            {
                if (Resizable)
                {
                    Size size = ResizeGridSize;
                    switch (WhereIsResizeGrid)
                    {
                        case ResizeGridLocation.TopLeft:
                            return new Rectangle(new Point(0 + 1, 0 + 1), size);                            
                        case ResizeGridLocation.TopRight:
                            return new Rectangle(
                                new Point(base.Width - size.Width - 1, 0 + 1), size);
                        case ResizeGridLocation.BottomLeft:
                            return new Rectangle(
                                new Point(0 + 1, base.Height - size.Height - 1), size);
                        case ResizeGridLocation.BottomRight:
                            return new Rectangle(
                                base.Width - size.Width - 1, base.Height - size.Height - 1, 
                                size.Width, size.Height);
                    }                    
                }
                return Rectangle.Empty;
            }
        }

        #endregion

        #region public properties

        private bool _resizable = true;
        private Color _borderColor = Color.DarkGray;
        private Color _resizeGridColor = Color.FromArgb(125, 125, 125);
        private Size _resizeGridSize = new Size(16, 16);
        private ResizeGridLocation _gridLocatioon = ResizeGridLocation.BottomRight;

        /// <summary>
        /// 获取或设置一个值，该值指示ToolStripDropDown是否可以通过鼠标拖动来改变自身大小
        /// </summary>
        public bool Resizable
        {
            get
            {
                return _resizable;
            }
            set
            {
                if (_resizable != value)
                {
                    _resizable = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置DropDown的边框颜色
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置Resize标记的颜色
        /// </summary>
        public Color ResizeGridColor
        {
            get
            {
                return _resizeGridColor;
            }
            set
            {
                if (_resizeGridColor != value)
                {
                    _resizeGridColor = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示鼠标感应的Resizable区域的大小
        /// </summary>
        public Size ResizeGridSize
        {
            get
            {
                return _resizeGridSize;
            }
            set
            {
                if (_resizeGridSize != value)
                {
                    _resizeGridSize = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示 Resize 区域位于哪个角上
        /// </summary>
        public ResizeGridLocation WhereIsResizeGrid
        {
            get
            {
                return _gridLocatioon;
            }
            set
            {
                if (_gridLocatioon != value)
                {
                    _gridLocatioon = value;
                }
            }
        }

        #endregion

        #region override somthing

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);            
            BasicBlockPainter.RenderBorder(
                e.Graphics,
                ClientRectangle,
                BorderColor,
                ButtonBorderType.Rectangle);
            if (Resizable)
            {
                BasicBlockPainter.RenderResizeGrid(
                    e.Graphics, ResizeRect, ResizeGridColor, WhereIsResizeGrid);
            }
        }

        #endregion

        #region message wndproc override

        private bool WmNcHitTest(ref Message m)
        {
            int para = m.LParam.ToInt32();
            int x0 = WinAPI.LOWORD(para);
            int y0 = WinAPI.HIWORD(para);
            Point p = base.PointToClient(new Point(x0, y0));

            if (ResizeRect.Contains(p))
            {
                switch (WhereIsResizeGrid)
                {
                    case ResizeGridLocation.TopLeft:
                        m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTTOPLEFT);
                        break;
                    case ResizeGridLocation.TopRight:
                        m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTTOPRIGHT);
                        break;
                    case ResizeGridLocation.BottomLeft:
                        m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTBOTTOMLEFT);
                        break;
                    case ResizeGridLocation.BottomRight:
                        m.Result = new IntPtr((int)WinAPI.NCHITTEST.HTBOTTOMRIGHT);
                        break;
                }
                return true;
            }
            return false;
        }

        private bool WmGetMinMaxInfo(ref Message m)
        {
            WinAPI.MINMAXINFO minmaxInfo = (WinAPI.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(WinAPI.MINMAXINFO));
            if (!MaximumSize.IsEmpty)
            {
                minmaxInfo.maxTrackSize = new WinAPI.SIZE(MaximumSize);
            }
            if (!MinimumSize.IsEmpty)
            {
                minmaxInfo.minTrackSize = new WinAPI.SIZE(MinimumSize);
            }
            Marshal.StructureToPtr(minmaxInfo, m.LParam, false);
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (Resizable)
            {
                if (m.Msg == (int)WinAPI.WindowsMessages.WM_NCHITTEST)
                {
                    if (WmNcHitTest(ref m))
                        return;
                }
                if (m.Msg == (int)WinAPI.WindowsMessages.WM_GETMINMAXINFO)
                {
                    if (WmGetMinMaxInfo(ref m))
                        return;
                }
            }
            base.WndProc(ref m);
        }

        #endregion
    }
}
