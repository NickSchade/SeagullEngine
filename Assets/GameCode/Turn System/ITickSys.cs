using UnityEngine;
using System.Collections.Generic;

public enum eTickSystem {TurnBased, SemiRealTime};

public interface ITickSys
{
    TickInfo GetTick(List<GraphicsData> graphicsData, HomelandsTurnData turnData);
}

