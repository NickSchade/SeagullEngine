using UnityEngine;
using System.Collections;

public enum eInput { Down, Held, Up }
public class InputHandlerInfo
{
    public KeyHandlerInfo _keyHandlerInfo;
    public MouseHandlerInfo _mouseHandlerInfo;
    public InputHandlerInfo(KeyHandlerInfo keyHandlerInfo, MouseHandlerInfo mouseHandlerInfo)
    {
        _keyHandlerInfo = keyHandlerInfo;
        _mouseHandlerInfo = mouseHandlerInfo;
    }
}
