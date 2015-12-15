using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.Common;
using SmartWinControls.Common;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SmartWinControls.SmartControls.Button;
using SmartWinControls.SmartControls.ListBox;
using SmartWinControls.Functions;
using SmartWinControls.Painter;

namespace SmartWinControls.SmartControls.ComboBox
{
    public class SmartComboBox : SmartBarControlBase, ISmartControl
    {
        #region constructors

        public SmartComboBox()
        {
            TabStop = true;
        }

        #endregion

        #region const

        private const int CtlLeft = 0;
        private const int CtlTop = 0;

        #endregion

        #region ISmartControl

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public SmartControlType ControlType
        {
            get { return SmartControlType.ComboBox; }
        }

        #endregion

        #region private vars

        /// <summary>
        /// 该值指示下拉框是否处于显示状态
        /// </summary>
        private bool isDropDown = false;

        /// <summary>
        /// 该值指示下拉框是否第一次显示，如果是第一次显示，则应该调整到默认大小，
        /// 即使下拉框是可以拖动改变大小的
        /// </summary>
        private bool dropDownFirstShow = true;

        /// <summary>
        /// 该变量指示是否是内部对InnerTextBox赋值
        /// </summary>
        private bool innerSetTextBoxValue;

        /// <summary>
        /// number of onpaint method called
        /// </summary>
        private int paintCount = 0;

        #endregion

        #region private methods

        #region core private

        private bool IsIndexValid(int index)
        {
            return (index >= 0 && index <= Items.Count - 1);
        }

        private bool CanShowBelowControl(int height)
        {
            Rectangle workingRect = Screen.GetWorkingArea(this);
            Point screenPoint = PointToScreen(Point.Empty);
            int belowHeight = workingRect.Bottom - (screenPoint.Y + base.Height);
            int aboveHieght = screenPoint.Y - workingRect.Top;

            if (belowHeight >= height)
                return true;
            else if (belowHeight >= aboveHieght)
                return true;
            return false;
        }

        /// <summary>
        /// 显示下拉框
        /// </summary>
        private void ShowDropDown()
        {
            if (DropDownResizable)
            {
                ShowDropDownResizable();
                return;
            }

            InnerDropDown.Size = new System.Drawing.Size(
                base.Width, 1 + InnerListBox.GetBoxHeightForTheseRows(DropDownRows) + 1);
            InnerDropDown.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            InnerListBox.Size = new Size(
                InnerDropDown.Width - InnerDropDown.Padding.Horizontal,
                InnerDropDown.Height - InnerDropDown.Padding.Vertical);

            if (CanShowBelowControl(InnerDropDown.Height))
            {
                InnerDropDown.Show(this, new Point(0, base.Height + 1));
            }
            else
            {
                InnerDropDown.Show(this, new Point(0, -InnerDropDown.Height - 1));
            }
        }

        private void ShowDropDownResizable()
        {
            int dropDownHeight = InnerDropDown.Height;
            int dropDownWidth = InnerDropDown.Width;
            if (dropDownFirstShow)
            {
                dropDownHeight = 1 + InnerListBox.GetBoxHeightForTheseRows(DropDownRows)
                    + ResizeGridSize.Height + 1;
                dropDownWidth = base.Width;
                InnerDropDown.Size = new System.Drawing.Size(dropDownWidth, dropDownHeight);
                dropDownFirstShow = false;
            }
            bool below;
            if (CanShowBelowControl(dropDownHeight))
            {
                below = true;
                InnerDropDown.WhereIsResizeGrid = ResizeGridLocation.BottomRight;
                InnerDropDown.Padding = new Padding(1, 1, 1, 1 + ResizeGridSize.Height);
            }
            else
            {
                below = false;
                InnerDropDown.WhereIsResizeGrid = ResizeGridLocation.TopRight;
                InnerDropDown.Padding = new Padding(1, 1 + ResizeGridSize.Height, 1, 1);
            }

            InnerListBox.Size = new Size(
                InnerDropDown.Width - InnerDropDown.Padding.Horizontal,
                InnerDropDown.Height - InnerDropDown.Padding.Vertical);

            if (below)
            {
                InnerDropDown.Show(this, new Point(0, base.Height + 1));
            }
            else
            {
                InnerDropDown.Show(this, new Point(0, -InnerDropDown.Height - 1));
            }
        }

        #endregion

        #region do when something hanppen


        #endregion

        #region do when something change

        private void DoWhenSizeChanged()
        {
            Rectangle r = TextBoxRect;
            InnerTextBox.Location = r.Location;
            InnerTextBox.Size = r.Size;
            InnerRightButton.Bounds = RightButtonRect;
        }

        private void DoWhenThemeChanged()
        {
            if (XTheme.RightButtonTheme != null)
            {
                InnerRightButton.SetNewTheme(XTheme.RightButtonTheme);
            }
            if (XTheme.ListBoxTheme != null)
            {
                XTheme.ListBoxTheme.DrawBorder = false;
                XTheme.ListBoxTheme.InnerPaddingWidth = 1;
                InnerListBox.SetNewTheme(XTheme.ListBoxTheme);
            }
            InnerDropDown.ResizeGridColor = XTheme.ResizeGridColor;
            InnerDropDown.BorderColor = XTheme.DropDownBorderColor;
            InnerDropDown.BackColor = XTheme.DropDownBackColor;

            InnerTextBox.Font = XTheme.ComboTextFont;
            InnerTextBox.ForeColor = XTheme.ComboTextColor;

            DoWhenSizeChanged();
        }

        #endregion

        #region Inner control event handler

        private void InnerRightButton_MouseDown()
        {
            if (isDropDown)
            {
                InnerDropDown.Close();
            }
            else
            {
                ShowDropDown();
            }
        }

        private void InnerListBox_ItemClick(object sender, int index)
        {
            object obj = InnerListBox.SelectedValue;
            if (obj != null)
            {
                _text = obj.ToString();
            }
            else
            {
                _text = "";
            }
            SelectedIndex = InnerListBox.SelectedIndex;

            innerSetTextBoxValue = true;
            InnerTextBox.Text = this.Text;
            innerSetTextBoxValue = false;

            if (!Editable)
            {
                base.Invalidate();
            }
            InnerDropDown.Close(ToolStripDropDownCloseReason.ItemClicked);
        }

        void InnerDropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            isDropDown = false;
        }

        void InnerDropDown_Opened(object sender, EventArgs e)
        {
            isDropDown = true;
        }

        void InnerDropDown_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
            {
                Point location = base.PointToClient(Cursor.Position);
                if (RightButtonRect.Contains(location) ||
                    (!Editable && TextBoxRect.Contains(location)))
                {
                    e.Cancel = true;
                }
            }
        }

        void InnerDropDown_Resize(object sender, EventArgs e)
        {
            if (DropDownResizable)
            {
                InnerListBox.Size = new Size(
                    InnerDropDown.Width - InnerDropDown.Padding.Horizontal,
                    InnerDropDown.Height - InnerDropDown.Padding.Vertical);
            }
        }

        void InnerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Editable)
            {
                if (innerSetTextBoxValue)
                    return;

                // 如果不是内部赋值，那就是用户通过键盘，鼠标右键粘贴，Ctl+V等方式更改值

                this.Text = InnerTextBox.Text;
                SelectedIndex = -1;
                OnComboTextChanged(EventArgs.Empty);
            }
        }

        #endregion

        #endregion

        #region private property

        #region readonly rect

        private Rectangle TextBoxRect
        {
            get
            {
                int x, y, w, h;

                if (Editable)
                {
                    x = CtlLeft + XTheme.TextBoxMarginEditable.Left;
                    y = CtlTop + (base.Height - InnerTextBox.Height) / 2;
                    w = base.Width - XTheme.TextBoxMarginEditable.Horizontal -
                        XTheme.RightButtonMargin.Horizontal - XTheme.RightButtonWidth;
                    h = InnerTextBox.Height;
                }
                else
                {
                    x = CtlLeft + XTheme.TextBoxMarginNotEditable.Left;
                    y = CtlTop + XTheme.TextBoxMarginNotEditable.Top;
                    w = base.Width - XTheme.TextBoxMarginNotEditable.Horizontal -
                        XTheme.RightButtonMargin.Horizontal - XTheme.RightButtonWidth;
                    h = base.Height - XTheme.TextBoxMarginNotEditable.Vertical;
                }

                return new Rectangle(x, y, w, h);
            }
        }

        private Rectangle RightButtonRect
        {
            get
            {
                int x, y, w, h;
                x = CtlLeft + base.Width - XTheme.RightButtonMargin.Right - XTheme.RightButtonWidth;
                y = CtlTop + XTheme.RightButtonMargin.Top;
                w = XTheme.RightButtonWidth;
                h = base.Height - XTheme.RightButtonMargin.Vertical;
                return new Rectangle(x, y, w, h);
            }
        }

        #endregion

        #region inner

        private SmartButtonState _state = SmartButtonState.Normal;
        private SmartButtonState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    base.Invalidate();
                }
            }
        }

        #endregion

        #endregion

        #region Inner controls

        private TextBox _innerTextBox;
        private WLButton _innerRightButton;
        private SmartToolStripDropDown _innerDropDown;
        private ToolStripControlHost _innerControlHost;
        private SmartListBox _innerListBox;

        private TextBox InnerTextBox
        {
            get
            {
                if (_innerTextBox == null)
                {
                    _innerTextBox = new TextBox();
                    _innerTextBox.BorderStyle = BorderStyle.None;
                    _innerTextBox.TextChanged += new EventHandler(InnerTextBox_TextChanged);
                    base.Controls.Add(_innerTextBox);
                    //_innerTextBox.BackColor = Color.LightPink;
                }
                return _innerTextBox;
            }
        }

        private WLButton InnerRightButton
        {
            get
            {
                if (_innerRightButton == null)
                {
                    _innerRightButton = new WLButton(this);
                    _innerRightButton.ForePathGetter = new ButtonForePathGetter(
                        GraphicsPathHelper.Create7x4In7x7DownTriangleFlag);
                    _innerRightButton.ForePathSize = new System.Drawing.Size(7, 7);
                }
                return _innerRightButton;
            }
        }

        private SmartListBox InnerListBox
        {
            get
            {
                if (_innerListBox == null)
                {
                    _innerListBox = new SmartListBox();
                    _innerListBox.Size = new Size(40, 40);
                    _innerListBox.ItemClick += new ListItemClickHandler(InnerListBox_ItemClick);
                    _innerListBox.HighLightItemWhenMouseMove = true;
                    _innerListBox.XTheme.DrawBorder = false;
                    _innerListBox.XTheme.InnerPaddingWidth--;
                }
                return _innerListBox;
            }
        }

        private SmartToolStripDropDown InnerDropDown
        {
            get
            {
                if (_innerDropDown == null)
                {
                    _innerDropDown = new SmartToolStripDropDown();
                    _innerDropDown.Items.Add(InnerControlHost);
                    _innerDropDown.Padding = Padding.Empty;
                    _innerDropDown.Opened += new EventHandler(InnerDropDown_Opened);
                    _innerDropDown.Closed += new ToolStripDropDownClosedEventHandler(InnerDropDown_Closed);
                    _innerDropDown.Closing += new ToolStripDropDownClosingEventHandler(InnerDropDown_Closing);
                    _innerDropDown.Resize += new EventHandler(InnerDropDown_Resize);
                    _innerDropDown.AutoSize = false;
                }
                return _innerDropDown;
            }
        }

        private ToolStripControlHost InnerControlHost
        {
            get
            {
                if (_innerControlHost == null)
                {
                    _innerControlHost = new ToolStripControlHost(InnerListBox, "ListBoxForComboBox");
                    _innerControlHost.Padding = Padding.Empty;
                    _innerControlHost.Margin = Padding.Empty;
                }
                return _innerControlHost;
            }
        }

        #endregion

        #region Inner Draw

        private void PaintBackground(Graphics g, Rectangle clipRect)
        {
            if (XTheme.HowBackgroundRender == BackgroundStyle.Flat)
            {
                Color c = XTheme.BackColorNormal;
                if (State == SmartButtonState.Hover)
                    c = XTheme.BackColorHover;
                BasicBlockPainter.RenderFlatBackground(
                    g, ClientRectangle, c, ButtonBorderType.Rectangle,
                    XTheme.BorderRoundedRadius, RoundStyle.All);
            }
            else if (XTheme.HowBackgroundRender == BackgroundStyle.LinearGradient)
            {
                Color c1, c2;
                if (State == SmartButtonState.Hover)
                {
                    c1 = XTheme.BackColorLG1Hover;
                    c2 = XTheme.BackColorLG2Hover;
                }
                else
                {
                    c1 = XTheme.BackColorLG1Normal;
                    c2 = XTheme.BackColorLG2Normal;
                }
                BasicBlockPainter.RenderLinearGradientBackground(
                    g, ClientRectangle, c1, c2, 90.001f, XTheme.BorderRoundedRadius, RoundStyle.All);
            }
        }

        private void PaintBorder(Graphics g, Rectangle clipRect)
        {
            if (XTheme.DrawBorder)
            {
                Color borderColor = XTheme.BorderColorNormal;
                if (State == SmartButtonState.Hover)
                    borderColor = XTheme.BorderColorHover;

                BasicBlockPainter.RenderBorder(
                    g, ClientRectangle, borderColor, ButtonBorderType.Rectangle,
                    XTheme.BorderRoundedRadius, RoundStyle.All);
            }
        }

        private void PaintContent(Graphics g, Rectangle clipRect)
        {
            if (clipRect.IntersectsWith(RightButtonRect))
            {
                InnerRightButton.PaintControl(g, clipRect);
            }
            if (!Editable)
            {
                Rectangle tbRect = TextBoxRect;
                if (clipRect.IntersectsWith(tbRect))
                {
                    if (ItemDrawMode == ListDrawMode.AutoDraw)
                    {
                        PaintComboText(g, tbRect);
                    }
                    else
                    {
                        OnDrawComboText(new DrawComboTextEventArgs(g, tbRect, SelectedValue));
                    }
                }
            }
        }

        private void PaintComboText(Graphics g, Rectangle textRect)
        {
            TextRenderer.DrawText(
                g,
                this.Text,
                XTheme.ComboTextFont,
                textRect,
                XTheme.ComboTextColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.PreserveGraphicsClipping);

            if (DrawFocusRect && base.Focused)
            {
                BasicBlockPainter.RenderFocusRect(
                    g, textRect, 0);
            }

            //BasicBlockPainter.RenderBorder(
            //    g, textRect, Color.Red, ButtonBorderType.Rectangle);

        }

        #endregion

        #region Events

        // selected index changed
        private static readonly object Event_SelectedIndexChanged = new object();
        public event EventHandler SelectedIndexChanged
        {
            add { base.Events.AddHandler(Event_SelectedIndexChanged, value); }
            remove { base.Events.RemoveHandler(Event_SelectedIndexChanged, value); }
        }
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[Event_SelectedIndexChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // list box draw item
        public event ListItemPaintHandler DrawItem
        {
            add { InnerListBox.DrawItem += value; }
            remove { InnerListBox.DrawItem -= value; }
        }

        // list box measure item
        public event MeasureItemEventHandler MeasureItem
        {
            add { InnerListBox.MeasureItem += value; }
            remove { InnerListBox.MeasureItem -= value; }
        }

        // draw combo text
        private static readonly object Event_DrawComboText = new object();
        public event DrawComboTextEventHandler DrawComboText
        {
            add { base.Events.AddHandler(Event_DrawComboText, value); }
            remove { base.Events.RemoveHandler(Event_DrawComboText, value); }
        }
        protected virtual void OnDrawComboText(DrawComboTextEventArgs e)
        {
            DrawComboTextEventHandler handler = (DrawComboTextEventHandler)
                base.Events[Event_DrawComboText];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // combo text changed, fired when editable is true and text value of InnerTextBox changed
        private static readonly object Event_ComboTextChanged = new object();
        public event EventHandler ComboTextChanged
        {
            add { base.Events.AddHandler(Event_ComboTextChanged, value); }
            remove { base.Events.RemoveHandler(Event_ComboTextChanged, value); }
        }
        protected virtual void OnComboTextChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[Event_ComboTextChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Public proterties

        SmartComboBoxThemeBase _xtheme;
        bool _editable = true;
        int _dropDownRows = 10;
        bool _drawFocusRect = false;
        int _selectedIndex = -1;
        string _text;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public SmartComboBoxThemeBase XTheme
        {
            get
            {
                if (_xtheme == null)
                {
                    _xtheme = new SmartComboBoxThemeBase();
                    DoWhenThemeChanged();
                }
                return _xtheme;
            }
        }

        [Browsable(false)]
        public SmartEventsCollection Items
        {
            get
            {
                return InnerListBox.Items;
            }
        }

        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (!Editable)
                {
                    return;
                }
                _text = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示ComboBox是否是可编辑的
        /// </summary>
        [DefaultValue(true)]
        public bool Editable
        {
            get
            {
                return _editable;
            }
            set
            {
                if (_editable != value)
                {
                    _editable = value;
                    InnerTextBox.Visible = _editable;
                    base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示每次显示下拉框时，最多显示多少项
        /// </summary>
        [DefaultValue(10)]
        public int DropDownRows
        {
            get
            {
                return _dropDownRows;
            }
            set
            {
                if (_dropDownRows != value)
                {
                    _dropDownRows = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示下拉框是否可以通过鼠标拖动改变大小
        /// </summary>
        [DefaultValue(true)]
        public bool DropDownResizable
        {
            get
            {
                return InnerDropDown.Resizable;
            }
            set
            {
                InnerDropDown.Resizable = value;
            }
        }

        /// <summary>
        /// 获取或设置ResizeGrid的大小
        /// </summary>
        [DefaultValue(typeof(Size), "16,16")]
        public Size ResizeGridSize
        {
            get
            {
                return InnerDropDown.ResizeGridSize;
            }
            set
            {
                InnerDropDown.ResizeGridSize = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，指示在控件获得焦点时，是否绘制虚线边框
        /// </summary>
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
                }
            }
        }

        /// <summary>
        /// 获取或设置列表项（下拉框项及不可编辑状态下的ComboText项）的绘制方式，
        /// 自动绘制还是手动绘制。
        /// </summary>
        public ListDrawMode ItemDrawMode
        {
            get
            {
                return InnerListBox.ItemDrawMode;
            }
            set
            {
                InnerListBox.ItemDrawMode = value;
            }
        }

        /// <summary>
        /// 获取或设置鼠标移动时是否高亮下拉框对应项
        /// </summary>
        public bool HotTrackItems
        {
            get
            {
                return InnerListBox.HighLightItemWhenMouseMove;
            }
            set
            {
                InnerListBox.HighLightItemWhenMouseMove = value;
            }
        }

        /// <summary>
        /// 获取或设置下拉框中每项的高度
        /// </summary>
        public int ItemHeight
        {
            get
            {
                return InnerListBox.ItemHeight;
            }
            set
            {
                InnerListBox.ItemHeight = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，指示当前选中的项的下标
        /// </summary>
        [Browsable(false)]
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (!IsIndexValid(value))
                    value = -1;
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    if (_selectedIndex == -1 && !Editable)
                        _text = string.Empty;
                    InnerListBox.SelectedIndex = _selectedIndex;
                    object obj = InnerListBox.SelectedValue;
                    if (obj != null)
                        _text = obj.ToString();
                    else
                        _text = string.Empty;
                    OnSelectedIndexChanged(EventArgs.Empty);
                    if (!Editable)
                        base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取当前选中的object值
        /// </summary>
        [Browsable(false)]
        public object SelectedValue
        {
            get
            {
                if (IsIndexValid(SelectedIndex))
                    return Items[SelectedIndex];
                return null;
            }
        }

        #endregion

        #region public methods

        public void SetNewTheme(SmartComboBoxThemeBase xtheme)
        {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            DoWhenThemeChanged();
        }

        #endregion

        #region override base methods

        #region mouse operation

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InnerRightButton.MouseOperation(e, MouseOperationType.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();

            InnerRightButton.MouseOperation(e, MouseOperationType.Down);

            if (!Editable || RightButtonRect.Contains(e.Location))
            {
                InnerRightButton_MouseDown();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            InnerRightButton.MouseOperation(e, MouseOperationType.Up);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            InnerRightButton.MouseOperation(Point.Empty, MouseOperationType.Leave);

            State = SmartButtonState.Normal;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            State = SmartButtonState.Hover;
        }

        #endregion

        #region paint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Editable && paintCount < 2)
            {
                paintCount++;
                DoWhenSizeChanged();
            }

            PaintBackground(e.Graphics, e.ClipRectangle);
            PaintContent(e.Graphics, e.ClipRectangle);
            PaintBorder(e.Graphics, e.ClipRectangle);
        }

        #endregion

        #region property changes

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            DoWhenSizeChanged();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DoWhenSizeChanged();
        }

        #endregion

        #region action

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InnerDropDown.Dispose();
                InnerControlHost.Dispose();
                InnerListBox.Dispose();
                InnerTextBox.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (DrawFocusRect)
                base.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            if (DrawFocusRect)
                base.Invalidate();
        }

        #endregion

        #endregion

        #region Debug

        public string GetDebugInfo()
        {
            string s = "textbox-rect: " + TextBoxRect.ToString() + "\r\n" +
                "textbox.bounds: " + InnerListBox.Bounds.ToString() + "\r\n" +
                "combobox-rect: " + ClientRectangle.ToString();
            return s;
        }

        #endregion
    }

    #region event handler, args

    public class DrawComboTextEventArgs : EventArgs
    {
        Graphics graphics;
        Rectangle textRect;
        object selectedValue;

        public DrawComboTextEventArgs(Graphics g, Rectangle textRect, object selectedValue)
            : base()
        {
            graphics = g;
            this.textRect = textRect;
            this.selectedValue = selectedValue;
        }

        public Graphics Graphics
        {
            get { return graphics; }
        }

        public Rectangle TextRect
        {
            get { return textRect; }
        }

        public object SelectedValue
        {
            get { return selectedValue; }
        }
    }

    public delegate void DrawComboTextEventHandler(object sender, DrawComboTextEventArgs e);

    #endregion
}
