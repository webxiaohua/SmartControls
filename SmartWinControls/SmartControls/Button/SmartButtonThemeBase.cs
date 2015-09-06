using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SmartWinControls.Common;
using SmartWinControls.Functions;

namespace SmartWinControls.SmartControls.Button
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-28
    /// @Desc：按钮皮肤基类
    /// </summary>
    public class SmartButtonThemeBase:IDisposable
    {
        public Padding InnerPadding { get; set; }
        public RoundStyle RoundedStyle { get; set; }
        public int RoundedRadius { get; set; }

        public ButtonColorTable ColorTable { get; set; }
        public Font TextFont { get; set; }

        public SmartButtonThemeBase()
        {
            InnerPadding = new Padding(16, 6, 16, 6);
            RoundedStyle = RoundStyle.All;
            RoundedRadius = 4;
            TextFont = new Font("微软雅黑", 9.0f);
            ColorTable = GetColor();
        }

        private ButtonColorTable GetColor()
        {
            ButtonColorTable table = new ButtonColorTable();

            table.ForeColorNormal = table.ForeColorHover = table.ForeColorPressed = Color.Black;
            table.ForeColorDisabled = Color.Gray;

            table.BorderColorNormal = table.BorderColorHover = table.BorderColorPressed =
                table.BorderColorDisabled = Color.FromArgb(178, 183, 189);

            Color c = Color.FromArgb(231, 236, 242);
            table.BackColorNormal = c;
            table.BackColorHover = ColorHelper.GetLighterColor(c, 40);
            table.BackColorPressed = ColorHelper.GetDarkerColor(c, 10);
            table.BackColorDisabled = ColorHelper.GetLighterColor(Color.Gray, 90);

            return table;
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
