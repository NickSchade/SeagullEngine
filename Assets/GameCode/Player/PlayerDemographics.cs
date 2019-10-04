using UnityEngine;
using System.Collections;

public struct PlayerDemographics
{
    public string name;
    public Color color;
    public float resources;
    public PlayerDemographics(string n, Color c, float r)
    {
        name = n;
        color = c;
        resources = r;
    }
}
