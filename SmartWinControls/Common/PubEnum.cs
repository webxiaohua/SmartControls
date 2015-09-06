using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SmartWinControls.Common
{
    /// <summary>
    /// @Author:Robin
    /// @Date:2015-08-28
    /// @Desc：公共枚举类型
    /// </summary>
    
    public delegate GraphicsPath ButtonForePathGetter(Rectangle rect);

    /// <summary>
    /// 定义三态按钮的各个状态, 因为系统也有一个ButtonState类型，
    /// 为避免冲突，加了GM前缀
    /// </summary>
    public enum SmartButtonState
    {
        Normal,
        Hover,
        Pressed,
        PressLeave,
    }

    /// <summary>
    /// 圆角样式
    /// </summary>
    public enum RoundStyle
    {
        None,
        All,
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// 定义按钮的边框是方形的还是圆形的
    /// </summary>
    public enum ButtonBorderType
    {
        Rectangle,
        Ellipse,
    }
    /// <summary>
    /// 图片文字对齐方式
    /// </summary>
    public enum ButtonImageAlignment
    {
        Left,
        Top,
        Right,
        Bottom
    }

    public enum BackgroundStyle
    {
        Flat,
        LinearGradient
    }

    public enum MouseOperationType
    {
        Move,
        Down,
        Up,
        Hover,
        Leave,
        Wheel
    }

    public enum KeyOperationType
    {
        KeyDown,
        KeyUp
    }

    public enum ForePathRatoteDirection
    {
        Down,
        Left,
        Up,
        Right,
    }

    public enum ForePathRenderMode
    {
        Draw,
        Fill,
    }

    public enum ScrollBarShowMode
    {
        Never,
        Always,
        Auto
    }

    public enum ListSelectionMode
    {
        None,
        One,
        Multiple
    }

    public enum ListDrawMode
    {
        AutoDraw,
        UserDraw
    }

    public enum ResizeGridLocation
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public enum GlassPosition
    {
        Fill,
        Top,
        Right,
        Left,
        Bottom,
    }

    public enum BarButtonAlignmentType
    {
        Left,
        //Right,
        AfterLastTab,
    }

    public enum CheckMarkAlignment
    {
        Left,
        Right,
    }
}
