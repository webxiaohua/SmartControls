using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SmartWinControls.Theme.ScrollBar;
using SmartWinControls.SmartControls.ScrollBar;

namespace SmartWinControls.SmartControls.ListBox
{
    public class SmartListBoxThemeBase
    {
        public SmartScrollBarThemeBase ScrollBarTheme { get; set; }
        public int InnerPaddingWidth { get; set; }

        public Color BackColor { get; set; }
        public Color BorderColor { get; set; }
        public Color InnerBorderColor { get; set; }

        public bool DrawBorder { get; set; }
        public bool DrawInnerBorder { get; set; }

        public Font TextFont { get; set; }
        public Color TextColor { get; set; }

        public Color SelectedItemBackColor { get; set; }
        public Color SelectedItemTextColor { get; set; }
        public Color SelectedItemBorderColor { get; set; }

        public Color HighLightBackColor { get; set; }
        public Color HighLightBorderColor { get; set; }
        public Color HighLightTextColor { get; set; }

        public SmartListBoxThemeBase()
        {
            InnerPaddingWidth = 2;
            ScrollBarTheme = new ThemeScrollbarVS2013();

            BackColor = Color.White;
            BorderColor = Color.FromArgb(130, 135, 144);

            DrawBorder = true;
            DrawInnerBorder = false;

            TextFont = new Font("微软雅黑", 9.0f);
            TextColor = Color.Black;

            SelectedItemBackColor = Color.FromArgb(51, 153, 255);
            SelectedItemBorderColor = SelectedItemBackColor;
            SelectedItemTextColor = Color.White;

            HighLightBackColor = Color.FromArgb(90, SelectedItemBackColor);
            HighLightBorderColor = Color.Transparent;//HighLightBackColor;
            HighLightTextColor = Color.Black;
        }
    }
}
