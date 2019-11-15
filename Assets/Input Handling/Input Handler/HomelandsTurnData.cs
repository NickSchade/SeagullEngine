using UnityEngine;
using System.Collections;

public class HomelandsTurnData 
{
    public KeyHandlerOutput _kho;
    public MouseHandlerOutput _mho;
    public HomelandsTurnData(KeyHandlerOutput kho, MouseHandlerOutput mho)
    {
        _kho = kho;
        _mho = mho;
    }
}
