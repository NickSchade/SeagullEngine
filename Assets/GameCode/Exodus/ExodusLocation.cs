using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusLocation : HomelandsLocation, ILocation
{
    ExodusLocationHabitability _habitability;

    public ExodusLocation(Pos pos, ILocationDrawer locationDrawer) : base(pos, locationDrawer)
    {

    }
}

public class ExodusLocationHabitability
{

}
