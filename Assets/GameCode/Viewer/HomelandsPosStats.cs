using UnityEngine;
using System.Collections;

public struct HomelandsPosStats 
{
    public Pos _pos;
    public HomelandsPosStatsVision _vision;
    public HomelandsPosStatsControl _control;
    public HomelandsPosStatsExtraction _extraction;
    public HomelandsPosStatsMilitary _military;
    public HomelandsPosStats(Pos pos, HomelandsPosStatsVision vision, HomelandsPosStatsControl control, HomelandsPosStatsExtraction extraction, HomelandsPosStatsMilitary military)
    {
        _pos = pos;
        _vision = vision;
        _control = control;
        _extraction = extraction;
        _military = military;
    }
}


public struct HomelandsPosStatsVision
{
    public eVisibility _visibility;
    public HomelandsPosStatsVision(eVisibility visibility)
    {
        _visibility = visibility;
    }
}

public struct HomelandsPosStatsControl
{
    public bool _controlled; // replace with List<HomelandsPlayer> _controllingPlayers after HomelandsPlayer becomes a thing
    public HomelandsPosStatsControl(bool controlled)
    {
        _controlled = controlled;
    }
}


public struct HomelandsPosStatsExtraction
{
    public float _extractionRate; // replace with HomelandsExtractionRate object or even D<eResource,HomelandsExtractionRate> after those become things
    public HomelandsPosStatsExtraction(float extractionRate)
    {
        _extractionRate = extractionRate;
    }
}


public struct HomelandsPosStatsMilitary
{
    public float _attack; // replace with D<Structure,Attack> or something
    public HomelandsPosStatsMilitary(float attack)
    {
        _attack = attack;
    }
}

