using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SmartWinControls.Common;
using SmartWinControls.SmartControls.ListBox;
using SmartWinControls.SmartControls.Button;

namespace SmartWinControls.SmartControls.ComboBox
{
    public class SmartComboBoxThemeBase
    {
        public Padding TextBoxMarginEditable { get; set; }
        public Padding TextBoxMarginNotEditable { get; set; }
        public Padding RightButtonMargin { get; set; }
        public int RightButtonWidth { get; set; }

        public int BorderRoundedRadius { get; set; }
        public Color BackColorNormal { get; set; }
        public Color BackColorHover { get; set; }
        public Color BorderColorNormal { get; set; }
        public Color BorderColorHover { get; set; }

        public BackgroundStyle HowBackgroundRender { get; set; }

        // background linear gradient colors
        public Color BackColorLG1Normal { get; set; }
        public Color BackColorLG2Normal { get; set; }
        public Color BackColorLG1Hover { get; set; }
        public Color BackColorLG2Hover { get; set; }

        public Font ComboTextFont { get; set; }
        public Color ComboTextColor { get; set; }

        public bool DrawBorder { get; set; }

        public SmartButtonThemeBase RightButtonTheme { get; set; }
        public SmartListBoxThemeBase ListBoxTheme { get; set; }

        /// <summary>
        /// 获取或设置下拉框的边框颜色
        /// </summary>
        public Color DropDownBorderColor { get; set; }
        public Color DropDownBackColor { get; set; }
        public Color ResizeGridColor { get; set; }

        public SmartComboBoxThemeBase()
        {
            TextBoxMarginEditable = new Padding(4);
            TextBoxMarginNotEditable = new Padding(2, 1, 2, 1);
            RightButtonMargin = Padding.Empty;
            RightButtonWidth = 21;

            HowBackgroundRender = BackgroundStyle.Flat;

            BorderRoundedRadius = 0;
            BackColorNormal = BackColorHover = Color.White;
            BorderColorNormal = Color.FromArgb(174, 174, 174);
            BorderColorHover = Color.FromArgb(123, 123, 123);

            ComboTextFont = new Font("微软雅黑", 9.0f);
            ComboTextColor = Color.Black;

            DrawBorder = true;

            RightButtonTheme = new ThemeButtonDefaultComboBox();
            ListBoxTheme = new SmartListBoxThemeBase();

            DropDownBorderColor = Color.DarkGray;
            DropDownBackColor = Color.FromArgb(233, 233, 233);
            ResizeGridColor = Color.FromArgb(125, 125, 125);
        }

        private class ThemeButtonDefaultComboBox : SmartButtonThemeBase
        {
            public ThemeButtonDefaultComboBox()
            {
                base.RoundedRadius = 0;
                base.ColorTable = GetColor();
            }

            private ButtonColorTable GetColor()
            {
                ButtonColorTable table = new ButtonColorTable();

                table.ForeColorNormal = Color.FromArgb(122, 122, 122);
                table.ForeColorHover = Color.FromArgb(101, 101, 101);
                table.ForeColorPressed = Color.FromArgb(51, 51, 51);

                table.BorderColorNormal = Color.FromArgb(174, 174, 174);
                table.BorderColorHover = Color.FromArgb(124, 124, 124);
                table.BorderColorPressed = Color.FromArgb(108, 108, 108);

                return table;
            }
        }
    }
}
