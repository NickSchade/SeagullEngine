using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDrawerBasic : ILocationDrawer
{
    public LocationGraphicsData Draw()
    {
        return new LocationGraphicsData(Color.blue);
    }
}
