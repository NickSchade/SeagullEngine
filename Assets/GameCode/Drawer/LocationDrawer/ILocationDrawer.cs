using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILocationDrawer 
{
    LocationGraphicsData Draw();
}

public enum eLocationDrawer { Basic, Homelands};

public static class LocationDrawerFactory
{
    public static ILocationDrawer Make(eLocationDrawer locationDrawerType)
    {
        if (locationDrawerType == eLocationDrawer.Basic)
        {
            return new LocationDrawerBasic();
        }
        else if (locationDrawerType == eLocationDrawer.Homelands)
        {
            return new LocationDrawerHomelands();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}

