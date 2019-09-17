using UnityEngine;
using System.Collections;

public enum eRadius { Vision, Control, Extraction, Military };
public enum ePath { Euclidian, NodeUniform, NodeEuclidian, NodeWeight };
public class RadiusRange
{
    float _range;
    ePath _path;
}
