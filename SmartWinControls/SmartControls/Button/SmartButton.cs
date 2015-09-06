using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using SmartWinControls.SmartControls.Common;
using SmartWinControls.Common;
using SmartWinControls.Painter;

namespace SmartWinControls.SmartControls.Button
{
    public class SmartButton : System.Windows.Forms.Button, ISmartControl
    {
        #region 构造函数及初始化

        public SmartButton()
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
            get { return SmartControlType.Button; }
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
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        #endregion

        #region 新增公开属性

        bool _autoSize = true;
        bool _drawFocusRect = false;
        ButtonImageAlignment _pathAlign = ButtonImageAlignment.Left;
        ButtonForePathGetter _forePathGetter = null;
        Size _forePathSize;
        Size _imageSize;
        int _space;
        Image _foreImage;

        [Browsable(false)]
        public SmartButtonThemeBase XTheme
        {
            get
            {
                return InnerButton.XTheme;
            }
        }

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
        public ButtonImageAlignment ForePathAlign
        {
            get
            {
                return _pathAlign;
            }
            set
            {
                if (_pathAlign != value)
                {
                    _pathAlign = value;
                    InnerButton.ImageAlign = _pathAlign;
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
                InnerButton.Text = base.Text;
                SetBestSize();
            }
        }

        [Browsable(false)]
        public ButtonForePathGetter ForePathGetter
        {
            get
            {
                return _forePathGetter;
            }
            set
            {
                if (_forePathGetter != value)
                {
                    _forePathGetter = value;
                    InnerButton.ForePathGetter = _forePathGetter;
                    base.Invalidate();
                }
            }
        }

        public Size ForePathSize
        {
            get
            {
                return _forePathSize;
            }
            set
            {
                if (_forePathSize != value)
                {
                    _forePathSize = value;
                    InnerButton.ForePathSize = _forePathSize;
                    base.Invalidate();
                }
            }
        }

        public Size ForeImageSize
        {
            get
            {
                return _imageSize;
            }
            set
            {
                if (_imageSize != value)
                {
                    _imageSize = value;
                    InnerButton.ForeImageSize = _imageSize;
                    base.Invalidate();
                }
            }
        }

        public int SpaceBetweenPathAndText
        {
            get
            {
                return _space;
            }
            set
            {
                _space = value;
                InnerButton.SpaceBetweenImageAndText = _space;
                base.Invalidate();
            }
        }

        public Image ForeImage
        {
            get
            {
                return _foreImage;
            }
            set
            {
                _foreImage = value;
                InnerButton.ForeImage = _foreImage;
                base.Invalidate();
            }
        }

        #endregion

        #region 新增公开方法

        public void SetNewTheme(SmartButtonThemeBase xtheme)
        {
            InnerButton.SetNewTheme(xtheme);
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
        
        private SmartButtonState state;
        private WLButton _innerButton;

        private WLButton InnerButton
        {
            get
            {
                if (_innerButton == null)
                {
                    _innerButton = new WLButton(this);
                    _innerButton.Bounds = ClientRectangle;                    
                }
                return _innerButton;
            }
        }        

        private Size BestFittedSize
        {
            get
            {
                Size textSize = TextRenderer.MeasureText(base.Text, XTheme.TextFont);
                Size pathSize = Size.Empty;
                int space = 0;

                if (ForePathGetter != null)
                {
                    pathSize = ForePathSize;
                    space = SpaceBetweenPathAndText;
                }
                else if (ForeImage != null)
                {
                    pathSize = ForeImageSize;
                    space = SpaceBetweenPathAndText;
                }

                int w, h;
                if (ForePathAlign == ButtonImageAlignment.Left || ForePathAlign == ButtonImageAlignment.Right)
                {
                    w = XTheme.InnerPadding.Left + pathSize.Width + space +
                        textSize.Width + XTheme.InnerPadding.Right;
                    h = XTheme.InnerPadding.Top + XTheme.InnerPadding.Bottom +
                        Math.Max(pathSize.Height, textSize.Height);
                }
                else
                {
                    w = XTheme.InnerPadding.Left + XTheme.InnerPadding.Right +
                        Math.Max(pathSize.Width, textSize.Width);
                    h = XTheme.InnerPadding.Top + XTheme.InnerPadding.Bottom + space +
                        pathSize.Height + textSize.Height;
                }
                return new Size(w, h);
            }
        }

        #endregion

        #region 内部绘图

        private void PaintButton(Graphics g)
        {
            SetBestSize();
            InnerButton.PaintControl(g);
            if (DrawFocusRect && Focused)
            {
                BasicBlockPainter.RenderFocusRect(g, ClientRectangle, 2);
            }
        }

        #endregion

        #region 重写基类方法

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            base.OnPaintBackground(pevent);

            PaintButton(pevent.Graphics);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetBestSize();
            InnerButton.Bounds = ClientRectangle;
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            state = SmartButtonState.Hover;
            InnerButton.State = state;
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            state = SmartButtonState.Normal;
            InnerButton.State = state;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            state = SmartButtonState.Pressed;
            InnerButton.State = state;
            base.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (ClientRectangle.Contains(mevent.Location))
                state = SmartButtonState.Hover;
            else
                state = SmartButtonState.Normal;
            InnerButton.State = state;
            base.Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            InnerButton.Enabled = base.Enabled;
            base.Refresh();
        }

        #endregion
    }
}
