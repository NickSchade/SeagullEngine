using UnityEngine;
using System.Collections;


public class SandboxLocation : HomelandsLocation, ILocation
{
    public SandboxLocation(Pos pos, ILocationDrawer locationDrawer) : base(pos, locationDrawer)
    {
    }

    public override void TryToMakeStructure()
    {
        if (_structure == null)
        {
            _structure = StructureFactory.Make(eGame.Sandbox);
        }
        else
        {
            Debug.Log("Can't Build");
        }
    }
}
