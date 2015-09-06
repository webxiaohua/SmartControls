using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartWinControls.SmartControls.CheckBox
{
    public class SmartCheckBoxThemeBase : IDisposable
    {
        public int InnerPaddingWidth { get; set; }

        /// <summary>
        /// 获取或设置Check标记的外部方块的宽度，宽度与高度是相等的
        /// </summary>
        public int CheckRectWidth { get; set; }

        /// <summary>
        /// 获取或设置当CheckState==Indeterminate时，内部Rect的缩进量，为正值
        /// </summary>
        public int InnerRectInflate { get; set; }

        public int SpaceBetweenCheckMarkAndText { get; set; }

        public Color CheckRectColor { get; set; }
        public Color CheckRectColorDisabled { get; set; }
        public Color CheckFlagColor { get; set; }
        public Color CheckFlagColorDisabled { get; set; }

        public Color CheckRectBackColorNormal { get; set; }
        public Color CheckRectBackColorHighLight { get; set; }
        public Color CheckRectBackColorPressed { get; set; }
        public Color CheckRectBackColorDisabled { get; set; }

        /// <summary>
        /// 获取或设置当鼠标进入时，是否高亮Check外边框
        /// </summary>
        public bool HighLightCheckRect { get; set; }

        public Font TextFont { get; set; }
        public Color TextColor { get; set; }
        public Color TextColorDisabled { get; set; }

        public SmartCheckBoxThemeBase()
        {
            InnerPaddingWidth = 2;
            CheckRectWidth = 14;
            InnerRectInflate = 3;
            SpaceBetweenCheckMarkAndText = 3;
            HighLightCheckRect = true;

            CheckRectColor = Color.FromArgb(213, 176, 72);
            CheckRectColorDisabled = Color.Gray;
            CheckFlagColor = Color.FromArgb(93, 151, 2);
            CheckFlagColorDisabled = Color.Gray;

            CheckRectBackColorNormal = CheckRectBackColorHighLight = Color.FromArgb(246, 239, 219);
            CheckRectBackColorPressed = Color.FromArgb(239, 226, 188);

            TextFont = new Font("微软雅黑", 9.0f);
            TextColor = Color.Black;
            TextColorDisabled = Color.Gray;
        }

        #region IDisposable

        public void Dispose()
        {
            if (TextFont != null && !TextFont.IsSystemFont)
                TextFont.Dispose();
        }

        #endregion
    }
}
