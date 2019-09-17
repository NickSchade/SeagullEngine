using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocation
{
    IStructure _structure { get; set; }
    void Click();
    GraphicsData Draw();
    LocationGraphicsData GetLocationData();
}

public static class LocationFactory
{
    public static HomelandsLocation Make(Pos p, eGame gameType, ILocationDrawer locationDrawer)
    {
        if (gameType == eGame.Exodus)
        {
            return new ExodusLocation(p, locationDrawer);
        }
        else if (gameType == eGame.Sandbox)
        {
            return new SandboxLocation(p, locationDrawer);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }
}
