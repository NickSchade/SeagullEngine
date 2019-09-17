using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseHandlerInfo
{
    public Pos _pos;
    public float _axis;
    public Dictionary<eInput, bool> _left;
    public Dictionary<eInput, bool> _right;
    public MouseHandlerInfo(Pos pos, float axis, Dictionary<eInput, bool> left, Dictionary<eInput, bool> right)
    {
        _pos = pos;
        _axis = axis;
        _left = left;
        _right = right;
    }
    public MouseHandlerInfo(float axis)
    {
        _axis = axis;
    }
}
