using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.Common;
using System.Windows.Forms;
using SmartWinControls.Common;
using System.ComponentModel;
using System.Drawing;

namespace SmartWinControls.SmartControls.CheckBox
{
    public class SmartCheckBox: System.Windows.Forms.CheckBox, ISmartControl
    {
        #region 构造函数及初始化

        public SmartCheckBox()
        {
            base.SetStyle(ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.OptimizedDoubleBuffer, true);

            base.AutoSize = false;
            state = SmartButtonState.Normal;
        }

        #endregion

        #region ISmartControl实现

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public SmartControlType ControlType
        {
            get { return SmartControlType.CheckBox; }
        }

        #endregion

        #region 隐藏基类无关属性

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Appearance Appearance
        {
            get
            {
                return base.Appearance;
            }
            set
            {
                base.Appearance = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get
            {
                return base.BackgroundImageLayout;
            }
            set
            {
                base.BackgroundImageLayout = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ContentAlignment CheckAlign
        {
            get
            {
                return base.CheckAlign;
            }
            set
            {
                base.CheckAlign = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                base.TextAlign = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new TextImageRelation TextImageRelation
        {
            get
            {
                return base.TextImageRelation;
            }
            set
            {
                base.TextImageRelation = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                base.Image = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ContentAlignment ImageAlign
        {
            get
            {
                return base.ImageAlign;
            }
            set
            {
                base.ImageAlign = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new FlatStyle FlatStyle
        {
            get
            {
                return base.FlatStyle;
            }
            set
            {
                base.FlatStyle = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ImageList ImageList
        {
            get
            {
                return base.ImageList;
            }
            set
            {
                base.ImageList = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new int ImageIndex
        {
            get
            {
                return base.ImageIndex;
            }
            set
            {
                base.ImageIndex = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string ImageKey
        {
            get
            {
                return base.ImageKey;
            }
            set
            {
                base.ImageKey = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoEllipsis
        {
            get
            {
                return base.AutoEllipsis;
            }
            set
            {
                base.AutoEllipsis = value;
            }
        }

        #endregion

        #region 新增公开属性

        bool _autoSize = true;
        bool _drawFocusRect = false;
        CheckMarkAlignment _radioMarkAlign = CheckMarkAlignment.Left;

        [DefaultValue(true)]
        public new bool AutoSize
        {
            get
            {
                return _autoSize;
            }
            set
            {
                if (_autoSize != value)
                {
                    base.AutoSize = false;
                    _autoSize = value;

                    DoWhenAutoSizeChanged();
                }
            }
        }

        [DefaultValue(false)]
        public bool DrawFocusRect
        {
            get
            {
                return _drawFocusRect;
            }
            set
            {
                if (_drawFocusRect != value)
                {
                    _drawFocusRect = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(CheckMarkAlignment), "0")]
        public CheckMarkAlignment CheckRectAlign
        {
            get
            {
                return _radioMarkAlign;
            }
            set
            {
                if (_radioMarkAlign != value)
                {
                    _radioMarkAlign = value;
                    base.Invalidate();
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                SetBestSize();
            }
        }

        #endregion

        #region 新增公开方法

        public void SetNewTheme(SmartCheckBoxThemeBase xtheme)
        {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            base.Invalidate();
        }

        #endregion

        #region 内部方法

        private void DoWhenAutoSizeChanged()
        {
            base.SetStyle(ControlStyles.FixedHeight, _autoSize);
            base.SetStyle(ControlStyles.FixedWidth, _autoSize);
            SetBestSize();
        }

        private void SetBestSize()
        {
            if (_autoSize)
            {
                Size bestSize = BestFittedSize;
                if (base.Size != bestSize)
                    base.Size = bestSize;
            }
        }

        #endregion

        #region 内部属性, 变量

        private SmartCheckBoxThemeBase _xtheme;
        private SmartButtonState state;

        private SmartCheckBoxThemeBase XTheme
        {
            get
            {
                if (_xtheme == null)
                    _xtheme = new SmartCheckBoxThemeBase();
                return _xtheme;
            }
        }

        private Size BestFittedSize
        {
            get
            {
                Size textSize = TextRenderer.MeasureText(base.Text, XTheme.TextFont);
                int w = XTheme.InnerPaddingWidth * 2 + XTheme.CheckRectWidth +
                    XTheme.SpaceBetweenCheckMarkAndText + textSize.Width;
                int h = XTheme.InnerPaddingWidth * 2 + Math.Max(XTheme.CheckRectWidth, textSize.Height);
                return new Size(w, h);
            }
        }

        #endregion

        #region 内部绘图

        private void PaintCButton(Graphics g)
        {
            CheckButtonPainter.RenderCheckButton(
                g,
                ClientRectangle,
                XTheme,
                base.Enabled,
                CheckState,
                state,
                base.Text,
                CheckRectAlign,
                DrawFocusRect && base.Focused);
        }

        #endregion

        #region 重写基类方法

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);

            PaintCButton(pevent.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetBestSize();
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            state = SmartButtonState.Hover;
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            state = SmartButtonState.Normal;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            state = SmartButtonState.Pressed;
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (ClientRectangle.Contains(mevent.Location))
                state = SmartButtonState.Hover;
            else
                state = SmartButtonState.Normal;
            base.Invalidate();
        }

        #endregion
    }
}
