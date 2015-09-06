using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.SmartControls.Button;
using System.Windows.Forms;
using SmartWinControls.Common;
using System.Drawing;

namespace SmartWinControls.SmartControls.CheckBox
{
    public class WLCheckBox : WLButtonBase
    {
        #region private var

        private static readonly object EVENT_CHECKSTATECHANGED = new object();

        #endregion

        #region constructors

        public WLCheckBox(Control owner)
            : base(owner)
        {

        }

        #endregion

        #region events

        public event EventHandler CheckStateChanged
        {
            add { base.Events.AddHandler(EVENT_CHECKSTATECHANGED, value); }
            remove { base.Events.RemoveHandler(EVENT_CHECKSTATECHANGED, value); }
        }

        protected virtual void OnCheckStateChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[EVENT_CHECKSTATECHANGED];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region public properties

        private CheckState _checkState = CheckState.Unchecked;
        private bool _threeState = false;
        private CheckMarkAlignment _radioMarkAlign = CheckMarkAlignment.Left;
        private SmartCheckBoxThemeBase _xtheme;

        public SmartCheckBoxThemeBase XTheme
        {
            get
            {
                if (_xtheme == null)
                    _xtheme = new SmartCheckBoxThemeBase();
                return _xtheme;
            }
        }

        public CheckMarkAlignment CheckRectAlign
        {
            get
            {
                return _radioMarkAlign;
            }
            set
            {
                if (_radioMarkAlign != value)
                {
                    _radioMarkAlign = value;
                    base.Invalidate();
                }
            }
        }

        public CheckState CheckState
        {
            get
            {
                return _checkState;
            }
            set
            {
                if (_checkState != value)
                {
                    _checkState = value;
                    OnCheckStateChanged(EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        public bool ThreeState
        {
            get
            {
                return _threeState;
            }
            set
            {
                if (value != _threeState)
                {
                    _threeState = value;
                }
            }
        }

        #endregion

        #region public methods

        public void SetNewTheme(SmartCheckBoxThemeBase xtheme)
        {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            Invalidate();
        }

        #endregion

        #region override

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (ThreeState)
            {
                if (CheckState == System.Windows.Forms.CheckState.Unchecked)
                    CheckState = System.Windows.Forms.CheckState.Checked;
                else if (CheckState == System.Windows.Forms.CheckState.Checked)
                    CheckState = System.Windows.Forms.CheckState.Indeterminate;
                else if (CheckState == System.Windows.Forms.CheckState.Indeterminate)
                    CheckState = System.Windows.Forms.CheckState.Unchecked;
            }
            else
            {
                if (CheckState == System.Windows.Forms.CheckState.Unchecked)
                    CheckState = System.Windows.Forms.CheckState.Checked;
                else
                    CheckState = System.Windows.Forms.CheckState.Unchecked;
            }
        }

        protected override void OnPaintContent(Graphics g, Rectangle clipRect)
        {
            base.OnPaintContent(g, clipRect);
            CheckButtonPainter.RenderCheckButton(
                g,
                Bounds,
                XTheme,
                Enabled,
                CheckState,
                State,
                Text,
                CheckRectAlign,
                false);
        }

        #endregion
    }
}
