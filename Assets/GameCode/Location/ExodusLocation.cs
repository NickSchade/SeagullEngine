using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExodusLocation : HomelandsLocation
{
    ExodusLocationHabitability _habitability;

    public ExodusLocation(HomelandsGame game, Pos pos, Dictionary<string,float> locationQualities) : base(game, pos, locationQualities)
    {

    }
    
}

public class ExodusLocationHabitability
{

}
