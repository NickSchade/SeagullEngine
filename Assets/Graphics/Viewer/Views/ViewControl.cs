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
        PlayerSystem ps = _game._playerSystem;
        Player currentPlayer = ps._currentPlayer;
        c = stats._control._controllingPlayers[currentPlayer]  ? Color.white : c;
        return new LocationGraphicsData(c);
    }

}
