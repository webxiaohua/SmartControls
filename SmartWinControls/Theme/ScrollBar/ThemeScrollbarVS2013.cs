using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.ScrollBar;
using System.Drawing;
using SmartWinControls.Common;
using SmartWinControls.Functions;

namespace SmartWinControls.Theme.ScrollBar
{
    public class ThemeScrollbarVS2013 : SmartScrollBarThemeBase
    {
        public ThemeScrollbarVS2013()
        {
            DrawLinesInMiddleButton = false;
            ShowSideButtons = true;
            DrawBackground = true;
            DrawBorder = false;
            DrawInnerBorder = false;

            BackColor = Color.FromArgb(232, 232, 232);
            InnerPaddingWidth = 0;
            MiddleButtonOutterSpace1 = 0;
            MiddleButtonOutterSpace2 = 4;
            BestUndirectLen = 17;
            SideButtonLength = 17;

            SideButtonTheme.ColorTable = SideBtnColor();
            MdlButtonTheme.ColorTable = MdlBtnColor();

            SideButtonForePathSize = new Size(9, 9);
            SideButtonForePathGetter = new ButtonForePathGetter(
                GraphicsPathHelper.Create9x5DownTriangleFlag);
        }

        private ButtonColorTable SideBtnColor()
        {
            ButtonColorTable table = new ButtonColorTable();

            table.ForeColorNormal = Color.FromArgb(134, 137, 153);
            table.ForeColorHover = Color.FromArgb(28, 151, 234);
            table.ForeColorPressed = Color.FromArgb(0, 122, 204);
            table.ForeColorDisabled = Color.LightGray;

            table.BackColorNormal = table.BackColorHover =
                table.BackColorPressed = table.BackColorDisabled = Color.FromArgb(232, 232, 232);

            return table;
        }

        private ButtonColorTable MdlBtnColor()
        {
            ButtonColorTable table = new ButtonColorTable();

            table.BackColorNormal = Color.FromArgb(208, 209, 215);
            table.BackColorHover = Color.FromArgb(136, 136, 136);
            table.BackColorPressed = Color.FromArgb(106, 106, 106);

            return table;
        }
    }
}
