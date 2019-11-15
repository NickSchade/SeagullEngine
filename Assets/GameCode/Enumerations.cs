using UnityEngine;
using System.Collections;


public enum eGame { Exodus, Sandbox, HotSeat }

public enum eTileShape { Square, Hex };
public enum eVisibility { Visible, Fog, Unexplored };
public enum eView { God, Vision, Control, Extraction, Military, Null };
public enum eTerrain { Land, Sea, Mountain };

public enum eRadius { Vision, Control, Extraction, Military };
public enum ePath { Euclidian, NodeUniform, NodeEuclidian, NodeWeight };

public enum eEndCondition { LastOneStanding, Survival};
public enum eTickSystem { TurnBased, SemiRealTime };