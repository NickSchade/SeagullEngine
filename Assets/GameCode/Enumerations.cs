using UnityEngine;
using System.Collections;


public enum eGame { Exodus, Sandbox }
public enum eTileShape { Square, Hex };
public enum eVisibility { Visible, Fog, Unexplored };
public enum eResource { Gold };
public enum eView { God, Vision, Control, Extraction, Military };
public enum eTerrain { Land, Sea, Mountain };

public enum eRadius { Vision, Control, Extraction, Military };
public enum ePath { Euclidian, NodeUniform, NodeEuclidian, NodeWeight };