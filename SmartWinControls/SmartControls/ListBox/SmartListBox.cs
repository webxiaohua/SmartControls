using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.Common;
using SmartWinControls.Common;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace SmartWinControls.SmartControls.ListBox
{
    public class SmartListBox : SmartBarControlBase, ISmartControl
    {
        #region constructors

        public SmartListBox()
        {
            base.TabStop = true;
        }

        #endregion

        #region IGMControl

        [Browsable(false)]
        public SmartControlType ControlType
        {
            get { return SmartControlType.ListBox; }
        }

        #endregion

        #region Inner properties

        private WLListBox _innerListBox;

        private WLListBox InnerListBox
        {
            get
            {
                if (_innerListBox == null)
                {
                    _innerListBox = new WLListBox(this);
                }
                return _innerListBox;
            }
        }

        #endregion

        #region override base methods

        #region mouse operation transfer

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            InnerListBox.MouseOperation(e, MouseOperationType.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Focus();
            InnerListBox.MouseOperation(e, MouseOperationType.Down);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            InnerListBox.MouseOperation(e, MouseOperationType.Up);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            InnerListBox.MouseOperation(Point.Empty, MouseOperationType.Leave);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            InnerListBox.MouseOperation(e, MouseOperationType.Wheel);
        }

        #endregion

        #region key operation transfer

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            InnerListBox.KeyOperation(e, KeyOperationType.KeyDown);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            InnerListBox.KeyOperation(e, KeyOperationType.KeyUp);
        }

        #endregion

        #region property chnages

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            InnerListBox.Bounds = base.ClientRectangle;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            //InnerListBox.Bounds = base.ClientRectangle;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            InnerListBox.Enabled = base.Enabled;
        }

        #endregion

        #region change behaviour

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                return false;
            }
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.End:
                case Keys.Home:

                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        #endregion

        #region paint

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            InnerListBox.PaintControl(e.Graphics, e.ClipRectangle);
        }

        #endregion

        #endregion

        #region public events

        public event EventHandler SelectedIndexChanged
        {
            add { InnerListBox.SelectedIndexChanged += value; }
            remove { InnerListBox.SelectedIndexChanged -= value; }
        }

        public event EventHandler SelectionChanged
        {
            add { InnerListBox.SelectionChanged += value; }
            remove { InnerListBox.SelectionChanged -= value; }
        }

        public event ListItemClickHandler ItemClick
        {
            add { InnerListBox.ItemClick += value; }
            remove { InnerListBox.ItemClick -= value; }
        }

        public event ListItemClickHandler ItemDoubleClick
        {
            add { InnerListBox.ItemDoubleClick += value; }
            remove { InnerListBox.ItemDoubleClick -= value; }
        }

        public event ListItemPaintHandler DrawItem
        {
            add { InnerListBox.DrawItem += value; }
            remove { InnerListBox.DrawItem -= value; }
        }

        public event MeasureItemEventHandler MeasureItem
        {
            add { InnerListBox.MeasureItem += value; }
            remove { InnerListBox.MeasureItem -= value; }
        }

        #endregion

        #region public methods

        public void SetNewTheme(SmartListBoxThemeBase xtheme)
        {
            InnerListBox.SetNewTheme(xtheme);
        }

        public void ScrollToIndex(int index)
        {
            InnerListBox.ScrollToIndex(index);
        }

        public void RefleshTheme()
        {
            InnerListBox.RefleshTheme();
        }

        public int GetBoxHeightForTheseRows(int rows)
        {
            return InnerListBox.GetBoxHeightForTheseRows(rows);
        }

        #endregion

        #region public properties

        /// <summary>
        /// 获取一个集合，该集合表示列表中所有的项。可以通过该集合添加、删除项
        /// </summary>
        [Browsable(false)]
        public SmartEventsCollection Items
        {
            get
            {
                return InnerListBox.Items;
            }
        }

        /// <summary>
        /// 获取当前ListBox的主题
        /// </summary>
        [Browsable(false)]
        public SmartListBoxThemeBase XTheme
        {
            get
            {
                return InnerListBox.XTheme;
            }
        }

        /// <summary>
        /// 获取或设置垂直滚动条的显示模式
        /// </summary>
        [DefaultValue(typeof(ScrollBarShowMode),"2")]
        public ScrollBarShowMode ScrollBarVShowMode
        {
            get
            {
                return InnerListBox.ScrollBarVShowMode;
            }
            set
            {
                InnerListBox.ScrollBarVShowMode = value;
            }
        }

        /// <summary>
        /// 获取或设置水平滚动条的显示模式
        /// </summary>
        [DefaultValue(typeof(ScrollBarShowMode), "0")]
        public ScrollBarShowMode ScrollBarHShowMode
        {
            get
            {
                return InnerListBox.ScrollBarHShowMode;
            }
            set
            {
                InnerListBox.ScrollBarHShowMode = value;
            }
        }

        /// <summary>
        /// 获取或设置列表中每项的高度
        /// </summary>
        [DefaultValue(18)]
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
        /// 获取或设置列表中最新选中的项的下标
        /// </summary>
        [Browsable(false)]
        public int SelectedIndex
        {
            get
            {
                return InnerListBox.SelectedIndex;
            }
            set
            {
                InnerListBox.SelectedIndex = value;
            }
        }

        /// <summary>
        /// 获取或设置是否完整显示列表中最上面的项
        /// </summary>
        [DefaultValue(true)]
        public bool TopItemFullyShow
        {
            get
            {
                return InnerListBox.TopItemFullyShow;
            }
            set
            {
                InnerListBox.TopItemFullyShow = value;
            }
        }

        /// <summary>
        /// 获取或设置列表最顶部显示的项的下标
        /// </summary>
        [Browsable(false)]
        public int TopIndex
        {
            get
            {
                return InnerListBox.TopIndex;
            }
            set
            {
                InnerListBox.TopIndex = value;
            }
        }

        /// <summary>
        /// 获取当前选中的项所代表的object，若无选中项则返回null
        /// </summary>
        [Browsable(false)]
        public object SelectedValue
        {
            get
            {
                return InnerListBox.SelectedValue;
            }
        }

        /// <summary>
        /// 获取或设置列表选择模式，单选还是允许多选
        /// </summary>
        [DefaultValue(typeof(ListSelectionMode), "1")]
        public ListSelectionMode ItemSelectionMode
        {
            get
            {
                return InnerListBox.ItemSelectionMode;
            }
            set
            {
                InnerListBox.ItemSelectionMode = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值表示鼠标在列表中移动时，是否高亮鼠标所在项
        /// </summary>
        [DefaultValue(false)]
        public bool HighLightItemWhenMouseMove
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
        /// 获取或设置列表项的绘制方式，自动绘制还是用户自行绘制
        /// </summary>
        [DefaultValue(typeof(ListDrawMode),"0")]
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
        /// 获取当前选中的项的总数目
        /// </summary>
        [Browsable(false)]
        public int SelectionCount
        {
            get
            {
                return InnerListBox.SelectionCount;
            }
        }

        /// <summary>
        /// 获取所有当前选中项的下标的数组
        /// </summary>
        [Browsable(false)]
        public int[] SelectedIndexes
        {
            get
            {
                return InnerListBox.SelectedIndexes;
            }
        }

        /// <summary>
        /// 获取表示当前所有选中项的数组
        /// </summary>
        [Browsable(false)]
        public object[] SelectedValues
        {
            get
            {
                return InnerListBox.SelectedValues;
            }
        }

        #endregion

        #region hide base properties



        #endregion
    }
}
