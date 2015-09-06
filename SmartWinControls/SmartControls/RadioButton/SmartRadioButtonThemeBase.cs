using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SmartWinControls.SmartControls.RadioButton
{
    public class SmartRadioButtonThemeBase: IDisposable
    {
        /// <summary>
        /// 获取或设置内部四周边界空白
        /// </summary>
        public int InnerPaddingWidth { get; set; }

        /// <summary>
        /// 获取或设置Radio标记的大小，其宽高是相等的
        /// </summary>
        public int RadioMarkWidth { get; set; }

        /// <summary>
        /// 获取或设置内部标记点相对于外圆的缩进量，要为正值
        /// </summary>
        public int InnerSpotInflate { get; set; }

        public Color OutterCircleColor { get; set; }
        public Color OutterCircleColorDisabled { get; set; }
        public Color InnerSpotColor { get; set; }
        public Color InnerSpotColorDisabled { get; set; }

        public Color RadioMarkBackColorNormal { get; set; }
        public Color RadioMarkBackColorHighLight { get; set; }
        public Color RadioMarkBackColorPressed { get; set; }
        public Color RadioMarkBackColorDisabled { get; set; }

        /// <summary>
        /// 获取或设置当鼠标进入时，是否高亮外圆圈
        /// </summary>
        public bool HighLightOutterCircle { get; set; }
        public int OutterCircleHighLightAlpha { get; set; }

        /// <summary>
        /// 获取或设置是否在内圈上画亮区
        /// </summary>
        public bool ShowGlassOnInnerSpot { get; set; }

        public Color TextColor { get; set; }
        public Color TextColorDisabled { get; set; }
        public Font TextFont { get; set; }

        public int SpaceBetweenMarkAndText { get; set; }

        public SmartRadioButtonThemeBase()
        {
            InnerPaddingWidth = 2;
            RadioMarkWidth = 13;
            InnerSpotInflate = 4;

            OutterCircleColor = Color.FromArgb(134, 198, 132);
            InnerSpotColor = Color.FromArgb(72, 150, 70);
            OutterCircleColorDisabled = Color.Gray;
            InnerSpotColorDisabled = Color.Gray;

            HighLightOutterCircle = true;
            OutterCircleHighLightAlpha = 60;
            ShowGlassOnInnerSpot = false;

            RadioMarkBackColorNormal = Color.Transparent;
            RadioMarkBackColorHighLight = Color.Transparent;
            RadioMarkBackColorPressed = Color.FromArgb(220, 238, 219);
            RadioMarkBackColorDisabled = Color.Transparent;

            TextColor = Color.Black;
            TextColorDisabled = Color.Gray;
            TextFont = new Font("微软雅黑", 9.0f);

            SpaceBetweenMarkAndText = 3;
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
