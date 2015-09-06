using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SmartWinControls.Common;
using SmartWinControls.Functions;
using System.Drawing.Drawing2D;
using SmartWinControls.Painter;

namespace SmartWinControls.SmartControls.RadioButton
{
    public class RadioMarkPainter
    {
        public static void RenderRadioMark(Graphics g, Rectangle rect,
            SmartRadioButtonThemeBase xtheme, bool enable, bool selected, SmartButtonState state)
        {
            if (rect.Width < 1 || rect.Height < 1)
                return;

            // get back-color
            Color backColor;
            if (!enable)
            {
                backColor = xtheme.RadioMarkBackColorDisabled;
            }
            else
            {
                switch (state)
                {
                    case SmartButtonState.Hover:
                        backColor = xtheme.RadioMarkBackColorHighLight;
                        break;
                    case SmartButtonState.Pressed:
                        backColor = xtheme.RadioMarkBackColorPressed;
                        break;
                    default:
                        backColor = xtheme.RadioMarkBackColorNormal;
                        break;
                }
            }

            // get outter-circle-color
            Color circleColor = enable ? xtheme.OutterCircleColor : xtheme.OutterCircleColorDisabled;

            // get inner-spot-color
            Color innerColor = enable ? xtheme.InnerSpotColor : xtheme.InnerSpotColorDisabled;

            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
            {
                // draw background
                Rectangle r1 = rect;
                r1.Width--;
                r1.Height--;
                if (r1.Width > 0 && r1.Height > 0)
                {
                    using (SolidBrush sb = new SolidBrush(backColor))
                    {
                        g.FillEllipse(sb, r1);
                    }
                }

                // draw outter circle
                using (Pen p = new Pen(circleColor))
                {
                    g.DrawEllipse(p, rect);
                }

                // draw high-light-outter-circle
                if (xtheme.HighLightOutterCircle && state == SmartButtonState.Hover)
                {
                    using (Pen p = new Pen(Color.FromArgb(xtheme.OutterCircleHighLightAlpha, circleColor)))
                    {
                        p.Width = 3;
                        p.Alignment = PenAlignment.Center;
                        g.DrawEllipse(p, rect);
                    }
                }

                // draw inner spot
                if (selected)
                {
                    rect.Inflate(-xtheme.InnerSpotInflate, -xtheme.InnerSpotInflate);
                    using (SolidBrush sb = new SolidBrush(innerColor))
                    {
                        g.FillEllipse(sb, rect);
                    }

                    // draw glass on inner spot
                    if (xtheme.ShowGlassOnInnerSpot)
                    {
                        GlassPainter.RenderEllipseGlass(g, rect, GlassPosition.Top, 0.2f, Color.White, 160, 20);
                    }
                }
            }
        }
    }
}
