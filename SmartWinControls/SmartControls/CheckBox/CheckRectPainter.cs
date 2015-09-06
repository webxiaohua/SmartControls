using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using SmartWinControls.Common;
using SmartWinControls.Functions;
using System.Drawing.Drawing2D;

namespace SmartWinControls.SmartControls.CheckBox
{
    public class CheckRectPainter
    {
        public static void RenderCheckRect(Graphics g, Rectangle rect, SmartCheckBoxThemeBase xtheme,
            bool enable, CheckState checkState, SmartButtonState state)
        {
            if (rect.Width < 1 || rect.Height < 1)
                return;

            // get back-color
            Color backColor;
            if (!enable)
            {
                backColor = xtheme.CheckRectBackColorDisabled;
            }
            else
            {
                switch (state)
                {
                    case SmartButtonState.Hover:
                        backColor = xtheme.CheckRectBackColorHighLight;
                        break;
                    case SmartButtonState.Pressed:
                        backColor = xtheme.CheckRectBackColorPressed;
                        break;
                    default:
                        backColor = xtheme.CheckRectBackColorNormal;
                        break;
                }
            }

            // get outter-rect-color
            Color rectColor = enable ? xtheme.CheckRectColor : xtheme.CheckRectColorDisabled;

            // get inner-flag-color
            Color flagColor = enable ? xtheme.CheckFlagColor : xtheme.CheckFlagColorDisabled;

            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
            {
                // draw check rect backcolor
                using (SolidBrush sb = new SolidBrush(backColor))
                {
                    g.FillRectangle(sb, rect);
                }

                // draw check rect border
                using (Pen p = new Pen(rectColor))
                {
                    rect.Height--;
                    rect.Width--;
                    g.DrawRectangle(p, rect);

                    // high light
                    if (xtheme.HighLightCheckRect && state == SmartButtonState.Hover && enable)
                    {
                        using (Pen p2 = new Pen(Color.FromArgb(40, rectColor)))
                        {
                            p2.Width = 3;
                            p2.Alignment = PenAlignment.Center;
                            g.DrawRectangle(p2, rect);
                        }
                    }

                    rect.Height++;
                    rect.Width++;
                }

                // draw Indeterminate flag
                if (checkState == CheckState.Indeterminate)
                {
                    rect.Inflate(-xtheme.InnerRectInflate, -xtheme.InnerRectInflate);
                    using (SolidBrush sb = new SolidBrush(flagColor))
                    {
                        g.FillRectangle(sb, rect);
                    }
                }
            }

            // draw check flag
            if (checkState == CheckState.Checked)
            {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
                {
                    rect.Inflate(-2, -2);
                    rect.Height -= 3;
                    rect.Width--;
                    PointF p1 = new PointF(rect.X, rect.Y + rect.Height / 2.0f);
                    PointF p2 = new PointF(rect.X + rect.Width / 3.0f, rect.Bottom);
                    PointF p3 = new PointF(rect.Right + 1, rect.Y - 1);
                    using (Pen p = new Pen(flagColor))
                    {
                        p.Width = 1.6f;
                        p.StartCap = LineCap.Round;
                        p.EndCap = LineCap.Round;
                        g.DrawLines(p, new PointF[] { p1, p2, p3 });

                        p2.Y += 1.8f;
                        g.DrawLines(p, new PointF[] { p1, p2, p3 });
                    }
                }
            }
        }
    }
}
