using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface IMouseHandler
{
    StructurePlacementData HandleMouse(MouseHandlerInfo mouseHandlerInfo);
}
