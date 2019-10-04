using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusLocation : HomelandsLocation
{
    ExodusLocationHabitability _habitability;

    public ExodusLocation(HomelandsGame game, Pos pos, HomelandsTerrain terrain, HomelandsResource resource) : base(game, pos, terrain, resource)
    {

    }
    
}

