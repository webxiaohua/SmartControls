using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SmartWinControls.Functions
{
    public class NewSmoothModeGraphics : IDisposable
    {
        SmoothingMode _oldMode;
        Graphics _graphics;

        public NewSmoothModeGraphics(Graphics g, SmoothingMode newMode)
        {
            _oldMode = g.SmoothingMode;
            g.SmoothingMode = newMode;
            _graphics = g;
        }

        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }
    }
}
