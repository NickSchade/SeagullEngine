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
    public static ILocation Make(Pos p, eGame gameType, ILocationDrawer locationDrawer)
    {
        if (gameType == eGame.Exodus)
        {
            return new ExodusLocation(p, locationDrawer);
        }
        else if (gameType == eGame.HomelandsSandbox)
        {
            return new HomelandsSandboxLocation(p, locationDrawer);
        }
        else
        {
            throw new KeyNotFoundException();
        }
    }
}
