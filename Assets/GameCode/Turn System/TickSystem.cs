using UnityEngine;
using System.Collections.Generic;

public enum eTickSystem {TurnBased, SemiRealTime};

public interface ITickSystem
{
    TickInfo GetTick(List<GraphicsData> graphicsData);
}

public static class TickSystemFactory
{
    public static ITickSystem Make(eTickSystem type, HomelandsGame game)
    {
        if (type == eTickSystem.TurnBased)
        {
            return new TickSystemTurnBased(game);
        }
        else if (type == eTickSystem.SemiRealTime)
        {
            return new TickSystemSemiRealTime(game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
