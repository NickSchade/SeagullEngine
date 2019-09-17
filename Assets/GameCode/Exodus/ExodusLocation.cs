using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusLocation : HomelandsLocation, ILocation
{
    ExodusLocationHabitability _habitability;

    public ExodusLocation(Pos pos, ILocationDrawer locationDrawer) : base(pos, locationDrawer)
    {

    }

    public override void TryToMakeStructure()
    {
        if (_structure == null)
        {
            _structure = StructureFactory.Make(eGame.Exodus);
        }
        else
        {
            Debug.Log("Can't Build");
        }
    }
}

public class ExodusLocationHabitability
{

}
