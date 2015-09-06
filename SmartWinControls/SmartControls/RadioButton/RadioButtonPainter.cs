using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SmartWinControls.Common;
using System.Windows.Forms;
using SmartWinControls.Painter;

namespace SmartWinControls.SmartControls.RadioButton
{
    public class RadioButtonPainter
    {
        public static void GetCheckRectAndTextRect(Rectangle rect, int paddingWidth, int checkWidth, int diffSpace,
            CheckMarkAlignment checkAlign, string text, Font font, out Rectangle rectCheck, out Rectangle rectText)
        {
            rectCheck = Rectangle.Empty;
            rectText = Rectangle.Empty;
            Size textSize = System.Windows.Forms.TextRenderer.MeasureText(text, font);

            if (checkAlign == CheckMarkAlignment.Left)
            {
                rectCheck = new Rectangle(rect.Left + paddingWidth,
                    rect.Top + (rect.Height - checkWidth) / 2,
                    checkWidth, checkWidth);
                rectText = new Rectangle(rectCheck.Right + diffSpace,
                    rect.Top + (rect.Height - textSize.Height) / 2 + 1, // 这里加1是为了使radio-mark 与 text 看起来更协调
                    textSize.Width, textSize.Height);
                if (rectCheck.Right > rect.Right)
                {
                    int w = rectCheck.Width - (rectCheck.Right - rect.Right);
                    rectCheck.Width = w < 0 ? 0 : w;
                }
                if (rectText.Right > rect.Right)
                {
                    int w = rectText.Width - (rectText.Right - rect.Right);
                    rectText.Width = w < 0 ? 0 : w;
                }
            }
            else
            {
                rectCheck = new Rectangle(rect.Right - paddingWidth - checkWidth,
                    rect.Top + (rect.Height - checkWidth) / 2,
                    checkWidth, checkWidth);
                rectText = new Rectangle(rectCheck.Left - diffSpace - textSize.Width,
                    rect.Top + (rect.Height - textSize.Height) / 2 + 1,
                    textSize.Width, textSize.Height);
                if (rectCheck.Left < rect.Left)
                {
                    int w = rectCheck.Width - (rect.Left - rectCheck.Left);
                    rectCheck.X = rect.Left;
                    rectCheck.Width = w < 0 ? 0 : w;
                }
                if (rectText.Left < rect.Left)
                {
                    int w = rectText.Width - (rect.Left - rectText.Left);
                    rectText.X = rect.Left;
                    rectText.Width = w < 0 ? 0 : w;
                }
            }
        }

        public static void RenderRadioButton(Graphics g, Rectangle rect, SmartRadioButtonThemeBase xtheme,
            bool enable, bool selected, SmartButtonState state, string text,
            CheckMarkAlignment markAlign, bool drawFocus)
        {
            Rectangle rectMark, rectText;

            GetCheckRectAndTextRect(
                rect,
                xtheme.InnerPaddingWidth,
                xtheme.RadioMarkWidth,
                xtheme.SpaceBetweenMarkAndText,
                markAlign,
                text,
                xtheme.TextFont,
                out rectMark,
                out rectText);

            RadioMarkPainter.RenderRadioMark(
                g,
                rectMark,
                xtheme,
                enable,
                selected,
                state);
            System.Windows.Forms.TextRenderer.DrawText(
                g,
                text,
                xtheme.TextFont,
                rectText,
                enable ? xtheme.TextColor : xtheme.TextColorDisabled,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            if (drawFocus)
                BasicBlockPainter.RenderFocusRect(g, rectText, 0);

            //rectMark.Width--;
            //rectMark.Height--;
            //g.DrawRectangle(Pens.Red, rectMark);

            //rectText.Width--;
            //rectText.Height--;
            //g.DrawRectangle(Pens.Blue, rectText);

            //rect.Width--;
            //rect.Height--;
            //g.DrawRectangle(Pens.Black, rect);

        }
    }
}
