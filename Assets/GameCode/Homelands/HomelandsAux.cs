using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomelandsPosVisibility
{
    public eVisibility _visibility;
}
public class HomelandsPosControl
{
    List<HomelandsPlayer> _controllingPlayers;
}
public class HomelandsPosExtraction
{
    Dictionary<eResource, HomelandsExtractionRate> _extractionStats;
}
public class HomelandsExtractionRate
{

}
public class HomelandsPosMilitary
{
    public float GetTotalAttacking()
    {
        throw new KeyNotFoundException();
    }
    public Dictionary<HomelandsStructure, HomelandsStructureAttack> _attackers;

}
public class HomelandsStructureAttack
{

}
public class HomelandsPosStats
{
    public HomelandsPosVisibility _visibility;
    public HomelandsPosControl _control;
    public HomelandsPosExtraction _extraction;
    public HomelandsPosMilitary _military;
}
