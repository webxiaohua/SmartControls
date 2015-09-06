using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartWinControls.Common;

namespace SmartWinControls.SmartControls.Common
{
    /// <summary>
    /// 该接口提供一个ControlType属性来标识控件的类别，该类库的所有自定义控件都实现该接口    
    /// </summary>
    public interface ISmartControl
    {
        SmartControlType ControlType { get; }
    }
}
