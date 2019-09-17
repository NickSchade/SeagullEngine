using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IStructure
{
    void Click();
    StructureGraphicsData Draw();
}
public class NullStructure : IStructure
{
    public void Click()
    {
        throw new System.NotImplementedException();
    }

    public StructureGraphicsData Draw()
    {
        return null;
    }
}
