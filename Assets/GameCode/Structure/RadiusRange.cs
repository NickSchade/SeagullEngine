using UnityEngine;
using System.Collections;

public class RadiusRange
{
    public float _range;
    public ePath _path;

    public RadiusRange(ePath path, float range)
    {
        _path = path;
        _range = range;
    }
}
