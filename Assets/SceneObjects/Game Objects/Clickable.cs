using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{

    public Pos pos;
    // Use this for initialization
    void Start()
    {

    }
    
    public void setPos(Pos _pos)
    {
        pos = _pos;
    }
}
