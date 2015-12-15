using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.Common;
using System.Windows.Forms;
using System.Drawing;
using SmartWinControls.Common;
using SmartWinControls.SmartControls.ScrollBar;
using SmartWinControls.Painter;
using SmartWinControls.Functions;

namespace SmartWinControls.SmartControls.ListBox
{
    public class WLListBox : WLContainerBase
    {
        #region Const

        private static readonly int DEFAULT_ITEM_HEIGHT = 18;

        #endregion

        #region constructors

        public WLListBox(Control owner)
            : base(owner)
        {
        }

        #endregion

        #region private methods

        #region selection operation

        private void Add2Selection(int index)
        {
            if (IsIndexValid(index))
            {
                if (!listSelectedIndexes.Contains(index))
                {
                    listSelectedIndexes.Add(index);
                    OnSelectionChanged(EventArgs.Empty);
                }
            }
        }

        private void Add2Selection(int index1, int index2)
        {
            int oldCnt = listSelectedIndexes.Count;
            AddTheseRangeToSelected(index1, index2, listSelectedIndexes);
            if (listSelectedIndexes.Count > oldCnt)
                OnSelectionChanged(EventArgs.Empty);
        }

        private void Add2Selection(List<int> listTemp)
        {
            bool added = false;
            foreach (int i in listTemp)
            {
                if (!listSelectedIndexes.Contains(i))
                {
                    listSelectedIndexes.Add(i);
                    added = true;
                }
            }
            if (added)
                OnSelectionChanged(EventArgs.Empty);
        }

        private void DeleteFromSelection(int index)
        {
            if (listSelectedIndexes.Contains(index))
            {
                listSelectedIndexes.Remove(index);
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        private void ClearSelection()
        {
            if (listSelectedIndexes.Count > 0)
            {
                listSelectedIndexes.Clear();
                OnSelectionChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region useful core methods

        private Size MeasureItemSize(int index)
        {
            Size size = Size.Empty;
            if (ItemDrawMode == ListDrawMode.UserDraw)
            {
                MeasureItemEventArgs e = new MeasureItemEventArgs(null, index);
                OnMeasureItem(e);
                size.Width = e.ItemWidth;
                size.Height = e.ItemHeight;
            }
            else
            {
                size = TextRenderer.MeasureText(Items[index].ToString(), XTheme.TextFont,
                    Size.Empty, TextFormatFlags.NoPrefix);
            }
            return size;
        }

        /// <summary>
        /// 将indexFrom至indexTo的下标添加为已选定
        /// </summary>        
        private void AddTheseRangeToSelected(int indexFrom, int indexTo, List<int> list)
        {
            int small, big;
            if (indexFrom < indexTo)
            {
                small = indexFrom;
                big = indexTo;
            }
            else
            {
                small = indexTo;
                big = indexFrom;
            }
            if (small < 0)
                small = 0;
            else if (small > Items.Count - 1)
                small = Items.Count - 1;
            if (big < 0)
                big = 0;
            else if (big > Items.Count - 1)
                big = Items.Count - 1;
            for (int i = small; i <= big; i++)
            {
                if (!list.Contains(i))
                    list.Add(i);
            }
        }

        /// <summary>
        /// 判断指定的Index是否有效
        /// </summary>        
        private bool IsIndexValid(int index)
        {
            return (Items.Count > 0 && index >= 0 && index <= Items.Count - 1);
        }

        private int GetOriginalIndex()
        {
            if (IsIndexValid(SelectedIndex))
                return SelectedIndex;
            return mouseDownItemIndex;
        }

        private Rectangle GetItemRectFromIndex(int index)
        {
            if (!IsIndexValid(index) || index < FirstShowIndex || index > LastShowIndex)
                return Rectangle.Empty;

            Rectangle contentRect = ContentRectangle;
            int x = contentRect.X - ScrollBarHValue;
            int y = contentRect.Y - FirstItemYOffset + ItemHeight * (index - FirstShowIndex);
            int w = contentRect.Width + ScrollBarHValue;
            int h = ItemHeight;
            return new Rectangle(x, y, w, h);
        }

        private int GetItemIndexFromPoint(Point p)
        {
            int offset, num = -1;
            offset = p.Y - ContentRectangle.Y;

            if (TopItemFullyShow)
            {
                num = FirstShowIndex + offset / ItemHeight;
            }
            else
            {
                if (offset <= (ItemHeight - FirstItemYOffset))
                    num = FirstShowIndex;
                else
                {
                    offset -= (ItemHeight - FirstItemYOffset);
                    num = FirstShowIndex + offset / ItemHeight + 1;
                }
            }

            if (Items.Count == 0 || num > Items.Count - 1 || num < 0)
                num = -1;
            return num;
        }

        private void CalculateFirstLastShowIndex()
        {
            if (Items.Count == 0)
            {
                _firstShowIndex = _lastShowIndex = -1;
                return;
            }

            // first show index

            int num = ScrollBarVValue / ItemHeight;
            int remain = ScrollBarVValue % ItemHeight;

            if (TopItemFullyShow)
            {
                _firstItemYOffset = 0;
                _firstItemPartlyShow = false;
                if (remain >= (ItemHeight / 2))
                    num++;
            }
            else
            {
                _firstItemYOffset = remain;
                _firstItemPartlyShow = (remain > 0);
            }
            _firstShowIndex = num;

            // last show index

            Rectangle contentRect = ContentRectangle;
            if (TopItemFullyShow)
            {
                num = contentRect.Height / ItemHeight;
                remain = contentRect.Height % ItemHeight;
                if (remain > 0)
                    num++;
                _lastShowIndex = _firstShowIndex + num - 1;
            }
            else
            {
                int contentHeight = contentRect.Height - (ItemHeight - _firstItemYOffset);
                num = contentHeight / ItemHeight;
                remain = contentHeight % ItemHeight;
                if (remain > 0)
                    num++;
                _lastShowIndex = _firstShowIndex + num;
            }

            _lastItemPartlyShow = (remain > 0);
            _lastItemCutHeight = ItemHeight - remain;
            if (_lastShowIndex > Items.Count - 1)
            {
                _lastItemPartlyShow = false;
                _lastItemCutHeight = 0;
                _lastShowIndex = Items.Count - 1;
            }

            _topIndex = _firstShowIndex;
            if (_firstItemPartlyShow)
                _topIndex++;

            showIndexReady = true;
        }

        private void ResetScrollBarInfo()
        {
            ScrollBarV.Visible = NeedToShowScrollBarV;
            ScrollBarV.Bounds = ScrollBarVRect;
            ScrollBarH.Visible = NeedToShowScrollBarH;
            ScrollBarH.Bounds = ScrollBarHRect;

            showIndexReady = false;
            Rectangle r = ContentRectangle;
            if (ScrollBarV.Visible)
            {
                ScrollBarV.Enabled = (NeededHeight > r.Height);
                if (ScrollBarV.Enabled)
                {
                    ScrollBarV.MiddleButtonLengthPercentage = (int)((float)r.Height / NeededHeight * 100f);

                    int max = NeededHeight - r.Height;

                    if (TopItemFullyShow)
                    {
                        int remain = max % ItemHeight;
                        if (remain > 0 && remain < (ItemHeight / 2))
                            max += ItemHeight;
                    }
                    ScrollBarV.Maximum = max;
                }
            }
            if (ScrollBarH.Visible)
            {
                ScrollBarH.Enabled = (MaxItemWidth > r.Width);
                if (ScrollBarH.Enabled)
                {
                    ScrollBarH.Maximum = MaxItemWidth - r.Width;
                    ScrollBarH.MiddleButtonLengthPercentage = (int)((float)r.Width / MaxItemWidth * 100f);
                }
            }
            base.Invalidate();
        }

        private void RecheckMaxItemWidth()
        {
            _maxItemWidth = 0;
            for (int i = 0; i < Items.Count - 1; i++)
            {
                Size size = MeasureItemSize(i);
                if (size.Width > _maxItemWidth)
                    _maxItemWidth = size.Width;
            }
        }

        private void SetNewSelectedIndexAndScrollTo(int newIndex)
        {
            if (Items.Count == 0)
                return;
            if (ItemSelectionMode == ListSelectionMode.None)
            {
                ScrollToIndex(newIndex);
                newestItemIndexForKeySet = newIndex;
            }
            else if (ItemSelectionMode == ListSelectionMode.One)
            {
                ScrollToIndex(newIndex);
                SelectedIndex = newIndex;
            }
            else if (ItemSelectionMode == ListSelectionMode.Multiple)
            {
                if (isShiftKeyDown)
                {
                    // multi selection using up/down/pageup/pagedown/home/end key
                    int originIndex = GetOriginalIndex();
                    if (!IsIndexValid(originIndex))
                        return;
                    ClearSelection(); //listSelectedIndexes.Clear();
                    Add2Selection(originIndex, newIndex); //AddTheseRangeToSelected(originIndex, newIndex, listSelectedIndexes);
                    ScrollToIndex(newIndex);
                    newestItemIndexForKeySet = newIndex;
                    base.Invalidate(ContentRectangle);
                }
                else
                {
                    ClearSelection(); //listSelectedIndexes.Clear();
                    ScrollToIndex(newIndex);
                    SelectedIndex = newIndex;
                    if (IsIndexValid(newIndex))
                        Add2Selection(newIndex); //listSelectedIndexes.Add(newIndex);
                }
            }
        }

        private void AddToSelectedIndexAndScrollTo(int amount)
        {
            int originIndex = newestItemIndexForKeySet;
            if (ItemSelectionMode == ListSelectionMode.Multiple && isShiftKeyDown && !IsIndexValid(originIndex))
            {
                return;
            }
            int newSelectedIndex = originIndex + amount;
            if (newSelectedIndex < 0)
                newSelectedIndex = 0;
            if (newSelectedIndex > Items.Count - 1)
                newSelectedIndex = Items.Count - 1;
            SetNewSelectedIndexAndScrollTo(newSelectedIndex);
        }

        /// <summary>
        /// 处理向上箭头键按下操作，SelectedIndex将递减
        /// </summary>
        private void DealUpArrowKey()
        {
            AddToSelectedIndexAndScrollTo(-1);
        }

        /// <summary>
        /// 处理向下箭头键按下操作，SelectedIndex将递增
        /// </summary>
        private void DealDownArrowKey()
        {
            AddToSelectedIndexAndScrollTo(1);
        }

        /// <summary>
        /// 处理PageUp键按下操作，SelectedIndex将减掉一页
        /// </summary>
        private void DealPageUpKey()
        {
            AddToSelectedIndexAndScrollTo(-ShowIndexCount + 1);
        }

        /// <summary>
        /// 处理PageDown键按下操作，SelectedIndex将加上一页
        /// </summary>
        private void DealPageDownKey()
        {
            AddToSelectedIndexAndScrollTo(ShowIndexCount - 1);
        }

        /// <summary>
        /// 处理Home键按下操作，将定位到第一项并选中它
        /// </summary>
        private void DealHomeKey()
        {
            SetNewSelectedIndexAndScrollTo(0);
        }

        /// <summary>
        /// 处理End键按下操作，将定位到最后一项并选中它
        /// </summary>
        private void DealEndKey()
        {
            SetNewSelectedIndexAndScrollTo(Items.Count - 1);
        }

        private MouseDownLocation CheckLocation(Point location)
        {
            MouseDownLocation loc = MouseDownLocation.NoWhere;
            if (ContentRectangle.Contains(location))
                loc = MouseDownLocation.ItemArea;
            else if (ScrollBarV.Visible && ScrollBarV.Bounds.Contains(location))
                loc = MouseDownLocation.ScrollBars;
            else if (ScrollBarH.Visible && ScrollBarH.Bounds.Contains(location))
                loc = MouseDownLocation.ScrollBars;
            return loc;
        }

        #endregion

        #region response to action

        private void Items_CollectionChange(GMCollectionChangeArgs e)
        {
            if (e.Action == GMCollectionChangeAction.AfterInsert)
                DoWhenInsertItem(e.Index, e.Value);
            else if (e.Action == GMCollectionChangeAction.AfterRemove)
                DoWhenRemoveItem(e.Index, e.Value);
            else if (e.Action == GMCollectionChangeAction.AfterClear)
                DoWhenClearItem();
        }

        private void ScrollBarH_ValueChanged(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void ScrollBarV_ValueChanged(object sender, EventArgs e)
        {
            showIndexReady = false;
            base.Invalidate();
        }

        private void DoWhenThemeChanged()
        {
            if (ScrollBarHShowMode != ScrollBarShowMode.Never)
            {
                RecheckMaxItemWidth();
            }
            ScrollBarV.SetNewTheme(XTheme.ScrollBarTheme);
            ScrollBarH.SetNewTheme(XTheme.ScrollBarTheme);
            ResetScrollBarInfo();
        }

        private void DoWhenSizeChanged()
        {
            ResetScrollBarInfo();
        }

        private void DoWhenInsertItem(int index, object value)
        {
            Size size = MeasureItemSize(index);
            if (size.Width > _maxItemWidth)
                _maxItemWidth = size.Width;

            ResetScrollBarInfo();
        }

        private void DoWhenRemoveItem(int index, object value)
        {
            if (ScrollBarHShowMode != ScrollBarShowMode.Never)
            {
                Size size = TextRenderer.MeasureText(value.ToString(), XTheme.TextFont);
                if (_maxItemWidth == size.Width)
                    RecheckMaxItemWidth();
            }
            if (ItemSelectionMode == ListSelectionMode.Multiple)
            {
                DeleteFromSelection(index);

                if (listSelectedIndexes.Count > 0)
                {
                    for (int i = 0; i < listSelectedIndexes.Count; i++)
                    {
                        if (listSelectedIndexes[i] > index)
                            listSelectedIndexes[i] = listSelectedIndexes[i] - 1;
                    }
                }

                if (index == SelectedIndex)
                {
                    if (listSelectedIndexes.Count > 0)
                        SelectedIndex = listSelectedIndexes[listSelectedIndexes.Count - 1];
                    else
                        SelectedIndex = -1;
                }
            }

            ResetScrollBarInfo();
        }

        private void DoWhenClearItem()
        {
            _maxItemWidth = 0;
            ClearSelection(); //listSelectedIndexes.Clear();
            listSelectedIndexesTemp.Clear();
            SelectedIndex = -1;
            mouseDownItemIndex = -1;
            newestItemIndexForKeySet = -1;

            ResetScrollBarInfo();
        }

        private void DoWhenLeftButtonDownInItemsArea_One(Point location)
        {
            int index = GetItemIndexFromPoint(location);
            if (index != -1)
                SelectedIndex = index;
        }

        private void DoWhenLeftButtonDownInItemsArea_Multi(Point location)
        {
            int index = GetItemIndexFromPoint(location);
            if (index == -1)
                return;

            int newIndex = SelectedIndex;
            if (isControlKeyDown && isShiftKeyDown)
            {
                if (!IsIndexValid(SelectedIndex))
                    return;
                Add2Selection(SelectedIndex, index); // AddTheseRangeToSelected(SelectedIndex, index, listSelectedIndexes);                
            }
            else if (isControlKeyDown)
            {
                if (listSelectedIndexes.Contains(index))
                {
                    DeleteFromSelection(index); // listSelectedIndexes.Remove(index);
                    if (index == SelectedIndex)
                    {
                        if (listSelectedIndexes.Count > 0)
                            newIndex = listSelectedIndexes[listSelectedIndexes.Count - 1];
                        else
                            newIndex = -1;
                    }
                }
                else
                {
                    Add2Selection(index); // listSelectedIndexes.Add(index);
                    newIndex = index;
                }
            }
            else if (isShiftKeyDown)
            {
                int selected = GetOriginalIndex();
                if (!IsIndexValid(selected))
                    return;
                newIndex = selected;
                ClearSelection(); //listSelectedIndexes.Clear();
                Add2Selection(selected, index); //AddTheseRangeToSelected(selected, index, listSelectedIndexes);                
            }
            else
            {
                ClearSelection(); // listSelectedIndexes.Clear();
                Add2Selection(index); // listSelectedIndexes.Add(index);
                newIndex = index;
            }
            base.Invalidate(ContentRectangle);
            SelectedIndex = newIndex;
        }

        private void DoWhenLeftButtonUpInItemsArea(Point location, int clicks)
        {
            // item-click, item-double-click events
            int index = GetItemIndexFromPoint(location);
            bool lastItemClick = ((isMouseDownInLastPartlyShowIndex) && (index == -1 || index == mouseDownItemIndex + 1));
            if ((index != -1 && index == mouseDownItemIndex) || lastItemClick)
            {
                if (clicks == 1)
                {
                    OnItemClick(mouseDownItemIndex);
                }
                else if (clicks == 2)
                {
                    OnItemDoubleClick(mouseDownItemIndex);
                }
            }

            // merge temp selection item by down move to selected-list
            if (ItemSelectionMode == ListSelectionMode.Multiple && listSelectedIndexesTemp.Count > 0)
            {
                Add2Selection(listSelectedIndexesTemp);
                SelectedIndex = listSelectedIndexesTemp[listSelectedIndexesTemp.Count - 1];
                listSelectedIndexesTemp.Clear();
            }
        }

        private void DoWhenMouseMove(Point p)
        {
            if (whereMouseDown == MouseDownLocation.NotDown)
            {
                // 鼠标键未按下
                if (CheckLocation(p) == MouseDownLocation.ItemArea)
                {
                    if (HighLightItemWhenMouseMove)
                        HighLightIndex = GetItemIndexFromPoint(p);
                }
                else
                {
                    HighLightIndex = -1;
                }
            }
            else if (whereMouseDown == MouseDownLocation.ItemArea)
            {
                // 鼠标左键在列表区按下且移动
                if (ItemSelectionMode == ListSelectionMode.One || ItemSelectionMode == ListSelectionMode.None)
                    DealMouseDownMove_One(p);
                else if (ItemSelectionMode == ListSelectionMode.Multiple)
                    DealMouseDownMove_Multi(p);
            }
        }

        private void DealMouseDownMove_One(Point p)
        {
            Rectangle cr = ContentRectangle;
            int index = -1;
            if (p.Y < cr.Top && p.Y <= lastMouseDownMovePoint.Y)
            {
                if (SelectedIndex != 0)
                    index = FirstShowIndex - 1;
            }
            else if (p.Y > cr.Bottom && p.Y >= lastMouseDownMovePoint.Y)
            {
                if (SelectedIndex != Items.Count - 1)
                    index = LastShowIndex + 1;
            }
            else if (p.Y >= cr.Top && p.Y <= cr.Bottom)
            {
                index = GetItemIndexFromPoint(p);
            }

            if (index != -1)
            {
                ScrollToIndex(index);
                if (index < 0)
                    index = 0;
                if (index > Items.Count - 1)
                    index = Items.Count - 1;
                if (ItemSelectionMode == ListSelectionMode.One)
                    SelectedIndex = index;
            }
            lastMouseDownMovePoint = p;
        }

        private void DealMouseDownMove_Multi(Point p)
        {
            int originIndex = GetOriginalIndex();
            if (!IsIndexValid(originIndex))
                return;

            if (SelectedIndex != originIndex)
                _selectedIndex = originIndex;

            Rectangle cr = ContentRectangle;
            int index = -1;
            if (p.Y < cr.Top && p.Y <= lastMouseDownMovePoint.Y)
            {
                if (SelectedIndex != 0)
                    index = FirstShowIndex - 1;
            }
            else if (p.Y > cr.Bottom && p.Y >= lastMouseDownMovePoint.Y)
            {
                if (SelectedIndex != Items.Count - 1)
                    index = LastShowIndex + 1;
            }
            else if (p.Y >= cr.Top && p.Y <= cr.Bottom)
            {
                index = GetItemIndexFromPoint(p);
            }
            if (index == -1)
                return;

            if (index < 0)
                index = 0;
            if (index > Items.Count - 1)
                index = Items.Count - 1;

            if (index == lastMouseDownMoveItemIndex)
                return;

            ScrollToIndex(index);
            listSelectedIndexesTemp.Clear();
            AddTheseRangeToSelected(originIndex, index, listSelectedIndexesTemp);
            base.Invalidate(cr);
            lastMouseDownMoveItemIndex = index;
            lastMouseDownMovePoint = p;
        }

        #endregion

        #endregion

        #region private var

        private List<int> listSelectedIndexes = new List<int>();
        private List<int> listSelectedIndexesTemp = new List<int>();
        private bool isShiftKeyDown;
        private bool isControlKeyDown;
        private MouseDownLocation whereMouseDown = MouseDownLocation.NotDown;
        private Point pointMouseDown = Point.Empty;
        private int mouseDownItemIndex = -1;
        private bool isMouseDownInLastPartlyShowIndex = false;
        private int mouseDownClicks = 0;
        private Point lastMouseDownMovePoint = Point.Empty;
        private int lastMouseDownMoveItemIndex = -1;
        private int newestItemIndexForKeySet = -1;

        #endregion

        #region inner properties

        #region scrollbar wl control

        private WLScrollBar _scrollBarH;
        private WLScrollBar _scrollBarV;

        private WLScrollBar ScrollBarH
        {
            get
            {
                if (_scrollBarH == null)
                {
                    _scrollBarH = new WLScrollBar(base.Owner, Orientation.Horizontal);
                    _scrollBarH.SmallChange = 6;
                    _scrollBarH.LargeChange = 18;
                    _scrollBarH.ValueChanged += new EventHandler(ScrollBarH_ValueChanged);
                    base.WLControls.Add(_scrollBarH);
                }
                return _scrollBarH;
            }
        }

        private WLScrollBar ScrollBarV
        {
            get
            {
                if (_scrollBarV == null)
                {
                    _scrollBarV = new WLScrollBar(base.Owner, Orientation.Vertical);
                    _scrollBarV.ValueChanged += new EventHandler(ScrollBarV_ValueChanged);
                    _scrollBarV.SmallChange = ItemHeight;
                    _scrollBarV.LargeChange = ItemHeight * 10;
                    base.WLControls.Add(_scrollBarV);
                }
                return _scrollBarV;
            }
        }

        #endregion

        #region scrollbar visible

        /// <summary>
        /// 表示是否需要显示垂直滚动条
        /// </summary>
        private bool NeedToShowScrollBarV
        {
            get
            {
                ScrollBarShowResult vresult = ScrollBarShowResultV;
                if (vresult == ScrollBarShowResult.Show)
                    return true;
                else if (vresult == ScrollBarShowResult.Hide)
                    return false;
                else
                {
                    ScrollBarShowResult hresult = ScrollBarShowResultH;
                    if (hresult == ScrollBarShowResult.Hide)
                        return false;
                    else
                        return true;
                }
            }
        }

        /// <summary>
        /// 表示是否需要显示水平滚动条
        /// </summary>
        private bool NeedToShowScrollBarH
        {
            get
            {
                ScrollBarShowResult hresult = ScrollBarShowResultH;
                if (hresult == ScrollBarShowResult.Show)
                    return true;
                else if (hresult == ScrollBarShowResult.Hide)
                    return false;
                else
                {
                    ScrollBarShowResult vresult = ScrollBarShowResultV;
                    if (vresult == ScrollBarShowResult.Hide)
                        return false;
                    else
                        return true;
                }
            }
        }

        /// <summary>
        /// 判断垂直滚动条是否确实显示，确实不显示，或者受水平滚动条显示与否影响
        /// </summary>
        private ScrollBarShowResult ScrollBarShowResultV
        {
            get
            {
                if (ScrollBarVShowMode == ScrollBarShowMode.Always)
                {
                    return ScrollBarShowResult.Show;
                }
                else if (ScrollBarVShowMode == ScrollBarShowMode.Never)
                {
                    return ScrollBarShowResult.Hide;
                }
                else
                {
                    int height = NeededHeight;
                    if (height <= ContentSizeWithoutScrollBars.Height - XTheme.ScrollBarTheme.BestUndirectLen)
                    {
                        return ScrollBarShowResult.Hide;
                    }
                    else if (height > ContentSizeWithoutScrollBars.Height)
                    {
                        return ScrollBarShowResult.Show;
                    }
                    else
                    {
                        return ScrollBarShowResult.Unknown;
                    }
                }
            }
        }

        /// <summary>
        /// 判断水平滚动条是否确实显示，确实不显示，或者受垂直滚动条显示与否影响
        /// </summary>
        private ScrollBarShowResult ScrollBarShowResultH
        {
            get
            {
                if (ScrollBarHShowMode == ScrollBarShowMode.Always)
                {
                    return ScrollBarShowResult.Show;
                }
                else if (ScrollBarHShowMode == ScrollBarShowMode.Never)
                {
                    return ScrollBarShowResult.Hide;
                }
                else
                {
                    int width = MaxItemWidth;
                    if (width <= ContentSizeWithoutScrollBars.Width - XTheme.ScrollBarTheme.BestUndirectLen)
                    {
                        return ScrollBarShowResult.Hide;
                    }
                    else if (width > ContentSizeWithoutScrollBars.Width)
                    {
                        return ScrollBarShowResult.Show;
                    }
                    else
                    {
                        return ScrollBarShowResult.Unknown;
                    }
                }
            }
        }

        #endregion

        #region scrollbar rect

        /// <summary>
        /// 表示垂直滚动条的区域
        /// </summary>
        private Rectangle ScrollBarVRect
        {
            get
            {
                int x = Bounds.Right - XTheme.InnerPaddingWidth - XTheme.ScrollBarTheme.BestUndirectLen;
                int y = CtlTop + XTheme.InnerPaddingWidth;
                int w = XTheme.ScrollBarTheme.BestUndirectLen;
                int h = CtlSize.Height - XTheme.InnerPaddingWidth * 2;

                if (NeedToShowScrollBarH)
                    h -= XTheme.ScrollBarTheme.BestUndirectLen;

                if (!NeedToShowScrollBarV)
                {
                    w = h = 0;
                }
                return new Rectangle(x, y, w, h);
            }
        }

        /// <summary>
        /// 表示水平滚动条的区域
        /// </summary>
        private Rectangle ScrollBarHRect
        {
            get
            {
                int x = CtlLeft + XTheme.InnerPaddingWidth;
                int y = Bounds.Bottom - XTheme.InnerPaddingWidth - XTheme.ScrollBarTheme.BestUndirectLen;
                int w = CtlSize.Width - XTheme.InnerPaddingWidth * 2;
                int h = XTheme.ScrollBarTheme.BestUndirectLen;

                if (NeedToShowScrollBarV)
                    w -= XTheme.ScrollBarTheme.BestUndirectLen;

                if (!NeedToShowScrollBarH)
                {
                    w = h = 0;
                }
                return new Rectangle(x, y, w, h);
            }
        }

        #endregion

        #region scrollbar value

        /// <summary>
        /// 表示垂直滚动条的当前值
        /// </summary>
        private int ScrollBarVValue
        {
            get
            {
                if (!NeedToShowScrollBarV || !ScrollBarV.Enabled)
                    return 0;
                else
                    return ScrollBarV.Value;
            }
        }

        /// <summary>
        /// 表示水平滚动条的当前值
        /// </summary>
        private int ScrollBarHValue
        {
            get
            {
                if (!NeedToShowScrollBarH || !ScrollBarH.Enabled)
                    return 0;
                else
                    return ScrollBarH.Value;
            }
        }

        #endregion

        #region show index

        private bool showIndexReady = false;

        private int _firstShowIndex;
        private int _lastShowIndex;
        private int _firstItemYOffset;
        private int _lastItemCutHeight;
        private bool _firstItemPartlyShow;
        private bool _lastItemPartlyShow;

        /// <summary>
        /// 列表中能显示的最上边项的下标
        /// </summary>
        private int FirstShowIndex
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _firstShowIndex;
            }
        }

        /// <summary>
        /// 列表中能显示的最下边的项的下标
        /// </summary>
        private int LastShowIndex
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _lastShowIndex;
            }
        }

        /// <summary>
        /// 最上面的项有可能不能完整显示，而是向上偏一点，该值表示偏移量
        /// </summary>
        private int FirstItemYOffset
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _firstItemYOffset;
            }
        }

        private int LastItemCutHeight
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _lastItemCutHeight;
            }
        }

        private bool FirstItemPartlyShow
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _firstItemPartlyShow;
            }
        }

        private bool LastItemPartlyShow
        {
            get
            {
                if (!showIndexReady)
                {
                    CalculateFirstLastShowIndex();
                }
                return _lastItemPartlyShow;
            }
        }

        private int ShowIndexCount
        {
            get
            {
                int cnt = LastShowIndex - FirstShowIndex + 1;
                if (FirstItemPartlyShow)
                    cnt--;
                if (LastItemPartlyShow)
                    cnt--;
                if (cnt < 0)
                    cnt = 0;
                return cnt;
            }
        }

        #endregion

        #region high-light

        private int _hightLightIndex = -1;

        /// <summary>
        /// 获取或设置表示当前高亮的项的下标
        /// </summary>
        private int HighLightIndex
        {
            get
            {
                return _hightLightIndex;
            }
            set
            {
                if (!HighLightItemWhenMouseMove)
                    return;

                if (!IsIndexValid(value))
                    value = -1;

                if (_hightLightIndex != value)
                {
                    int oldIndex = _hightLightIndex;
                    _hightLightIndex = value;

                    // 仅仅需要刷新两个Items所在区域即可，不需要刷新整个列表
                    if (oldIndex != -1)
                        base.Invalidate(GetItemRectFromIndex(oldIndex));
                    if (_hightLightIndex != -1)
                        base.Invalidate(GetItemRectFromIndex(_hightLightIndex));
                }
            }
        }

        #endregion

        #region item width, height, content-rect

        private int _maxItemWidth = 0;
        private int _neededHeight = 0;

        /// <summary>
        /// 表示列表中最宽的项的宽度，应该在增加项，修改项，删除项这三个操作发生后重新计算该值
        /// </summary>
        private int MaxItemWidth
        {
            get
            {
                return _maxItemWidth;
            }
        }

        /// <summary>
        /// 列表中所有项的高度的总和
        /// </summary>
        private int NeededHeight
        {
            get
            {
                _neededHeight = this.Items.Count * this.ItemHeight;
                return _neededHeight;
            }
        }

        private Rectangle ContentRectangle
        {
            get
            {
                int x, y, w, h;
                x = base.CtlLeft + XTheme.InnerPaddingWidth;
                y = base.CtlTop + XTheme.InnerPaddingWidth;
                w = h = 0;
                Size size = ContentSizeWithoutScrollBars;
                w = size.Width;
                if (NeedToShowScrollBarV)
                {
                    w -= (XTheme.ScrollBarTheme.BestUndirectLen + XTheme.InnerPaddingWidth);
                    if (w < 0)
                        w = 0;
                }
                h = size.Height;
                if (NeedToShowScrollBarH)
                {
                    h -= (XTheme.ScrollBarTheme.BestUndirectLen + XTheme.InnerPaddingWidth);
                    if (h < 0)
                        h = 0;
                }
                return new Rectangle(x, y, w, h);
            }
        }

        private Size ContentSizeWithoutScrollBars
        {
            get
            {
                return new Size(CtlSize.Width - XTheme.InnerPaddingWidth * 2,
                    CtlSize.Height - XTheme.InnerPaddingWidth * 2);
            }
        }

        #endregion

        #endregion

        #region public properties

        private SmartEventsCollection _items;
        private SmartListBoxThemeBase _xtheme;
        private ScrollBarShowMode _scrollBarVShowMode = ScrollBarShowMode.Auto;
        private ScrollBarShowMode _scrollBarHShowMode = ScrollBarShowMode.Never;
        private int _itemHeight = DEFAULT_ITEM_HEIGHT;
        private int _selectedIndex = -1;
        private bool _topItemFullyShow = true;
        private int _topIndex;
        private ListSelectionMode _itemSelectionMode = ListSelectionMode.One;
        private bool _highLightItem = false;
        private ListDrawMode _itemDrawMode = ListDrawMode.AutoDraw;

        /// <summary>
        /// 获取一个集合，该集合表示列表中所有的项。可以通过该集合添加、删除项
        /// </summary>
        public SmartEventsCollection Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new SmartEventsCollection();
                    _items.CollectionChange += new GMCollectionChangeHandler(Items_CollectionChange);
                }
                return _items;
            }
        }

        /// <summary>
        /// 获取当前ListBox的主题
        /// </summary>
        public SmartListBoxThemeBase XTheme
        {
            get
            {
                if (_xtheme == null)
                {
                    _xtheme = new SmartListBoxThemeBase();
                    DoWhenThemeChanged();
                }
                return _xtheme;
            }
        }

        /// <summary>
        /// 获取或设置垂直滚动条的显示模式
        /// </summary>
        public ScrollBarShowMode ScrollBarVShowMode
        {
            get
            {
                return _scrollBarVShowMode;
            }
            set
            {
                if (_scrollBarVShowMode != value)
                {
                    _scrollBarVShowMode = value;

                    ResetScrollBarInfo();
                    ////
                    // to do here
                    ////
                }
            }
        }

        /// <summary>
        /// 获取或设置水平滚动条的显示模式
        /// </summary>
        public ScrollBarShowMode ScrollBarHShowMode
        {
            get
            {
                return _scrollBarHShowMode;
            }
            set
            {
                if (_scrollBarHShowMode != value)
                {
                    ScrollBarShowMode oldMode = _scrollBarHShowMode;
                    _scrollBarHShowMode = value;
                    if (oldMode == ScrollBarShowMode.Never)
                        RecheckMaxItemWidth();
                    ResetScrollBarInfo();
                    ////
                    // to do here
                    ////
                }
            }
        }

        /// <summary>
        /// 获取或设置列表中每项的高度
        /// </summary>
        public int ItemHeight
        {
            get
            {
                return _itemHeight;
            }
            set
            {
                if (_itemHeight != value)
                {
                    _itemHeight = value;

                    ScrollBarV.SmallChange = _itemHeight;
                    ScrollBarV.LargeChange = ItemHeight * 10;
                    ResetScrollBarInfo();
                    ////
                    // to do here
                    ////
                }
            }
        }

        /// <summary>
        /// 获取或设置列表中最新选中的项的下标
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (ItemSelectionMode == ListSelectionMode.None || _selectedIndex == value)
                    return;
                int oldIndex = _selectedIndex;
                _selectedIndex = value;
                newestItemIndexForKeySet = value;
                if (_selectedIndex == FirstShowIndex && FirstItemPartlyShow)
                {
                    ScrollBarV.ValueAdd(-FirstItemYOffset);
                }
                else if (_selectedIndex == LastShowIndex && LastItemPartlyShow)
                {
                    if (TopItemFullyShow)
                        ScrollBarV.ValueAdd(ItemHeight);
                    else
                        ScrollBarV.ValueAdd(LastItemCutHeight);
                }

                if (ItemSelectionMode == ListSelectionMode.One)
                {
                    if (oldIndex >= FirstShowIndex && oldIndex <= LastShowIndex)
                        base.Invalidate(GetItemRectFromIndex(oldIndex));
                    if (_selectedIndex >= FirstShowIndex && _selectedIndex <= LastShowIndex)
                        base.Invalidate(GetItemRectFromIndex(_selectedIndex));
                }
                else if (ItemSelectionMode == ListSelectionMode.Multiple)
                {
                    base.Invalidate(ContentRectangle);
                }

                OnSelectedIndexChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// 获取或设置是否完整显示列表中最上面的项
        /// </summary>
        public bool TopItemFullyShow
        {
            get
            {
                return _topItemFullyShow;
            }
            set
            {
                if (_topItemFullyShow != value)
                {
                    _topItemFullyShow = value;

                    ResetScrollBarInfo();
                }
            }
        }

        /// <summary>
        /// 获取或设置列表最顶部显示的项的下标
        /// </summary>
        public int TopIndex
        {
            get
            {
                return _topIndex;
            }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > Items.Count - 1)
                    value = Items.Count - 1;

                if (_topIndex != value && value >= 0 && value < Items.Count)
                {
                    _topIndex = value;
                    ScrollBarV.ValueAdd(_topIndex * ItemHeight - ScrollBarVValue);
                }
            }
        }

        /// <summary>
        /// 获取当前选中的项所代表的object，若无选中项则返回null
        /// </summary>
        public object SelectedValue
        {
            get
            {
                if (IsIndexValid(SelectedIndex))
                    return Items[SelectedIndex];
                else
                    return null;
            }
        }

        /// <summary>
        /// 获取或设置列表选择模式，单选还是允许多选
        /// </summary>
        public ListSelectionMode ItemSelectionMode
        {
            get
            {
                return _itemSelectionMode;
            }
            set
            {
                if (_itemSelectionMode == value)
                    return;
                _itemSelectionMode = value;
                ClearSelection(); // listSelectedIndexes.Clear();
                if (_itemSelectionMode == ListSelectionMode.None)
                {
                    //SelectedIndex = -1;
                    _selectedIndex = -1;
                    base.Invalidate(ContentRectangle);
                }
                else if (_itemSelectionMode == ListSelectionMode.One)
                {
                    base.Invalidate(ContentRectangle);
                }
                else if (_itemSelectionMode == ListSelectionMode.Multiple)
                {
                    if (IsIndexValid(SelectedIndex))
                        Add2Selection(SelectedIndex); // listSelectedIndexes.Add(SelectedIndex);
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值表示鼠标在列表中移动时，是否高亮鼠标所在项
        /// </summary>
        public bool HighLightItemWhenMouseMove
        {
            get
            {
                return _highLightItem;
            }
            set
            {
                if (_highLightItem != value)
                {
                    _highLightItem = value;
                    if (!_highLightItem)
                        HighLightIndex = -1;
                }
            }
        }

        /// <summary>
        /// 获取或设置列表项的绘制方式，自动绘制还是用户自行绘制
        /// </summary>
        public ListDrawMode ItemDrawMode
        {
            get
            {
                return _itemDrawMode;
            }
            set
            {
                if (_itemDrawMode != value)
                {
                    _itemDrawMode = value;
                    RecheckMaxItemWidth();
                    base.Invalidate(ContentRectangle);
                }
            }
        }

        /// <summary>
        /// 获取当前选中的项的总数目
        /// </summary>
        public int SelectionCount
        {
            get
            {
                return listSelectedIndexes.Count;
            }
        }

        /// <summary>
        /// 获取所有当前选中项的下标的数组
        /// </summary>
        public int[] SelectedIndexes
        {
            get
            {
                if (listSelectedIndexes.Count == 0)
                {
                    return new int[0] { };
                }
                else
                {
                    return listSelectedIndexes.ToArray();
                }
            }
        }

        /// <summary>
        /// 获取表示当前所有选中项的数组
        /// </summary>
        public object[] SelectedValues
        {
            get
            {
                if (listSelectedIndexes.Count == 0)
                {
                    return new object[0] { };
                }
                else
                {
                    List<object> list = new List<object>();
                    foreach (int i in listSelectedIndexes)
                    {
                        if (IsIndexValid(i))
                            list.Add(Items[i]);
                    }
                    return list.ToArray();
                }
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// 为ListBox设置新的主题
        /// </summary>
        /// <param name="xtheme"></param>
        public void SetNewTheme(SmartListBoxThemeBase xtheme)
        {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            DoWhenThemeChanged();
        }

        /// <summary>
        /// 以最少的滚动距离，将指定下标的项滚动到显示视图中
        /// </summary>
        /// <param name="index">指定的下标</param>
        public void ScrollToIndex(int index)
        {
            if (Items.Count == 0)
                return;

            if (index < 0)
                index = 0;
            if (index > Items.Count - 1)
                index = Items.Count - 1;

            int firstFully = FirstShowIndex;
            int lastFully = LastShowIndex;
            if (FirstItemPartlyShow)
                firstFully++;
            if (LastItemPartlyShow)
                lastFully--;

            if (index >= firstFully && index <= lastFully)
                return;

            if (index > lastFully)
            {
                ScrollBarV.ValueAdd(ItemHeight * (index - lastFully));
            }
            else
            {
                ScrollBarV.ValueAdd(-ItemHeight * (firstFully - index));
            }
        }

        /// <summary>
        /// 当前的Theme某个属性值发生变化后，该过程负责刷新。
        /// 用户直接修改Theme某个属性值后，应该调用该方法。
        /// </summary>
        public void RefleshTheme()
        {
            DoWhenThemeChanged();
        }

        /// <summary>
        /// 该过程返回显示指定的那么多行所需要的ListBox的高度，包括边界空白在内
        /// </summary>
        /// <param name="rows">表示要显示多少行</param>
        /// <returns>返回所需的高度</returns>
        public int GetBoxHeightForTheseRows(int rows)
        {
            if (rows > Items.Count)
                rows = Items.Count;
            if (rows < 1)
                rows = 1;
            return rows * ItemHeight + XTheme.InnerPaddingWidth * 2;
        }

        #endregion

        #region events

        // SelectedIndexChanged
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

        // SelectionChanged
        private static readonly object Event_SelectionChanged = new object();
        public event EventHandler SelectionChanged
        {
            add { base.Events.AddHandler(Event_SelectionChanged, value); }
            remove { base.Events.RemoveHandler(Event_SelectionChanged, value); }
        }
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[Event_SelectionChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // ItemClick
        private static readonly object Event_ItemClick = new object();
        public event ListItemClickHandler ItemClick
        {
            add { base.Events.AddHandler(Event_ItemClick, value); }
            remove { base.Events.RemoveHandler(Event_ItemClick, value); }
        }
        protected virtual void OnItemClick(int index)
        {
            ListItemClickHandler handler = (ListItemClickHandler)base.Events[Event_ItemClick];
            if (handler != null)
            {
                handler(this, index);
            }
        }

        // ItemDoubleClick
        private static readonly object Event_ItemDoubleClick = new object();
        public event ListItemClickHandler ItemDoubleClick
        {
            add { base.Events.AddHandler(Event_ItemDoubleClick, value); }
            remove { base.Events.RemoveHandler(Event_ItemDoubleClick, value); }
        }
        protected virtual void OnItemDoubleClick(int index)
        {
            ListItemClickHandler handler = (ListItemClickHandler)base.Events[Event_ItemDoubleClick];
            if (handler != null)
            {
                handler(this, index);
            }
        }

        // DrawItem
        private static readonly object Event_DrawItem = new object();
        public event ListItemPaintHandler DrawItem
        {
            add { base.Events.AddHandler(Event_DrawItem, value); }
            remove { base.Events.RemoveHandler(Event_DrawItem, value); }
        }
        protected virtual void OnDrawItem(ItemPaintEventArgs e)
        {
            ListItemPaintHandler handler = (ListItemPaintHandler)base.Events[Event_DrawItem];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // MeasureItem
        private static readonly object Event_MeasureItem = new object();
        public event MeasureItemEventHandler MeasureItem
        {
            add { base.Events.AddHandler(Event_MeasureItem, value); }
            remove { base.Events.RemoveHandler(Event_MeasureItem, value); }
        }
        protected virtual void OnMeasureItem(MeasureItemEventArgs e)
        {
            MeasureItemEventHandler handler = (MeasureItemEventHandler)base.Events[Event_MeasureItem];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region inner draw

        /// <summary>
        /// 判断指定下标项是否被选中
        /// </summary>        
        private bool IsSelectedIndex(int index)
        {
            if (ItemSelectionMode == ListSelectionMode.Multiple)
            {
                return listSelectedIndexes.Contains(index) || listSelectedIndexesTemp.Contains(index);
            }
            else
            {
                return index == SelectedIndex;
            }
        }

        private void RenderThisItem(Graphics g, Rectangle itemRect, int index, bool isSelected, bool isHighLight)
        {
            Color textColor, backColor, borderColor;

            if (isSelected)
            {
                backColor = XTheme.SelectedItemBackColor;
                textColor = XTheme.SelectedItemTextColor;
                borderColor = XTheme.SelectedItemBorderColor;
            }
            else if (isHighLight)
            {
                backColor = XTheme.HighLightBackColor;
                textColor = XTheme.HighLightTextColor;
                borderColor = XTheme.HighLightBorderColor;
            }
            else
            {
                textColor = XTheme.TextColor;
                backColor = Color.Transparent;
                borderColor = Color.Transparent;
            }

            if (isSelected || isHighLight)
            {
                BasicBlockPainter.RenderFlatBackground(
                    g,
                    itemRect,
                    backColor,
                    ButtonBorderType.Rectangle);
                BasicBlockPainter.RenderBorder(
                    g,
                    itemRect,
                    borderColor,
                    ButtonBorderType.Rectangle);
                //GlassPainter.RenderRectangleGlass(
                //    g,
                //    r,
                //    0,
                //    RoundStyle.None,
                //    GlassPosition.Top,
                //    90.0001f,
                //    0.5f,
                //    Color.White,
                //    120,20);
            }

            TextRenderer.DrawText(
                g,
                Items[index].ToString(),
                XTheme.TextFont,
                itemRect,
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter
                | TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.NoPrefix);
        }

        private void RenderItems(Graphics g, Rectangle clipRect)
        {
            Rectangle contentRect = ContentRectangle;

            int x = contentRect.Left - ScrollBarHValue;
            int y = contentRect.Top - FirstItemYOffset;
            int h = ItemHeight;
            int w = contentRect.Width + ScrollBarHValue;

            Rectangle r = new Rectangle(x, y, w, h);
            bool isSelected;
            for (int i = FirstShowIndex; i <= LastShowIndex; i++)
            {
                if (clipRect.IntersectsWith(r))
                {
                    isSelected = IsSelectedIndex(i);
                    if (ItemDrawMode == ListDrawMode.AutoDraw)
                    {
                        RenderThisItem(g, r, i, isSelected, i == HighLightIndex);
                    }
                    else if (ItemDrawMode == ListDrawMode.UserDraw)
                    {
                        OnDrawItem(new ItemPaintEventArgs(g, r, i, isSelected, i == HighLightIndex));
                    }
                }
                r.Offset(0, h);
            }
        }

        private void PaintBackgroundAndBorder(Graphics g, Rectangle clipRect)
        {
            // background
            BasicBlockPainter.RenderFlatBackground(g, Bounds, XTheme.BackColor,
                ButtonBorderType.Rectangle);

            // border
            if (XTheme.DrawBorder)
            {
                BasicBlockPainter.RenderBorder(g, Bounds, XTheme.BorderColor,
                    ButtonBorderType.Rectangle);
            }

            // content background
            //BasicBlockPainter.RenderFlatBackground(g, ContentRectangle, Color.LightSalmon, 
            //    ButtonBorderType.Rectangle);
        }

        private void PaintListBoxItems(Graphics g, Rectangle clipRect)
        {
            if (Items.Count == 0)
                return;

            Rectangle cr = ContentRectangle;
            if (clipRect.IntersectsWith(cr))
            {
                using (NewClipGraphics ng = new NewClipGraphics(g, new Region(cr), true))
                {
                    RenderItems(g, clipRect);
                }
            }
        }

        #endregion

        #region override base method

        #region Paint

        protected override void OnPaintBackground(Graphics g, Rectangle clipRect)
        {
            base.OnPaintBackground(g, clipRect);
            PaintBackgroundAndBorder(g, clipRect);
        }

        protected override void OnPaintContent(Graphics g, Rectangle clipRect)
        {
            base.OnPaintContent(g, clipRect);
            PaintListBoxItems(g, clipRect);
        }

        #endregion

        #region mouse operation

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (whereMouseDown == MouseDownLocation.NonLeftKeyDown)
                return;

            DoWhenMouseMove(e.Location);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                mouseDownClicks = e.Clicks;
                if (e.Clicks == 1)
                {
                    pointMouseDown = e.Location;
                    whereMouseDown = CheckLocation(e.Location);

                    mouseDownItemIndex = GetItemIndexFromPoint(e.Location);
                    if (TopItemFullyShow)
                    {
                        if (mouseDownItemIndex == LastShowIndex && LastItemPartlyShow)
                            isMouseDownInLastPartlyShowIndex = true;
                        else
                            isMouseDownInLastPartlyShowIndex = false;
                    }

                    if (whereMouseDown == MouseDownLocation.ItemArea)
                    {
                        if (ItemSelectionMode == ListSelectionMode.One)
                        {
                            DoWhenLeftButtonDownInItemsArea_One(e.Location);
                        }
                        else if (ItemSelectionMode == ListSelectionMode.Multiple)
                        {
                            lastMouseDownMoveItemIndex = GetItemIndexFromPoint(e.Location);
                            DoWhenLeftButtonDownInItemsArea_Multi(e.Location);
                        }
                    }
                }
            }
            else
            {
                whereMouseDown = MouseDownLocation.NonLeftKeyDown;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                if (whereMouseDown == MouseDownLocation.ItemArea ||
                    (mouseDownItemIndex != -1 && mouseDownClicks == 2))
                {
                    DoWhenLeftButtonUpInItemsArea(e.Location, mouseDownClicks);
                }
                whereMouseDown = MouseDownLocation.NotDown;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            HighLightIndex = -1;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (e.Delta != 0 && ScrollBarV.Visible && ScrollBarV.Enabled)
            {
                int amount = ((e.Delta < 0) ? ItemHeight : -ItemHeight);
                ScrollBarV.ValueAdd(amount);
            }
        }

        #endregion

        #region key operation

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            isShiftKeyDown = e.Shift;
            isControlKeyDown = e.Control;

            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.Left:
                    DealUpArrowKey();
                    break;
                case Keys.Down:
                case Keys.Right:
                    DealDownArrowKey();
                    break;
                case Keys.PageDown:
                    DealPageDownKey();
                    break;
                case Keys.PageUp:
                    DealPageUpKey();
                    break;
                case Keys.Home:
                    DealHomeKey();
                    break;
                case Keys.End:
                    DealEndKey();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if ((e.KeyData & Keys.ControlKey) == Keys.ControlKey)
                isControlKeyDown = false;
            if ((e.KeyData & Keys.ShiftKey) == Keys.ShiftKey)
                isShiftKeyDown = false;

        }

        #endregion

        #region size, location, enable

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            DoWhenSizeChanged();
        }

        #endregion

        #endregion

        #region debug

        public string GetDebugInfo()
        {
            string s = "Items.Count: " + Items.Count.ToString() + "\r\n" +
                "ItemHeight: " + ItemHeight.ToString() + "\r\n" +
                "needed-height: " + NeededHeight.ToString() + "\r\n" +
                "content-rect:" + ContentRectangle.ToString() + "\r\n" +
                "v-scroll-maximum: " + ScrollBarV.Maximum.ToString() + "\r\n" +
                "v-scroll-value: " + ScrollBarV.Value.ToString() + "\r\n" +
                "first_show-index: " + FirstShowIndex.ToString() + "\r\n" +
                "last-show-index: " + LastShowIndex.ToString() + "\r\n";
            return s;
        }

        #endregion

        #region Inner enum, class

        private enum ScrollBarShowResult
        {
            /// <summary>
            /// 滚动条必须要显示
            /// </summary>
            Show,

            /// <summary>
            /// 滚动条不需要显示
            /// </summary>
            Hide,

            /// <summary>
            /// 显示与否受到另一个方向的滚动条的影响
            /// </summary>
            Unknown
        }

        private enum MouseDownLocation
        {
            /// <summary>
            /// 鼠标键未按下
            /// </summary>
            NotDown,

            /// <summary>
            /// 鼠标左键在边框上或界面空白区域按下
            /// </summary>
            NoWhere,

            /// <summary>
            /// 鼠标左键在垂直或水平滚动条位置按下
            /// </summary>
            ScrollBars,

            /// <summary>
            /// 鼠标左键在列表项区域按下
            /// </summary>
            ItemArea,

            /// <summary>
            /// 非左键，即鼠标右键或中键被按下
            /// </summary>
            NonLeftKeyDown
        }

        #endregion
    }

    #region event class

    public class ItemPaintEventArgs : PaintEventArgs
    {
        Rectangle itemRect;
        int index;
        bool isSelected;
        bool isHighLight;

        public ItemPaintEventArgs(Graphics g, Rectangle itemRect, int index, bool selected, bool highLight)
            : base(g, itemRect)
        {
            this.itemRect = itemRect;
            this.index = index;
            this.isHighLight = highLight;
            this.isSelected = selected;
        }

        /// <summary>
        /// 获取要绘制的列表项的下标
        /// </summary>
        public int Index
        {
            get { return this.index; }
        }

        /// <summary>
        /// 获取要绘制的列表项的区域
        /// </summary>
        public Rectangle ItemRect
        {
            get { return itemRect; }
        }

        /// <summary>
        /// 获取一个值，指示该项是否被选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
        }

        /// <summary>
        /// 获取一个值，指示该项是否处于高亮状态
        /// </summary>
        public bool IsHighLight
        {
            get { return isHighLight; }
        }
    }

    public delegate void ListItemClickHandler(object sender, int index);

    public delegate void ListItemPaintHandler(object sender, ItemPaintEventArgs e);

    #endregion
}
