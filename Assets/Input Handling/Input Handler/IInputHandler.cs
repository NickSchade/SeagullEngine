using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    IKeyHandler _keyHandler { get; set; }
    IMouseHandler _mouseHandler { get; set; }
    HomelandsTurnData HandleInput(InputHandlerInfo inputHandlerInfo);
}
