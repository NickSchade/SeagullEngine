using UnityEngine;
using System.Collections.Generic;

public struct Stats 
{
    public Pos _pos;
    public StatsVision _vision;
    public StatsControl _control;
    public StatsBuild _build;
    public StatsExtraction _extraction;
    public StatsMilitary _military;
    public Stats(Pos pos, StatsVision vision, StatsControl control, StatsBuild build, StatsExtraction extraction, StatsMilitary military)
    {
        _pos = pos;
        _vision = vision;
        _control = control;
        _build = build;
        _extraction = extraction;
        _military = military;
    }
}





