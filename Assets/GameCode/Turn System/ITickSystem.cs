using UnityEngine;
using System.Collections.Generic;

public enum eTickSystem {TurnBased, SemiRealTime};

public interface ITickSystem
{
    TickInfo GetTick(List<GraphicsData> graphicsData);
}

