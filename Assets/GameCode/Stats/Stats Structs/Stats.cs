using UnityEngine;
using System.Collections;

public struct Stats 
{
    public Pos _pos;
    public StatsVision _vision;
    public StatsControl _control;
    public StatsExtraction _extraction;
    public StatsMilitary _military;
    public Stats(Pos pos, StatsVision vision, StatsControl control, StatsExtraction extraction, StatsMilitary military)
    {
        _pos = pos;
        _vision = vision;
        _control = control;
        _extraction = extraction;
        _military = military;
    }
}





