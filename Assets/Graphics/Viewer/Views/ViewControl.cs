using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewControl : ViewVision
{
    public ViewControl(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        
        Color c = GetFogOfWarColor(location, stats);
        IPlayerSys ps = _game._playerSystem;
        Player currentPlayer = ps.GetPlayer();
        c = stats._control._controllingPlayers[currentPlayer]  ? Color.white : c;
        return new LocationGraphicsData(c);
    }

}
