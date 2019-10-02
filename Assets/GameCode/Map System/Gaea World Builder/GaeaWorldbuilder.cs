using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Text;
using System;




/// <summary>
/// TO DO: MapGen - Continents: Create Map by running mapgen multiple times, creating a different continent each time
/// TO DO: Elevation - Fill Depressions: Planchon-Darboux algorithm
/// TO DO: Elevation - Add Spikey Blob Masks: https://azgaar.wordpress.com/2017/04/01/heightmap/
/// TO DO: MapPainter - Display View that fills in different colors for different areas
/// TO DO: Intermediate Colors - find a simple package w/ 20-30 colors instead of only 8.
/// TO DO: Animate Generation - waits are tough in unity, so this is hard until i learn that
/// TO DO: Simple City growth algorithm: each city w/ an empty neighbor builds a new city at that location which maximizes resources [f(waterFlux,Temp)] and minimizes costs [f(distance from new loc to all existing cities)]
/// </summary>


public enum WaterBodyPrefence { None, Islands, Continent, Lakes, Coast };

public class GaeaWorldbuilder
{
    public Benchmark bench;
    public int xDim, yDim, iExpand;
    public float[,] Elevation, Rain, WaterFlux, Temperature, WindMagnitude, Regions, Flatness, Fertility, Harbor, Land;
    public Vec[,] WindVector, Downhill;
    public float seaLevel, riverLevel, iceLevel;
    public float percentSea, percentRiver;
    public ElevationBuilder elevationBuilder;
    public RainBuilder RainBuilder;
    public FlowBuilder flowBuilder;

    WaterBodyPrefence prefWaterBody = WaterBodyPrefence.Continent;

    // Use this for initialization
    public GaeaWorldbuilder(int _xDim, int _yDim, int _iExpand = 1, float _percentSea = 0.5f, float _percentRiver = 0.01f)
    {
        xDim = _xDim;
        yDim = _yDim;
        iExpand = _iExpand;
        seaLevel = 0.5f;
        riverLevel = 0.5f;
        iceLevel = 0.3f;
        percentRiver = _percentRiver;
        percentSea = _percentSea;
        initializeArrays();

    }

    void initializeArrays()
    {
        Elevation = new float[xDim, yDim];
        WaterFlux = new float[xDim, yDim];
        Rain = new float[xDim, yDim];
        Temperature = new float[xDim, yDim];
        WindMagnitude = new float[xDim, yDim];
        Regions = new float[xDim, yDim];
        Flatness = new float[xDim, yDim];
        Fertility = new float[xDim, yDim];
        Harbor = new float[xDim, yDim];

    }

    // GENERATION
    public void GenerateMap()
    {
        bench = new Benchmark("Generate Map");
        Benchmark outerBench = new Benchmark("Enitre Gen");
        outerBench.StartBenchmark("Entire Gen");

        CreateElevation();

        ApplyPreferencesWaterBody();

        CreateElevationAdjustments(35, 0.5f);

        CreateRain(RainMethod.Wind);

        CreateFluxErosion(3, 30);

        //PaintRegions();

        bench.WriteBenchmarkToDebug();
        outerBench.EndBenchmark("Entire Gen");
        outerBench.WriteBenchmarkToDebug();
    }
    public void ApplyPreferencesWaterBody()
    {
        MapUtil.TransformApplyPreferencesWaterBody(ref Elevation, seaLevel, prefWaterBody, bench);
        SetTemperature();

    }
    public void CreateElevation()
    {
        //Debug.Log("Creating Elevation of Dimensions [" + xDim + "," + yDim + "]");
        elevationBuilder = new ElevationBuilder(MapUtil.nFromDims(xDim, yDim));
        elevationBuilder.SetElevationWithMidpointDisplacement(iExpand, bench: bench);
        //elevationBuilder.TrimToDimensions(xDim, yDim);
        Elevation = elevationBuilder.Elevation;

        xDim = Elevation.GetLength(0);
        yDim = Elevation.GetLength(1);
        //Debug.Log("Creating Elevation of Dimensions [" + xDim + "," + yDim + "]");
        WaterFlux = new float[xDim, yDim];
        Rain = new float[xDim, yDim];
        Temperature = new float[xDim, yDim];
        WindMagnitude = new float[xDim, yDim];
        Regions = new float[xDim, yDim];
        Flatness = new float[xDim, yDim];
        Fertility = new float[xDim, yDim];
        Harbor = new float[xDim, yDim];
        SetTemperature();
    }
    public void CreateElevationAdjustments(int iterDepression = 25, float waterPercent = 0.5f)
    {
        ResolveDepression(iterDepression, waterPercent);
        SetTemperature(); // Temp now in case we need it for rain
    }
    public void ResolveDepression(int iterDepression = 35, float waterPercent = 0.5f)
    {
        MapUtil.TransformResolveDepressions(ref Elevation, iterDepression, bench, false);
        MapUtil.TransformEqualizeMapByLevel(ref seaLevel, ref Elevation, waterPercent, bench);
    }
    public void SetTemperature()
    {
        Temperature = TemperatureBuilder.BuildTemperature(Elevation, seaLevel);

    }
    public void CreateRain(RainMethod rainMethod)
    {
        RainBuilder = new RainBuilder(Elevation, Temperature, seaLevel);
        RainBuilder.BuildRain(rainMethod, bench);
        Rain = RainBuilder.Rain;
        WindMagnitude = RainBuilder.WindMagnitude;
        WindVector = RainBuilder.WindVectors;
    }
    public void CreateFluxErosion(int erosionIterations, int flowIterations)
    {
        flowBuilder = new FlowBuilder(Elevation, Rain);
        WaterFlux = flowBuilder.Flow;
        RiverErosionLoop(erosionIterations, flowIterations);
        MapUtil.TransformResolveDepressions(ref Elevation, 1, bench);
        Temperature = TemperatureBuilder.BuildTemperature(Elevation, seaLevel);
        Downhill = MapUtil.GetDownhillVectors(Elevation);
        SetFlatness();
        SetFertility();
        SetHarbor();
        SetLands();
        //PaintRegions();
    }
    public void ErosionStep()
    {
        ErosionBuilder.HydraulicErosion(Elevation, WaterFlux, 0.3f);
    }
    public void FlowStep(int flowIterations)
    {
        flowBuilder.FlowStep(flowIterations, bench);
        MapUtil.TransformMap(ref WaterFlux, MapUtil.dExponentiate, 0.5f, bench);
        MapUtil.TransformMapMinMax(ref WaterFlux, MapUtil.dNormalize, bench);
        MapUtil.TransformMapMinMax(ref Elevation, MapUtil.dNormalize, bench);
    }
    public void RiverErosionLoop(int erosionIterations, int flowIterations)
    {
        for (int i = 0; i < erosionIterations; i++)
        {
            FlowStep(flowIterations);
            ErosionStep();
            MapUtil.TransformResolveDepressions(ref Elevation, 10, bench);
        }
        MapUtil.TransformMapMinMax(ref Elevation, MapUtil.dNormalize, bench);
        MapUtil.TransformEqualizeMapByLevel(ref seaLevel, ref Elevation, percentSea, bench);
        MapUtil.TransformEqualizeMapByLevelAboveSea(ref riverLevel, ref WaterFlux, percentRiver, Elevation, seaLevel);
    }
    public void PaintRegions()
    {
        Debug.Log("Painting Regions");
        MapPainter mp = new MapPainter(Elevation, seaLevel);
        Regions = mp.BuildRegions();
    }
    public void SetFlatness()
    {
        Flatness = MapUtil.GetSlopeMap(Elevation);
        MapUtil.TransformMapMinMax(ref Flatness, MapUtil.dNormalize);
        MapUtil.TransformMap(ref Flatness, MapUtil.dInvert, 0f);
        MapUtil.TransformMap(ref Flatness, MapUtil.dExponentiate, 0.5f);
    }
    public void SetFertility()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                float fertility = WaterFlux[x, y] * Flatness[x, y] * Mathf.Abs(0.5f - Temperature[x, y]);
                Fertility[x, y] = fertility;
            }
        }
        MapUtil.TransformMapMinMax(ref Fertility, MapUtil.dNormalize);
        MapUtil.TransformMap(ref Fertility, MapUtil.dExponentiate, 0.5f);
    }
    public void SetHarbor()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] > seaLevel && WaterFlux[x, y] < riverLevel)
                {
                    int adjacentOceanCount = 0;
                    Dictionary<MapLoc, float> neighbors = MapUtil.GetValidNeighbors(Elevation, new MapLoc(x, y));
                    foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
                    {
                        if (kvp.Value < seaLevel)
                        {
                            adjacentOceanCount++;
                        }
                    }
                    if (adjacentOceanCount == 0)
                    {
                        Harbor[x, y] = 0f;
                    }
                    else if (adjacentOceanCount == 1)
                    {
                        Harbor[x, y] = 1f;
                    }
                    else
                    {
                        Harbor[x, y] = 0.5f;
                    }
                }
            }
        }
    }
    public void SetLands()
    {
        Land = new float[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                float e = Elevation[x, y];
                Land[x, y] = e > seaLevel ? 1f : 0f;
            }
        }
    }

    // LOCATION SPECIFIC
    public Dictionary<string, float> GetLocationQualities(int x, int y)
    {
        Dictionary<string, float> qualities = new Dictionary<string, float>();

        qualities["Elevation"] = Elevation[x, y];
        qualities["Rain"] = Rain[x, y];
        qualities["WaterFlux"] = WaterFlux[x, y];
        qualities["Temperature"] = Temperature[x, y];
        qualities["WindMagnitude"] = WindMagnitude[x, y];
        qualities["Regions"] = Regions[x, y];
        qualities["Flatness"] = Flatness[x, y];
        qualities["Fertility"] = Fertility[x, y];
        qualities["Harbor"] = Harbor[x, y];
        qualities["Land"] = Land[x, y];
        qualities["Longitude"] = x;
        qualities["Latitude"] = y;

        return qualities;
    }

}

public static class TemperatureBuilder
{

    public static float[,] BuildTemperature(float[,] Elevation, float seaLevel, float elevationWeight = 0.5f, Benchmark bench = null)
    {
        if (!(bench == null))
        {
            bench.StartBenchmark("Temperature");
        }
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float Center = (yDim - 1) / 2f;
        float[,] Temp = new float[xDim, yDim];
        float distanceFromCenterRatio;
        for (int y = 0; y < yDim; y++)
        {
            distanceFromCenterRatio = Mathf.Abs(Center - y) / Center;
            for (int x = 0; x < xDim; x++)
            {
                Temp[x, y] = GetTemp(Elevation[x, y], elevationWeight, seaLevel, distanceFromCenterRatio);
            }
        }
        MapUtil.TransformMapMinMax(ref Temp, MapUtil.dNormalize);
        if (!(bench == null))
        {
            bench.EndBenchmark("Temperature");
        }
        return Temp;
    }
    private static float GetTemp(float elevation, float elevationWeight, float seaLevel, float distanceFromCenterRatio)
    {
        float d = (1 - distanceFromCenterRatio);
        float e = elevation > seaLevel ? MapUtil.dNormalize(elevation, seaLevel, 1f, 1f, 0f) : 1f;
        if (true)
        {
            d = (float)Math.Pow(d, 1.5);
            e = (float)Math.Pow(e, 1.5);
        }
        float t = elevationWeight * e + (1 - elevationWeight) * d;
        return t;
    }
}

public struct Vec
{
    public float x, y;
    public Vec(float _x, float _y)
    {
        x = _x;
        y = _y;
    }
    public Vec GetRotatedVector(float rotationInDegrees)
    {
        float rad = DegreesToRadians(rotationInDegrees);
        float cs = (float)Math.Cos(rad);
        float sn = (float)Math.Sin(rad);
        float px = x * cs - y * sn;
        float py = x * sn + y * cs;
        return new Vec(px, py);
    }
    private float DegreesToRadians(float degrees)
    {
        return (float)(degrees * (Math.PI / 180));
    }
}

public enum RainMethod { Equal, Noise, Wind };


public class RainBuilder
{
    public float[,] Elevation, Temperature, Rain, WindMagnitude;
    public List<Vec>[,] WindVectorsList;
    public Vec[,] WindVectors;
    public int xDim, yDim;
    public float seaLevel;

    public Wind[,] wind;

    public RainBuilder(float[,] _Elevation, float[,] _Temperature, float _seaLevel)
    {
        Elevation = _Elevation;
        Temperature = _Temperature;
        xDim = Elevation.GetLength(0);
        yDim = Elevation.GetLength(1);
        seaLevel = _seaLevel;
        Rain = new float[xDim, yDim];
        WindVectors = new Vec[xDim, yDim];
        WindVectorsList = new List<Vec>[xDim, yDim];
        WindMagnitude = new float[xDim, yDim];
    }

    public void BuildRain(RainMethod pm, Benchmark bench = null)
    {
        Benchmark.Start(bench, "Rain"); // etc
        if (pm == RainMethod.Equal)
        {
            BuildRainByEqual();
        }
        else if (pm == RainMethod.Noise)
        {
            BuildRainByNoise();
        }
        else if (pm == RainMethod.Wind)
        {
            WindSpawn ws = WindSpawn.TradeLinear;
            WindRainType rt = WindRainType.HeatBasedContinuous;
            WindEvaporationType et = WindEvaporationType.WaterAndHeat;
            WindTurnType wt = WindTurnType.TerrainBased;
            BuildRainByWind(1000, ws, rt, et, wt, bench);
            MapUtil.TransformMapMinMax(ref Rain, MapUtil.dNormalize);
        }
        if (bench != null)
        {
            bench.EndBenchmark("Rain");
        }
    }
    private void BuildRainByEqual()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Rain[x, y] = 1f;
            }
        }
    }
    private void BuildRainByNoise()
    {
        MidpointDisplacement mpd = new MidpointDisplacement(MapUtil.nFromDims(xDim, yDim));
        Rain = mpd.Elevation;
        MapUtil.TransformMapMinMax(ref Rain, MapUtil.dNormalize);
    }
    private void BuildRainByWind(int maxIter, WindSpawn windSpawn, WindRainType rainType, WindEvaporationType evapType, WindTurnType windTurnType, Benchmark bench = null)
    {
        Benchmark.Start(bench, "Wind");
        List<Wind> winds = new List<Wind>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                WindVectorsList[x, y] = new List<Vec>();
                Wind w = new Wind(x, y, Elevation, windSpawn);
                WindVectorsList[x, y].Add(new Vec(w.velocity.x, w.velocity.y));
                winds.Add(w);
            }
        }
        int iter = 0;
        int numberOfWinds = winds.Count;
        while (iter < maxIter)
        {
            numberOfWinds = winds.Count;
            if (numberOfWinds == 0)
            {
                break;
            }
            else
            {
                BuildRainByWindWorkhorse(ref winds, windSpawn, rainType, evapType, windTurnType);
            }
            iter++;
        }
        MapUtil.TransformMap(ref Rain, MapUtil.dExponentiate, 0.125f);
        TabulateWindflow();
        if (bench != null)
        {
            bench.EndBenchmark("Wind");
        }
    }
    private void BuildRainByWindWorkhorse(ref List<Wind> winds, WindSpawn windSpawn, WindRainType rainType, WindEvaporationType evapType, WindTurnType windTurnType)
    {
        //  Combine Winds at the same location
        CombineWinds(ref winds, windSpawn);
        // Add to WindFlow
        foreach (Wind w in winds)
        {
            WindVectorsList[w.approxMapLoc.x, w.approxMapLoc.y].Add(new Vec(w.velocity.x, w.velocity.y));
        }
        //  Move wind in the direction vector
        foreach (Wind w in winds)
        {
            w.Blow(Elevation, Temperature);
        }
        //  If the wind moves off the map, remove it
        RemoveOffWindMaps(ref winds);
        //  Rain
        foreach (Wind w in winds)
        {
            w.Rain(rainType, ref Rain, Elevation, seaLevel);
        }
        // Evaporate
        foreach (Wind w in winds)
        {
            w.Evaporate(evapType, Elevation, Temperature, seaLevel);
        }
        // Turn Wind
        foreach (Wind w in winds)
        {
            w.TurnWind(windTurnType, Elevation, seaLevel);
        }
    }
    private void RemoveOffWindMaps(ref List<Wind> winds)
    {
        List<Wind> newWinds = new List<Wind>();
        for (int i = 0; i < winds.Count; i++)
        {
            if (winds[i].WindOnMap == true)
            {
                newWinds.Add(winds[i]);
            }
        }
        winds = newWinds;
    }
    private void CombineWinds(ref List<Wind> winds, WindSpawn ws, bool addNewWinds = false)
    {
        List<Wind>[,] stackedWindsList = new List<Wind>[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                stackedWindsList[x, y] = new List<Wind>();
            }
        }
        foreach (Wind w in winds)
        {
            stackedWindsList[w.approxMapLoc.x, w.approxMapLoc.y].Add(w);
        }
        List<Wind> newWinds = new List<Wind>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (addNewWinds)
                {
                    stackedWindsList[x, y].Add(new Wind(x, y, Elevation, ws));
                }
                if (stackedWindsList[x, y].Count > 0)
                {
                    newWinds.Add(CombineWindsAtMapLocation(stackedWindsList[x, y]));
                }
            }
        }
        winds = newWinds;
    }
    private Wind CombineWindsAtMapLocation(List<Wind> windsAtMapLocation)
    {
        List<float> xl = new List<float>();
        List<float> yl = new List<float>();
        List<float> xv = new List<float>();
        List<float> yv = new List<float>();
        List<float> water = new List<float>();
        foreach (Wind w in windsAtMapLocation)
        {
            xl.Add(w.actualMapLoc.x);
            yl.Add(w.actualMapLoc.y);
            xv.Add(w.velocity.x);
            yv.Add(w.velocity.y);
            water.Add(w.Water);
        }
        float _x = xl.Average();
        float _y = yl.Average();
        float _xv = xv.Average();
        float _yv = yv.Average();
        float _water = water.Average();
        return new Wind(_x, _y, _xv, _yv, _water, Elevation);
    }
    private void TabulateWindflow()
    {
        float[,] xa = new float[xDim, yDim];
        float[,] ya = new float[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                List<float> xl = new List<float>();
                List<float> yl = new List<float>();
                foreach (Vec v in WindVectorsList[x, y])
                {
                    xl.Add(v.x);
                    yl.Add(v.y);
                }
                float fx = xl.Sum();
                float fy = yl.Sum();
                xa[x, y] = fx;
                ya[x, y] = fy;
                WindMagnitude[x, y] = (float)Math.Sqrt(fx * fx + fy * fy);
            }
        }
        MapUtil.TransformMapMinMax(ref WindMagnitude, MapUtil.dNormalize);
        MapUtil.TransformMap(ref WindMagnitude, MapUtil.dExponentiate, 0.25f);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                WindVectors[x, y] = new Vec(xa[x, y], ya[x, y]);
            }
        }
    }
}

public enum WindSpawn { Easterly, TradeSine, TradeLinear };
public enum WindEvaporationType { WaterOnly, WaterAndHeat }
public enum WindRainType { UphillDiscrete, HeatBasedDiscrete, UphillContinuous, HeatBasedContinuous };
public enum WindTurnType { None, RandomWalk, TerrainBased };

public class Wind
{
    public Vec velocity;
    public Vec actualMapLoc;
    public MapLoc approxMapLoc;
    public float Water, deltaHeight, deltaTemp, seaLevel;
    public int xDim, yDim;
    public bool WindOnMap, WindOverWater;
    public Wind(float _x, float _y, float[,] Elevation, WindSpawn ws)
    {
        Water = 0.1f;
        deltaHeight = 0f;
        deltaTemp = 0f;
        actualMapLoc = new Vec(_x, _y);
        approxMapLoc = new MapLoc((int)_x, (int)_y);
        WindOnMap = true;
        xDim = Elevation.GetLength(0);
        yDim = Elevation.GetLength(1);
        if (ws == WindSpawn.Easterly)
        {
            velocity = new Vec(1, 0);
        }
        else if (ws == WindSpawn.TradeSine)
        {
            velocity = TradeWindSine();
        }
        else if (ws == WindSpawn.TradeLinear)
        {
            velocity = TradeWindLinear();
        }
    }
    public Wind(float _x, float _y, float _xv, float _yv, float _water, float[,] Elevation)
    {
        actualMapLoc = new Vec(_x, _y);
        approxMapLoc = new MapLoc((int)_x, (int)_y);
        velocity = new Vec(_xv, _yv);
        Water = _water;
        xDim = Elevation.GetLength(0);
        yDim = Elevation.GetLength(1);
    }
    public void Blow(float[,] Elevation, float[,] Temperature)
    {
        deltaHeight = Elevation[approxMapLoc.x, approxMapLoc.y];
        deltaTemp = Temperature[approxMapLoc.x, approxMapLoc.y];
        actualMapLoc = new Vec(actualMapLoc.x + velocity.x, actualMapLoc.y + velocity.y);
        approxMapLoc = new MapLoc((int)actualMapLoc.x, (int)actualMapLoc.y);
        WindOnMap = true;
        WindOnMap = (actualMapLoc.x < 0 || actualMapLoc.x >= xDim) ? false : WindOnMap;
        WindOnMap = (approxMapLoc.x < 0 || approxMapLoc.x >= xDim) ? false : WindOnMap;
        WindOnMap = (actualMapLoc.y < 0 || actualMapLoc.y >= yDim) ? false : WindOnMap;
        WindOnMap = (approxMapLoc.y < 0 || approxMapLoc.y >= yDim) ? false : WindOnMap;
        if (WindOnMap)
        {
            deltaHeight = Elevation[approxMapLoc.x, approxMapLoc.y] - deltaHeight;
            deltaTemp = Temperature[approxMapLoc.x, approxMapLoc.y] - deltaTemp;
        }
    }
    public void Rain(WindRainType rt, ref float[,] Rain, float[,] Elevation, float seaLevel)
    {
        if (rt == WindRainType.UphillDiscrete)
        {
            if (deltaHeight > 0f && Elevation[approxMapLoc.x, approxMapLoc.y] > seaLevel)
            {
                float rainAmount = 0.1f * Water;
                RainWorkhorse(ref Rain, rainAmount);
            }
        }
        else if (rt == WindRainType.HeatBasedDiscrete)
        {
            if (deltaTemp < -0f && Elevation[approxMapLoc.x, approxMapLoc.y] > seaLevel)
            {
                float rainAmount = 0.1f * Water;
                RainWorkhorse(ref Rain, rainAmount);
            }
        }
        else if (rt == WindRainType.UphillContinuous)
        {
            if (deltaHeight > 0f && Elevation[approxMapLoc.x, approxMapLoc.y] > seaLevel)
            {
                float rainAmount = deltaHeight * Water;
                RainWorkhorse(ref Rain, rainAmount);
            }
        }
        else if (rt == WindRainType.HeatBasedContinuous)
        {
            if (deltaTemp < -0f && Elevation[approxMapLoc.x, approxMapLoc.y] > seaLevel)
            {
                float rainAmount = -deltaTemp * Water;
                RainWorkhorse(ref Rain, rainAmount);
            }
            if (Water > 10)
            {
                float rainAmount = 0.1f * Water;
                RainWorkhorse(ref Rain, rainAmount);
            }
        }
    }
    private void RainWorkhorse(ref float[,] Rain, float rainAmount)
    {
        Water -= rainAmount;
        Rain[approxMapLoc.x, approxMapLoc.y] += Water;
    }
    public void Evaporate(WindEvaporationType et, float[,] Elevation, float[,] Temperature, float seaLevel)
    {
        if (et == WindEvaporationType.WaterOnly)
        {
            if (Elevation[approxMapLoc.x, approxMapLoc.y] < seaLevel)
            {
                Water += 1f;
            }
        }
        else if (et == WindEvaporationType.WaterAndHeat)
        {
            if (Elevation[approxMapLoc.x, approxMapLoc.y] < seaLevel)
            {
                Water += Temperature[approxMapLoc.x, approxMapLoc.y];
            }
        }
    }
    public void TurnWind(WindTurnType wt, float[,] Elevation, float seaLevel)
    {
        if (wt == WindTurnType.None)
        {
            // do nothing
        }
        else if (wt == WindTurnType.RandomWalk)
        {
            int r = UnityEngine.Random.Range(-1, 1);
            velocity = new Vec(velocity.x, r);
        }
        else if (wt == WindTurnType.TerrainBased)
        {
            if (Elevation[approxMapLoc.x, approxMapLoc.y] > seaLevel)
            {
                velocity.x -= deltaHeight;
                velocity.y -= deltaHeight;
                Vec lVec = velocity.GetRotatedVector(-90);
                Vec rVec = velocity.GetRotatedVector(90);
                MapLoc lMapLoc = new MapLoc((int)(Math.Max(1, actualMapLoc.x + lVec.x)), (int)Math.Max(1, actualMapLoc.y + lVec.y));
                MapLoc rMapLoc = new MapLoc((int)(Math.Max(1, actualMapLoc.x + rVec.x)), (int)Math.Max(1, actualMapLoc.y + rVec.y));
                try
                {
                    float fDiff = Elevation[lMapLoc.x, lMapLoc.y] - Elevation[rMapLoc.x, rMapLoc.y];
                    velocity.x += lMapLoc.x * fDiff;
                    velocity.y += lMapLoc.y * fDiff;
                }
                catch
                {
                    // ¯\_(ツ)_/¯
                }
            }
        }
    }
    private Vec TradeWindSine()
    {
        float yC = (yDim - 1) / 2f;
        float EquatorToPolePosition = Math.Abs(approxMapLoc.y - yC) / yC;
        float yAdjustedInput = MapUtil.dNormalize(EquatorToPolePosition, 0f, 1f, -0.499f, 0.999f);
        //float xDir = (float)Math.Tan(Math.PI * yAdjustedInput);
        //float yDir = (float)Math.Tan(Math.PI * 0.5f - Math.PI * yAdjustedInput);
        float xDir = (float)Math.Tan(yAdjustedInput);
        float yDir = (float)Math.Tan(0.5f - yAdjustedInput);

        //Debug.Log("Wind at " + approxMapLoc.y + "("+EquatorToPolePosition+") is "+xDir+","+yDir);
        return new Vec(xDir, yDir);
    }
    private Vec TradeWindLinear()
    {
        float yC = (yDim - 1) / 2f;
        float EquatorToPolePosition = Math.Abs(approxMapLoc.y - yC) / yC;
        EquatorToPolePosition = MapUtil.dNormalize(EquatorToPolePosition, 0f, 1f, -0.499f, 0.999f);
        float xDir, yDir;
        if (EquatorToPolePosition <= 0.0f)
        {
            float newLerp = MapUtil.dNormalize(EquatorToPolePosition, -0.5f, 0f);
            xDir = -1f + newLerp;
            yDir = -newLerp;
        }
        else if (EquatorToPolePosition <= 0.5f)
        {
            float newLerp = MapUtil.dNormalize(EquatorToPolePosition, 0f, 0.5f);
            xDir = newLerp;
            yDir = 1f - newLerp;
        }
        else
        {
            float newLerp = MapUtil.dNormalize(EquatorToPolePosition, 0.5f, 1.0f);
            xDir = -1f + newLerp;
            yDir = -newLerp;
        }
        //Debug.Log("Wind at " + approxMapLoc.y + "(" + EquatorToPolePosition + ") is " + xDir + "," + yDir);
        return new Vec(xDir, yDir);
    }
}



public class MapScaler
{
    public MapScaler()
    {
    }
    public Dictionary<string, float[,]> BuildSurfaces(GaeaWorldbuilder mg)
    {
        Dictionary<string, float[,]> surfaces = new Dictionary<string, float[,]> {
            { "Elevation", mg.Elevation },
            { "Rain", mg.Rain },
            { "WaterFlux", mg.WaterFlux },
            { "Temperature", mg.Temperature },
            { "WindMagnitude", mg.WindMagnitude  },
            { "Flatness", mg.Flatness },
            { "Fertility", mg.Fertility },
            { "Harbor", mg.Harbor}
                    };
        return surfaces;
    }
    public void Crop(ref GaeaWorldbuilder inputMap, int xDim, int yDim, int xTL, int yTL, bool bNormalize = false)
    {
        GaeaWorldbuilder outputMap = new GaeaWorldbuilder(xDim, yDim);
        outputMap.seaLevel = inputMap.seaLevel;
        outputMap.riverLevel = inputMap.riverLevel;
        Dictionary<string, float[,]> inputSurfaces = BuildSurfaces(inputMap);
        Dictionary<string, float[,]> outputSurfaces = BuildSurfaces(outputMap);

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                foreach (KeyValuePair<string, float[,]> kvp in inputSurfaces)
                {
                    string k = kvp.Key;
                    outputSurfaces[k][x, y] = inputSurfaces[k][x + xTL, y + yTL];
                }
            }
        }
        if (bNormalize)
        {
            Normalize(ref outputMap, outputSurfaces);
        }
        inputMap = outputMap;
    }
    public void ExpandSquare(ref GaeaWorldbuilder inputMap, int xE, int yE, bool bNormalize = true)
    {
        int xDim = inputMap.Elevation.GetLength(0);
        int yDim = inputMap.Elevation.GetLength(1);
        int xl = xDim * xE;
        int yl = yDim * yE;
        GaeaWorldbuilder outputMap = new GaeaWorldbuilder(xl, yl);
        outputMap.seaLevel = inputMap.seaLevel;
        outputMap.riverLevel = inputMap.riverLevel;

        Dictionary<string, float[,]> inputSurfaces = BuildSurfaces(inputMap);
        Dictionary<string, float[,]> outputSurfaces = BuildSurfaces(outputMap);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                for (int xi = 0; xi < xE; xi++)
                {
                    for (int yi = 0; yi < yE; yi++)
                    {
                        foreach (KeyValuePair<string, float[,]> kvp in inputSurfaces)
                        {
                            string k = kvp.Key;
                            int xp1 = x < xDim - 1 ? x + 1 : x;
                            int yp1 = y < yDim - 1 ? y + 1 : y;
                            float xLerpTop = Mathf.Lerp(inputSurfaces[k][x, y], inputSurfaces[k][xp1, y], (float)xi / (float)xE);
                            float xLerpBot = Mathf.Lerp(inputSurfaces[k][x, yp1], inputSurfaces[k][xp1, yp1], (float)xi / (float)xE);
                            float lerpVal = Mathf.Lerp(xLerpTop, xLerpBot, yi / yE);
                            int xind = x * xE + xi;
                            int yind = y * yE + yi;
                            outputSurfaces[k][xind, yind] = lerpVal;
                        }
                    }
                }
            }
        }
        if (bNormalize)
        {
            Normalize(ref outputMap, outputSurfaces);
        }
        inputMap = outputMap;
    }
    public void Smooth(ref GaeaWorldbuilder inputMap, bool bNormalize = true)
    {
        int xDim = inputMap.Elevation.GetLength(0);
        int yDim = inputMap.Elevation.GetLength(1);
        GaeaWorldbuilder outputMap = new GaeaWorldbuilder(xDim, yDim);
        outputMap.seaLevel = inputMap.seaLevel;
        outputMap.riverLevel = inputMap.riverLevel;

        Dictionary<string, float[,]> inputSurfaces = BuildSurfaces(inputMap);
        Dictionary<string, float[,]> outputSurfaces = BuildSurfaces(outputMap);
        foreach (KeyValuePair<string, float[,]> kvp in inputSurfaces)
        {
            for (int x = 0; x < xDim; x++)
            {
                for (int y = 0; y < yDim; y++)
                {
                    outputSurfaces[kvp.Key][x, y] = MapUtil.dSmooth(x, y, inputSurfaces[kvp.Key]);
                }
            }

        }
        if (bNormalize)
        {
            Normalize(ref outputMap, outputSurfaces);
        }
        inputMap = outputMap;
    }
    private void Normalize(ref GaeaWorldbuilder inputMap, Dictionary<string, float[,]> inputSurfaces)
    {
        foreach (KeyValuePair<string, float[,]> kvp in inputSurfaces)
        {
            float[,] surface = kvp.Value;
            MapUtil.TransformMapMinMax(ref surface, MapUtil.dNormalize);
        }
        MapUtil.TransformEqualizeMapByLevel(ref inputMap.seaLevel, ref inputMap.Elevation, 0.5f);
        MapUtil.TransformEqualizeMapByLevelAboveSea(ref inputMap.riverLevel, ref inputMap.WaterFlux, 0.05f, inputMap.Elevation, inputMap.seaLevel);
    }
}
public class ElevationBuilder
{
    public int Dim, N;
    public float[,] Elevation;
    public ElevationBuilder(int _N)
    {
        // Currently, Dimensions must be W = H and W = 2^N + 1 to make midpoint displacement simpler
        // Later, we should build the midpoint displacement map, then stretch it to the appropriate shape 
        // OR build a bigger MPD map, then truncate to fit w and h
        N = _N;
        Dim = (int)Mathf.Pow(2, N) + 1;
        Elevation = new float[Dim, Dim];
    }
    public void SetElevationWithMidpointDisplacement(int iExpand, Benchmark bench = null)
    {
        if (!(bench == null))
        {
            bench.StartBenchmark("Midpoint Displacement");
        }
        MidpointDisplacement mpd = new MidpointDisplacement(N);
        Elevation = mpd.Elevation;
        MapUtil.TransformMapMinMax(ref Elevation, MapUtil.dNormalize);
        if (iExpand > 1)
        {
            ExpandSquare(iExpand, iExpand);
            Smooth();
        }
        if (!(bench == null))
        {
            bench.EndBenchmark("Midpoint Displacement");
        }
    }
    public void TrimToDimensions(int xDim, int yDim)
    {
        float[,] dElev = new float[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                dElev[x, y] = Elevation[x, y];
            }
        }
        Elevation = dElev;
    }
    public void ExpandSquare(int xE, int yE, bool bNormalize = true)
    {
        PrintDims();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        int xl = xDim * xE;
        int yl = yDim * yE;
        float[,] newElevation = new float[xl, yl];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                for (int xi = 0; xi < xE; xi++)
                {
                    for (int yi = 0; yi < yE; yi++)
                    {
                        int xp1 = x < xDim - 1 ? x + 1 : x;
                        int yp1 = y < yDim - 1 ? y + 1 : y;
                        float xLerpTop = Mathf.Lerp(Elevation[x, y], Elevation[xp1, y], (float)xi / (float)xE);
                        float xLerpBot = Mathf.Lerp(Elevation[x, yp1], Elevation[xp1, yp1], (float)xi / (float)xE);
                        float lerpVal = Mathf.Lerp(xLerpTop, xLerpBot, yi / yE);
                        int xind = x * xE + xi;
                        int yind = y * yE + yi;
                        newElevation[xind, yind] = lerpVal;
                    }
                }
            }
        }
        if (bNormalize)
        {
            MapUtil.TransformMapMinMax(ref newElevation, MapUtil.dNormalize);
        }
        Elevation = newElevation;
        PrintDims();
    }
    public void Smooth(bool bNormalize = true)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float[,] newElevation = new float[xDim, yDim];

        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                newElevation[x, y] = MapUtil.dSmooth(x, y, Elevation);
            }
        }
        if (bNormalize)
        {
            MapUtil.TransformMapMinMax(ref newElevation, MapUtil.dNormalize);
        }
        Elevation = newElevation;
    }
    public void PrintDims()
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);

        Debug.Log("Elevation[" + xDim + "," + yDim + "]");
    }
}

public static class ErosionBuilder
{
    public static void HydraulicErosion(float[,] Elevation, float[,] WaterFlux, float erosionRate, Benchmark bench = null)
    {
        if (bench != null)
        {
            bench.StartBenchmark("Erode");
        }
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Elevation[x, y] -= erosionRate * WaterFlux[x, y];
            }
        }
        if (bench != null)
        {
            bench.EndBenchmark("Erode");
        }
    }
    public static void ThermalErosion(float[,] Elevation, float talusAngle = 0.5f, int iterations = 1, Benchmark bench = null)
    {
        if (bench != null)
        {
            bench.StartBenchmark("Thermal Erosion");
        }

        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);

        for (int i = 0; i < iterations; i++)
        {
            for (int x = 0; x < xDim; x++)
            {
                for (int y = 0; y < yDim; y++)
                {
                    ThermalWorkhorse(Elevation, new MapLoc(x, y), talusAngle);
                }
            }
            MapUtil.TransformMapMinMax(ref Elevation, MapUtil.dNormalize, bench);
        }

        if (bench != null)
        {
            bench.EndBenchmark("Thermal Erosion");
        }
    }
    public static void ThermalWorkhorse(float[,] Elevation, MapLoc l, float talusAngle)
    {
        Dictionary<MapLoc, float> n = MapUtil.GetValidNeighbors(Elevation, l); // Thermal Erosion
        float di;
        float dtotal = 0f;
        float dmax = float.NegativeInfinity;
        foreach (KeyValuePair<MapLoc, float> kvp in n)
        {
            di = Elevation[l.x, l.y] - Elevation[kvp.Key.x, kvp.Key.y];
            if (di > talusAngle)
            {
                dtotal += di;
                di = di > dmax ? dmax : di;
            }
        }
        float startingElev = Elevation[l.x, l.y];
        foreach (KeyValuePair<MapLoc, float> kvp in n)
        {
            di = startingElev - Elevation[kvp.Key.x, kvp.Key.y];
            float delta = 0.5f * (dmax - talusAngle) * di / dtotal;
            Elevation[kvp.Key.x, kvp.Key.y] += delta;
            Elevation[l.x, l.y] -= delta;
        }
    }

}

public class FlowBuilder
{

    public float[,] Elevation;
    public float[,] Rain;
    public float[,] Flow;
    public float[,] Water;
    int xDim, yDim;

    public FlowBuilder(float[,] _Elevation, float[,] _Rain)
    {
        Elevation = _Elevation;
        Rain = _Rain;
        xDim = Elevation.GetLength(0);
        yDim = Elevation.GetLength(1);
        Flow = new float[xDim, yDim];
        Water = new float[xDim, yDim];
    }

    public void FlowStep(int iterations, Benchmark bench = null)
    {
        if (bench != null)
        {
            bench.StartBenchmark("Flow Step");
        }
        RainStep();
        for (int i = 0; i < iterations; i++)
        {
            AddWaterToFlow();
            float[,] flowStep = new float[xDim, yDim];
            for (int x = 0; x < xDim; x++)
            {
                for (int y = 0; y < yDim; y++)
                {
                    FlowProportional(ref flowStep, new MapLoc(x, y));
                }
            }
            Water = flowStep;
        }
        if (bench != null)
        {
            bench.EndBenchmark("Flow Step");
        }
    }
    public void FlowProportional(ref float[,] flowStep, MapLoc l)
    {
        Dictionary<MapLoc, float> neighbors = MapUtil.GetValidNeighbors(Elevation, l, false); // Flow Proportional
        float localHeight = Elevation[l.x, l.y];
        Dictionary<MapLoc, float> lowerNeighbors = new Dictionary<MapLoc, float>();
        float fDiff;
        float totalDiff = 0f;
        foreach (KeyValuePair<MapLoc, float> n in neighbors)
        {
            if (n.Value < localHeight)
            {
                fDiff = localHeight - n.Value;
                lowerNeighbors[n.Key] = fDiff;
                totalDiff += fDiff;
            }
        }

        if (lowerNeighbors.Count > 0)
        {
            foreach (KeyValuePair<MapLoc, float> n in lowerNeighbors)
            {
                flowStep[n.Key.x, n.Key.y] += Water[l.x, l.y] * n.Value / totalDiff;
            }
        }
        else
        {
            float newElev = MapUtil.GetNeighborAverage(Elevation, l);
            newElev = (newElev + Elevation[l.x, l.y]) / 2f;
            Elevation[l.x, l.y] = newElev;
        }
    }
    private void AddWaterToFlow()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Flow[x, y] += Water[x, y];
            }
        }
    }
    private void RainStep()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Water[x, y] += Rain[x, y];
            }
        }
    }

}

public struct MapLoc
{
    public int x, y;
    public float v;
    public MapLoc(int _x, int _y)
    {
        x = _x;
        y = _y;
        v = 0f;
    }
}


public static class MapUtil
{

    public static void GetMapMaxMinValues(float[,] Elevation, out float maxVal, out float minVal)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        minVal = float.PositiveInfinity;
        maxVal = float.NegativeInfinity;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                maxVal = Elevation[x, y] > maxVal ? Elevation[x, y] : maxVal;
                minVal = Elevation[x, y] < minVal ? Elevation[x, y] : minVal;
            }
        }
    }
    public static void GetMapMaxIndices(float[,] Elevation, out int xMaxIndex, out int yMaxIndex)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float maxVal = float.NegativeInfinity;
        xMaxIndex = 0;
        yMaxIndex = 0;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] > maxVal)
                {
                    maxVal = Elevation[x, y];
                    xMaxIndex = x;
                    yMaxIndex = y;
                }
            }
        }
    }
    public static void GetMapMinIndices(float[,] Elevation, out int xMinIndex, out int yMinIndex)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float maxVal = float.PositiveInfinity;
        xMinIndex = 0;
        yMinIndex = 0;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] < maxVal)
                {
                    maxVal = Elevation[x, y];
                    xMinIndex = x;
                    yMinIndex = y;
                }
            }
        }
    }

    public static void TransformMap(ref float[,] Elevation, dTransform dFunc, float c, Benchmark bench = null)
    {
        if (bench != null)
        {
            bench.StartBenchmark("TransformMap:" + dFunc.ToString());
        }
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float[,] dElev = new float[xDim, yDim];
        float f;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                f = Elevation[x, y];
                f = dFunc(f, c);
                dElev[x, y] = f;
            }
        }
        Elevation = dElev;
        if (bench != null)
        {
            bench.EndBenchmark("TransformMap:" + dFunc.ToString());
        }
    }
    public delegate float dTransform(float f, float c);
    public static float dInvert(float f, float c)
    {
        return 1.0f - f;
    }
    public static float dExponentiate(float f, float c)
    {
        return (float)Math.Pow(f, c);
    }

    public static void TransformMapSpecialNormalize(ref float[,] Elevation, float[,] WaterFlux)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float[,] dElev = new float[xDim, yDim];
        float f;
        float minVal, maxVal;
        GetMapMaxMinValues(Elevation, out maxVal, out minVal);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                f = Elevation[x, y];
                f = dNormalize(f, minVal, maxVal);
                if (WaterFlux[x, y] > 0.0f)
                {
                    dElev[x, y] = f;
                }
            }
        }
        Elevation = dElev;
    }

    public static void TransformMapMinMax(ref float[,] Elevation, dTransformMinMax dFunc, Benchmark bm = null)
    {
        if (!(bm == null))
        {
            bm.StartBenchmark("TransformMapMinMax");
        }
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float[,] dElev = new float[xDim, yDim];
        float f;
        float minVal, maxVal;
        GetMapMaxMinValues(Elevation, out maxVal, out minVal);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                f = Elevation[x, y];
                f = dFunc(f, minVal, maxVal);
                dElev[x, y] = f;
            }
        }
        Elevation = dElev;
        if (!(bm == null))
        {
            bm.EndBenchmark("TransformMapMinMax");
        }
    }
    public delegate float dTransformMinMax(float f, float fMin, float fMax, float newMinVal = 0f, float newMaxVal = 1f);
    public static float dNormalize(float f, float oldMinVal, float oldMaxVal, float newMinVal = 0f, float newMaxVal = 1f)
    {
        return (((f - oldMinVal) * (newMaxVal - newMinVal)) / (oldMaxVal - oldMinVal)) + newMinVal;
    }

    public static void TransformMapWithMap(ref float[,] Elevation, dTransformWithMap dFunc)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float f;
        float minVal, maxVal;
        GetMapMaxMinValues(Elevation, out maxVal, out minVal);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                f = dFunc(x, y, Elevation);
                Elevation[x, y] = f;
            }
        }
    }
    public delegate float dTransformWithMap(int xIndex, int yIndex, float[,] Elevation);
    public static float dSmooth(int xIndex, int yIndex, float[,] Elevation)
    {
        float coeff = 0.5f; // value between 0 and 1; 0 doesnt change anything
        float nAvg = GetNeighborAverage(Elevation, new MapLoc(xIndex, yIndex));
        float r = coeff * nAvg + (1 - coeff) * Elevation[xIndex, yIndex];
        return r;
    }
    public static float dSmoothKeep1(int xIndex, int yIndex, float[,] Elevation)
    {
        float coeff = 0.5f; // value between 0 and 1; 0 doesnt change anything
        float nAvg = GetNeighborAverage(Elevation, new MapLoc(xIndex, yIndex));
        float r = coeff * nAvg + (1 - coeff) * Elevation[xIndex, yIndex];
        r = Elevation[xIndex, yIndex] == 1f ? 1f : r;
        return Math.Max(r, Elevation[xIndex, yIndex]);
    }
    public static void TransformMapWithMapLoop(ref float[,] Elevation, int iterations, dTransformWithMap dFunc)
    {
        for (int i = 0; i < iterations; i++)
        {
            TransformMapWithMap(ref Elevation, dFunc);
        }
    }

    public static void TransformEqualizeMapByLevel(ref float seaLevel, ref float[,] Elevation, float target, Benchmark bm = null, float tolerance = 0.01f, bool equalizeByExponentiation = false)
    {
        if (!(bm == null))
        {
            bm.StartBenchmark("Equalize Level to " + target);
        }
        float percentAbove = GetPercentAbove(Elevation, seaLevel);
        int iter = 0;
        while (Math.Abs(percentAbove - target) > tolerance && iter < 10000)
        {
            if (percentAbove > target + tolerance)
            {
                if (seaLevel > 0.0f)
                {
                    if (equalizeByExponentiation)
                    {
                        TransformMap(ref Elevation, dExponentiate, 2f);
                    }
                    else
                    {
                        seaLevel += tolerance / 2f;
                    }
                }
                else
                {
                    break;
                }
            }
            if (percentAbove < target - tolerance)
            {
                if (seaLevel < 1.0f)
                {
                    if (equalizeByExponentiation)
                    {
                        TransformMap(ref Elevation, dExponentiate, 0.5f);

                    }
                    else
                    {
                        seaLevel -= tolerance / 2f;
                    }
                }
                else
                {
                    break;
                }
            }
            percentAbove = GetPercentAbove(Elevation, seaLevel);
            iter++;
        }
        if (!(bm == null))
        {
            bm.EndBenchmark("Equalize Level to " + target);
        }
    }
    public static void TransformEqualizeMapByLevelAboveSea(ref float riverLevel, ref float[,] WaterFlux, float target, float[,] Elevation, float seaLevel, float tolerance = 0.01f, bool equalizeByExponentiation = false)
    {
        float percentAbove = GetPercentAboveSeaLevel(WaterFlux, riverLevel, Elevation, seaLevel);
        int iter = 0;
        while (Math.Abs(percentAbove - target) > tolerance && iter < 1000)
        {
            //Debug.Log("Percent Above SeaLevel is " + percentAbove);
            if (percentAbove > target + tolerance)
            {
                if (riverLevel < 1.0f)
                {
                    if (equalizeByExponentiation)
                    {
                        TransformMap(ref WaterFlux, dExponentiate, 2f);
                    }
                    else
                    {
                        riverLevel += tolerance;
                    }
                }
                else
                {
                    break;
                }
            }
            if (percentAbove < target - tolerance)
            {
                if (riverLevel > 0f)
                {
                    if (equalizeByExponentiation)
                    {
                        TransformMap(ref WaterFlux, dExponentiate, 0.5f);
                    }
                    else
                    {
                        riverLevel -= tolerance;
                    }
                }
                else
                {
                    break;
                }
            }
            percentAbove = GetPercentAboveSeaLevel(WaterFlux, riverLevel, Elevation, seaLevel);
            iter++;
        }
    }
    public static float GetPercentAboveSeaLevel(float[,] WaterFlux, float thresh, float[,] Elevation, float seaLevel)
    {
        List<float> listAbove = new List<float>();
        int xDim = WaterFlux.GetLength(0);
        int yDim = WaterFlux.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] > seaLevel)
                {
                    if (WaterFlux[x, y] > thresh)
                    {
                        listAbove.Add(1.0f);
                    }
                    else
                    {
                        listAbove.Add(0.0f);
                    }
                }
            }
        }
        float avg = listAbove.Count > 0 ? listAbove.Average() : 0f;
        return avg;
    }
    public static int OceanNeighbors(MapLoc l, float[,] Elevation, float seaLevel)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(Elevation, l);
        int numberOceanNeighbors = 0;
        foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
        {
            if (kvp.Value < seaLevel)
            {
                numberOceanNeighbors++;
            }
        }
        return numberOceanNeighbors;
    }
    public static int RiverNeighbors(MapLoc l, float[,] WaterFlux, float riverLevel)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(WaterFlux, l);
        int numberRiverNeighbors = 0;
        foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
        {
            if (kvp.Value > riverLevel)
            {
                numberRiverNeighbors++;
            }
        }
        return numberRiverNeighbors;
    }
    public static List<MapLoc> GetCoastMapLocs(float[,] Elevation, float seaLevel)
    {
        List<MapLoc> coast = new List<MapLoc>();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] > seaLevel)
                {
                    MapLoc l = new MapLoc(x, y);
                    int numberOfOceanNeighbors = OceanNeighbors(l, Elevation, seaLevel);
                    if (numberOfOceanNeighbors > 0)
                    {
                        coast.Add(l);
                    }
                }
            }
        }
        return coast;
    }

    public static bool IsBorder(MapLoc l, float[,] Elevation)
    {
        int x = l.x;
        int y = l.y;
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);

        bool isBorder = (x == 0 || y == 0 || x == xDim - 1 || y == yDim - 1) ? true : false;
        return isBorder;
    }
    public static float GetMapBorderAverage(float[,] Elevation)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        List<float> listBorder = new List<float>();
        MapLoc l;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                l = new MapLoc(x, y);
                if (IsBorder(l, Elevation))
                {
                    listBorder.Add(Elevation[x, y]);
                }
            }
        }
        return listBorder.Average();
    }
    public static float GetMapMedian(float[,] Elevation)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        List<float> listValues = new List<float>();
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                listValues.Add(Elevation[x, y]);
            }
        }
        return GetMedian(listValues); // change it to median
    }
    public static float GetMedian(List<float> x)
    {
        x.Sort();
        int i = (x.Count - 1) / 2;
        return x[i];
    }
    public static float GetPercentAbove(float[,] Elevation, float thresh)
    {
        List<float> listAbove = new List<float>();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (Elevation[x, y] > thresh)
                {
                    listAbove.Add(1.0f);
                }
                else
                {
                    listAbove.Add(0.0f);
                }
            }
        }
        return listAbove.Average();
    }
    public static float GetSlope(float[,] Elevation, MapLoc l)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(Elevation, l); // Get Slope
        float maxDiff = float.NegativeInfinity;
        float MapLocalDiff;
        foreach (KeyValuePair<MapLoc, float> n in neighbors)
        {
            MapLocalDiff = Math.Abs(Elevation[l.x, l.y] - Elevation[n.Key.x, n.Key.y]);
            if (MapLocalDiff > maxDiff)
            {
                maxDiff = MapLocalDiff;
            }
        }
        return maxDiff;
    }
    public static float[,] GetSlopeMap(float[,] Elevation)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        float[,] Slopes = new float[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Slopes[x, y] = GetSlope(Elevation, new MapLoc(x, y));
            }
        }
        return Slopes;
    }

    public static void GetCoordinates(int xDim, int yDim, int _X, int _Y, ref int X, ref int Y, bool Wrapped = true)
    {
        if (Wrapped)
        {
            X = (1000 * xDim + _X) % xDim;
            Y = (1000 * yDim + _Y) % yDim;
        }
        else
        {
            X = _X;
            Y = _Y;
        }
    }
    public static void GetCoordinates(int xDim, int yDim, float _X, float _Y, ref float X, ref float Y, bool Wrapped = true)
    {
        if (Wrapped)
        {
            X = (1000 * xDim + _X) % xDim;
            Y = (1000 * yDim + _Y) % yDim;
        }
        else
        {
            X = _X;
            Y = _Y;
        }
    }
    public static Dictionary<MapLoc, float> GetValidNeighbors(float[,] Elevation, MapLoc l, bool excludeSelf = true, bool includeDiagonal = false, bool Wrap = true)
    {
        Dictionary<MapLoc, float> neighbors = new Dictionary<MapLoc, float>();
        int x = 0;
        int y = 0;
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (excludeSelf && i == 0 && j == 0)
                {

                }
                else
                {
                    GetCoordinates(xDim, yDim, l.x + i, l.y + j, ref x, ref y, Wrap);
                    if (x >= 0 && y >= 0 && x < xDim && y < yDim)
                    {
                        if (includeDiagonal || Math.Sqrt(i * i + j * j) <= 1)
                        {
                            neighbors[new MapLoc(x, y)] = Elevation[x, y];
                        }
                    }
                }
            }
        }
        return neighbors;
    }
    public static MapLoc GetLowestNeighbor(float[,] Elevation, MapLoc l, bool excludeSelf)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(Elevation, l, excludeSelf); // Get Lowest Neighbor
        MapLoc minMapLoc = new MapLoc(0, 0);
        float minVal = float.PositiveInfinity;
        foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
        {
            if (kvp.Value < minVal)
            {
                minVal = kvp.Value;
                minMapLoc = kvp.Key;
            }
        }
        return minMapLoc;
    }
    public static MapLoc GetHighestNeighbor(float[,] Elevation, MapLoc l, bool includeSelf)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(Elevation, l, includeSelf); // Get Highest Neighbor
        MapLoc maxMapLoc = new MapLoc(0, 0);
        float maxVal = float.NegativeInfinity;
        foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
        {
            if (kvp.Value > maxVal)
            {
                maxVal = kvp.Value;
                maxMapLoc = kvp.Key;
            }
        }
        return maxMapLoc;
    }
    public static float GetNeighborAverage(float[,] Elevation, MapLoc l)
    {
        Dictionary<MapLoc, float> neighbors = GetValidNeighbors(Elevation, l); // GetNeighborAversge
        List<float> NeighborHeights = new List<float>();
        foreach (KeyValuePair<MapLoc, float> kvp in neighbors)
        {
            NeighborHeights.Add(kvp.Value);
        }
        return NeighborHeights.Average();
    }
    public static MapLoc[,] GetDownhillMap(float[,] Elevation)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        MapLoc[,] Downhill = new MapLoc[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                MapLoc ln = GetLowestNeighbor(Elevation, new MapLoc(x, y), false);
                Downhill[x, y] = ln;
                //Debug.Log("Lowest Neighbor of [" + x + "," + y + "] at " + Elevation[x, y] + " is [" + ln.x + "," + ln.y + "] at " + Elevation[ln.x, ln.y]);
            }
        }
        return Downhill;
    }
    public static Vec[,] GetDownhillVectors(float[,] Elevation)
    {
        MapLoc[,] Downhills = GetDownhillMap(Elevation);
        int xDim = Downhills.GetLength(0);
        int yDim = Downhills.GetLength(1);
        Vec[,] dhVectors = new Vec[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if (x == Downhills[x, y].x && y == Downhills[x, y].y)
                {

                }
                else
                {
                    Vec v = VectorBetween(new MapLoc(x, y), Downhills[x, y]);
                    dhVectors[x, y] = v;
                    //Debug.Log("Vector at [" + x + "," + y + "] is [" + v.x + "," + v.y + "]");
                }
            }
        }
        return dhVectors;
    }
    public static Vec VectorBetween(MapLoc l1, MapLoc l2)
    {
        float x = l2.x - l1.x;
        float y = l1.y - l2.y;
        return new Vec(x, y);
    }
    public static List<MapLoc> GetMapLocsDescending(float[,] Elevation, Benchmark bench = null)
    {
        if (bench != null)
        {
            bench.StartBenchmark("GetMapLocsDescending");
        }
        List<MapLoc> dMapLocs = new List<MapLoc>();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                MapLoc l = new MapLoc(x, y);
                l.v = Elevation[x, y];
                dMapLocs.Add(l);
            }
        }
        dMapLocs.OrderBy(l => l.v);
        dMapLocs.Reverse();

        if (bench != null)
        {
            bench.EndBenchmark("GetMapLocsDescending");
        }
        return dMapLocs;
    }

    public static void TransformApplyPreferencesWaterBody(ref float[,] Elevation, float seaLevel, WaterBodyPrefence prefWaterBody, Benchmark bench = null)
    {
        if (!(bench == null))
        {
            bench.StartBenchmark("Water Bodies");
        }
        if (prefWaterBody == WaterBodyPrefence.Islands)
        {
            if (GetMapBorderAverage(Elevation) > 0.5f)
            {
                TransformMap(ref Elevation, dInvert, 0f);
            }
        }
        else if (prefWaterBody == WaterBodyPrefence.Continent)
        {
            AddMaskIsland(ref Elevation, 0.1f);
            if (GetMapBorderAverage(Elevation) > 0.5f)
            {
                TransformMap(ref Elevation, dInvert, 0f);
            }
        }
        else if (prefWaterBody == WaterBodyPrefence.Lakes)
        {
            if (GetMapBorderAverage(Elevation) < 0.5f)
            {
                TransformMap(ref Elevation, dInvert, 0f);
            }
        }
        else if (prefWaterBody == WaterBodyPrefence.Coast)
        {
            AddMaskCoast(ref Elevation);
            if (GetMapBorderAverage(Elevation) > 0.5f)
            {
                TransformMap(ref Elevation, dInvert, 0f);
            }
        }
        if (!(bench == null))
        {
            bench.EndBenchmark("Water Bodies");
        }

        TransformMapMinMax(ref Elevation, dNormalize);
        TransformEqualizeMapByLevel(ref seaLevel, ref Elevation, 0.5f, bench);
    }
    public static void TransformResolveDepressions(ref float[,] Elevation, int maxIter = 1000, Benchmark bench = null, bool normalize = true)
    {
        if (!(bench == null))
        {
            bench.StartBenchmark("Resolve Depression" + maxIter);
        }
        List<MapLoc> depressedMapLocs = GetListOfDepressedMapLocs(Elevation);
        int iter = 0;
        while (depressedMapLocs.Count > 0 && iter < maxIter)
        {
            SortMapLocs(ref depressedMapLocs, Elevation);
            depressedMapLocs.Reverse();
            foreach (MapLoc l in depressedMapLocs)
            {
                Elevation[l.x, l.y] = GetNeighborAverage(Elevation, l);
            }

            depressedMapLocs = GetListOfDepressedMapLocs(Elevation);
            iter++;
        }
        if (!(bench == null))
        {
            bench.EndBenchmark("Resolve Depression" + maxIter);
        }
        if (normalize)
        {
            TransformMapMinMax(ref Elevation, dNormalize, bench);
        }
    }
    public static List<MapLoc> GetListOfDepressedMapLocs(float[,] Elevation)
    {
        List<MapLoc> depressedMapLocs = new List<MapLoc>();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);

        MapLoc l;
        float e;
        bool lIsDepressed;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                l = new MapLoc(x, y);
                e = Elevation[x, y];
                lIsDepressed = true;
                Dictionary<MapLoc, float> lNeighbors = GetValidNeighbors(Elevation, l); // Get Depressed MapLocs
                foreach (KeyValuePair<MapLoc, float> n in lNeighbors)
                {
                    if (n.Value <= e)
                    {
                        lIsDepressed = false;
                        break;
                    }
                }
                if (lIsDepressed)
                {
                    depressedMapLocs.Add(l);
                }
            }
        }

        return depressedMapLocs;
    }
    public static void SortMapLocs(ref List<MapLoc> MapLocs, float[,] Elevation)
    {
        MapLocs.OrderBy(p => Elevation[p.x, p.y]);
    }

    public static List<MapLoc> GetCircleMapLocs(float[,] Elevation, MapLoc center, float radius)
    {
        List<MapLoc> circleMapLocs = new List<MapLoc>();
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                float d = Distance(center, new MapLoc(x, y));
                if (d < radius)
                {
                    circleMapLocs.Add(new MapLoc(x, y));
                }
            }
        }
        return circleMapLocs;
    }
    public static void AddMaskCircle(ref float[,] Elevation, MapLoc center, float radius, float height)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                float d = Distance(center, new MapLoc(x, y));
                if (d < radius)
                {
                    Elevation[x, y] += height * (float)(1 - Math.Pow(d / radius, 2));
                }
            }
        }
        TransformMapMinMax(ref Elevation, dNormalize);
    }
    public static void AddMaskIsland(ref float[,] Elevation, float height = 0.5f)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        int xi = (xDim - 1) / 2;
        int yi = (yDim - 1) / 2;
        int ri = (int)Math.Min(xi, yi);
        AddMaskCircle(ref Elevation, new MapLoc(xi, yi), ri, height);
    }
    public static void AddMaskCoast(ref float[,] Elevation, float height = 0.5f)
    {
        int xDim = Elevation.GetLength(0);
        int yDim = Elevation.GetLength(1);
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Elevation[x, y] += height * x / xDim;
            }
        }
        TransformMapMinMax(ref Elevation, dNormalize);
    }

    public static float Distance(MapLoc l1, MapLoc l2)
    {
        float d = (float)Math.Sqrt(Math.Pow(l1.x - l2.x, 2) + Math.Pow(l1.y - l2.y, 2));
        return d;
    }
    public static int nFromDims(int xDim, int yDim)
    {
        int N = 1;
        int resultDim = (int)Mathf.Pow(2, N) + 1;
        int maxDim = Mathf.Max(xDim, yDim);
        while (resultDim < maxDim)
        {
            N++;
            resultDim = (int)Mathf.Pow(2, N) + 1;
        }
        return N;
    }
    public static float DegreesToRadians(float degrees)
    {
        return (float)(degrees * (Math.PI / 180));
    }
    public static float RadiansToDegrees(float radians)
    {
        return (float)(radians * (180 / Math.PI));
    }
    public static float VectorToRadians(float x, float y)
    {
        return (float)Math.Atan2(y, x);
    }

    public static void PrintElevationToDebug(float[,] Elevation)
    {
        int Dim = Elevation.GetLength(0);
        string sLine;
        for (int y = 0; y < Dim; y++)
        {
            sLine = "";
            for (int x = 0; x < Dim; x++)
            {
                sLine += Math.Round(Elevation[x, y], 2).ToString() + ",";
            }
            sLine = sLine.TrimEnd(',');
            Debug.Log(sLine);
        }
    }


}


public class MidpointDisplacement
{

    public int N, xDim, yDim, Dim, featureSize;
    public float RandomInfluence;
    public float[,] Elevation;
    bool wrapEastWest, wrapNorthSouth;

    public MidpointDisplacement(int N, bool _wrapEastWest = false, bool _wrapNorthSouth = false)
    {
        Dim = (int)Mathf.Pow(2, N) + 1;         // This indicates the height and width of the map
        xDim = Dim;
        yDim = xDim;
        RandomInfluence = 1.0f;                    // R should be between 0.0 and 1.0
        featureSize = xDim;                        // This indicates the intricacy of detail on the map
        Elevation = new float[xDim, yDim];        // Most functions implicitly refer to this array
        wrapEastWest = _wrapEastWest;
        wrapNorthSouth = _wrapNorthSouth;
        BuildMap();                             // The constructor calls this function to build the map
    }

    public void BuildMap()
    {
        //RandomizePoints(featureSize);
        int sampleSize = featureSize;
        float scale = RandomInfluence;
        while (sampleSize > 1)
        {
            DiamondSquare(sampleSize, scale);
            sampleSize /= 2;
            scale /= 2f;
        }
    }

    public void DiamondSquare(int stepsize, float scale)
    {
        int halfstep = stepsize / 2;
        //Debug.Log("Diamond Square on stepsize = " + stepsize + " and scale = " + scale + " and halfstep = " + halfstep);

        for (int x = halfstep; x < xDim - halfstep; x += stepsize)
        {
            for (int y = halfstep; y < yDim - halfstep; y += stepsize)
            {
                SquareStep(x, y, stepsize, GetRandom(scale));
            }
        }
        for (int x = halfstep; x < xDim - halfstep; x += stepsize)
        {
            for (int y = halfstep; y < yDim - halfstep; y += stepsize)
            {
                DiamondStep(x + halfstep, y, stepsize, GetRandom(scale));
                DiamondStep(x, y + halfstep, stepsize, GetRandom(scale));
            }
        }
    }
    public void SquareStep(int x, int y, int size, float randomValue)
    {
        //Debug.Log("Square Step on [" + x + "," + y + "]");
        int hs = size / 2;

        // a     b 
        //
        //    x
        //
        // c     d

        List<float> validPoints = new List<float>();
        try
        {
            validPoints.Add(GetValueAt(x - hs, y - hs));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x + hs, y - hs));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x - hs, y + hs));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x + hs, y + hs));
        }
        catch { }
        float midpointDisplacedValue = validPoints.Average() + randomValue;

        SetValue(x, y, midpointDisplacedValue);
    }
    public void DiamondStep(int x, int y, int size, float randomValue)
    {
        //Debug.Log("Diamond Step on [" + x + "," + y + "]");
        int hs = size / 2;

        //   c
        //
        //a  x  b
        //
        //   d
        List<float> validPoints = new List<float>();
        try
        {
            validPoints.Add(GetValueAt(x - hs, y));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x + hs, y));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x, y - hs));
        }
        catch { }
        try
        {
            validPoints.Add(GetValueAt(x, y + hs));
        }
        catch { }


        float midpointDisplacedValue = validPoints.Average() + randomValue;

        SetValue(x, y, midpointDisplacedValue);
    }

    public void GetCoordinates(int xDim, int yDim, int _X, int _Y, out int X, out int Y)
    {
        X = wrapEastWest ? (_X + xDim) % xDim : _X;
        Y = wrapNorthSouth ? (_Y + yDim) % yDim : _Y;
        //X = _X % xDim;
        //Y = _Y % yDim;
    }
    public float GetValueAt(int _X, int _Y)
    {
        int X, Y;
        GetCoordinates(xDim, yDim, _X, _Y, out X, out Y);
        return Elevation[X, Y];
    }
    public void SetValue(int _X, int _Y, float v)
    {
        int X, Y;
        GetCoordinates(xDim, yDim, _X, _Y, out X, out Y);
        Elevation[X, Y] = v;
        //Debug.Log("Setting [" + X + "," + Y + "] to " + v);
    }
    public static float GetRandom(float coeff)
    {
        float r = (UnityEngine.Random.Range(0, 100) - 50) / 100f;
        return r * coeff;
    }

}



public class MapPainter
{

    List<Node> nodelist = new List<Node>();
    Dictionary<MapLoc, Node> nodedict = new Dictionary<MapLoc, Node>();

    Dictionary<int, List<MapLoc>> areas = new Dictionary<int, List<MapLoc>>();
    List<MapLoc>[] landbodies;
    List<MapLoc>[] waterbodies;
    int xDim, yDim;
    float[,] elevation;
    float seaLevel;

    public MapPainter(float[,] _elevation, float _seaLevel)
    {
        nodelist = new List<Node>();
        elevation = _elevation;
        seaLevel = _seaLevel;
        xDim = _elevation.GetLength(0);
        yDim = _elevation.GetLength(1);
        Node[,] nodes = new Node[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                nodes[x, y] = new Node(x, y);
            }
        }
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Node n = nodes[x, y];
                n.Visited = false;
                MapLoc l = new MapLoc(x, y);
                Dictionary<MapLoc, float> d = MapUtil.GetValidNeighbors(_elevation, l, Wrap: false);
                foreach (KeyValuePair<MapLoc, float> kvp in d)
                {
                    if ((_elevation[x, y] > _seaLevel && _elevation[kvp.Key.x, kvp.Key.y] > _seaLevel)
                        || (_elevation[x, y] < _seaLevel && _elevation[kvp.Key.x, kvp.Key.y] < _seaLevel))
                    {
                        n.neighborWeights[nodes[kvp.Key.x, kvp.Key.y]] = MapUtil.Distance(kvp.Key, l);
                        n.neighbors.Add(nodes[kvp.Key.x, kvp.Key.y]);
                    }
                }
                nodelist.Add(n);
                nodedict[n.loc] = n;
            }
        }
        Paint();
    }

    public void Paint()
    {
        int iColor = 0;
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                MapLoc l = new MapLoc(x, y);
                Node n = nodedict[l];
                iColor = n.Visited ? iColor : iColor + 1;
                AssignColorToNodeAndNeighbors(n, iColor);
            }
        }
        BuildAreasDictionaries();
    }
    public void AssignColorToNodeAndNeighbors(Node n, int iColor)
    {
        if (!n.Visited)
        {
            n.Visited = true;
            n.iRegion = iColor;
            foreach (Node neighbor in n.neighbors)
            {
                AssignColorToNodeAndNeighbors(neighbor, iColor);
            }
        }
    }

    public void BuildAreasDictionaries()
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                MapLoc l = new MapLoc(x, y);
                Node n = nodedict[l];
                int iColor = n.iRegion;
                if (!areas.ContainsKey(iColor))
                {
                    areas[iColor] = new List<MapLoc>();
                }
                areas[iColor].Add(l);
            }
        }
        List<List<MapLoc>> waterBodies = new List<List<MapLoc>>();
        List<List<MapLoc>> landBodies = new List<List<MapLoc>>();
        foreach (KeyValuePair<int, List<MapLoc>> kvp in areas)
        {
            int iColor = kvp.Key;
            List<MapLoc> locsInArea = kvp.Value;
            MapLoc l0 = locsInArea[0];
            float e = elevation[l0.x, l0.y];
            if (e > seaLevel)
            {
                landBodies.Add(locsInArea);
            }
            else
            {
                waterBodies.Add(locsInArea);
            }
        }
        waterbodies = waterBodies.ToArray();
        landbodies = landBodies.ToArray();
    }
    public float[,] BuildRegions()
    {
        float[,] regions = new float[xDim, yDim];
        foreach (Node n in nodelist)
        {
            regions[n.loc.x, n.loc.y] = elevation[n.loc.x, n.loc.y] < seaLevel ? 0f : (float)n.iRegion;
        }
        return regions;
    }
}


public class Benchmark
{
    Dictionary<string, List<DateTime>> startTimes;
    Dictionary<string, List<DateTime>> endTimes;
    Dictionary<string, TimeSpan> averageTime;
    Dictionary<string, TimeSpan> totalTime;
    Dictionary<string, int> numberOfBenchmarks;
    string BenchmarkFamilyName;
    int PadN = 30;
    public Benchmark(string _BenchmarkFamilyName)
    {
        BenchmarkFamilyName = _BenchmarkFamilyName;
        startTimes = new Dictionary<string, List<DateTime>>();
        endTimes = new Dictionary<string, List<DateTime>>();
        averageTime = new Dictionary<string, TimeSpan>();
        totalTime = new Dictionary<string, TimeSpan>();
        numberOfBenchmarks = new Dictionary<string, int>();
    }
    public static void Start(Benchmark b, string s)
    {
        if (b != null)
        {
            b.StartBenchmark(s);
        }
    }
    public static void End(Benchmark b, string s)
    {
        if (b != null)
        {
            b.EndBenchmark(s);
        }
    }
    public static void Write(Benchmark b, string s)
    {
        if (b != null)
        {
            b.WriteBenchmarkToDebug();
        }
    }
    public void StartBenchmark(string MarkName)
    {
        if (!startTimes.ContainsKey(MarkName))
        {
            startTimes[MarkName] = new List<DateTime>();
        }
        startTimes[MarkName].Add(DateTime.Now);
    }
    public void EndBenchmark(string MarkName)
    {
        if (!endTimes.ContainsKey(MarkName))
        {
            endTimes[MarkName] = new List<DateTime>();
        }
        endTimes[MarkName].Add(DateTime.Now);
    }
    private void TabulateBenchmarks()
    {
        foreach (KeyValuePair<string, List<DateTime>> kvp in startTimes)
        {
            int NumberOfBenchmarks = 0;
            string keyString = kvp.Key;
            //Debug.Log("Tabulating " + keyString);
            totalTime[keyString] = TimeSpan.Zero;
            for (int i = 0; i < kvp.Value.Count(); i++)
            {
                TimeSpan elapsedTime = endTimes[keyString][i] - startTimes[keyString][i];
                totalTime[keyString] += elapsedTime;
                NumberOfBenchmarks++;
            }
            averageTime[keyString] = new TimeSpan(totalTime[keyString].Ticks / NumberOfBenchmarks);
            numberOfBenchmarks[keyString] = NumberOfBenchmarks;
        }
    }
    private List<string> BenchmarkText()
    {
        TabulateBenchmarks();
        List<string> BenchLines = new List<string>();
        string LineString = new string('-', PadN - 5);
        string[] titles = new string[] { "Benchmark Family", "Benchmark Name", "Total Time", "Average Time", "Number" };
        string titleString = "";
        string headerString = "";
        foreach (string title in titles)
        {
            titleString += title.PadRight(PadN);
            headerString += LineString.PadRight(PadN);
        }
        BenchLines.Add("Current Time:" + DateTime.Now.ToString());
        BenchLines.Add(titleString);
        BenchLines.Add(headerString);
        foreach (KeyValuePair<string, TimeSpan> kvp in totalTime)
        {
            string k = kvp.Key;
            string totalTimeString = FormatTimespan(totalTime[k]);
            string averageTimeString = FormatTimespan(averageTime[k]);
            string MiddleString = BenchmarkFamilyName.PadRight(PadN);
            MiddleString += k.PadRight(PadN);
            MiddleString += totalTimeString.PadRight(PadN);
            MiddleString += averageTimeString.PadRight(PadN);
            MiddleString += numberOfBenchmarks[k].ToString().PadRight(PadN);
            BenchLines.Add(MiddleString);
        }
        BenchLines.Add(headerString);
        BenchLines.Add("");
        return BenchLines;
    }
    public void WriteBenchmarkToDebug(bool run = false)
    {
        if (run)
        {
            List<string> tlines = BenchmarkText();
            foreach (string t in tlines)
            {
                Debug.Log(t);
            }
        }
    }
    public string FormatTimespan(TimeSpan ts)
    {
        return ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + ts.Seconds.ToString() + ":" + ts.Milliseconds.ToString();
    }
}

public class Node
{
    public MapLoc loc;
    public int x, y;
    public Dictionary<Node, float> neighborWeights;
    public List<Node> neighbors;
    public Dictionary<MapLoc, float> distanceTo;
    public Dictionary<MapLoc, float> distanceFrom;
    public Dictionary<MapLoc, Node> NearestTo;
    public Dictionary<PathOD, float> distanceBetween;
    public bool Visited;
    public int iRegion;
    public Node(int _x, int _y)
    {
        x = _x;
        y = _y;
        loc = new MapLoc(_x, _y);
        distanceTo = new Dictionary<MapLoc, float>();
        distanceFrom = new Dictionary<MapLoc, float>();
        distanceBetween = new Dictionary<PathOD, float>();
        neighbors = new List<Node>();
        NearestTo = new Dictionary<MapLoc, Node>();
        neighborWeights = new Dictionary<Node, float>();
    }
}

public struct PathOD
{
    public MapLoc origin, destination;

    public PathOD(MapLoc _origin, MapLoc _destination)
    {
        origin = _origin;
        destination = _destination;
    }
}

public struct Path
{
    public MapLoc originMapLoc, destinationMapLoc;
    public float distancePath, distanceEuclidean;
    public List<Node> path;
    public Path(MapLoc _origin, MapLoc _destination, List<Node> _path, float _distancePath, float _distanceEuclidean)
    {
        originMapLoc = _origin;
        destinationMapLoc = _destination;
        distancePath = _distancePath;
        distanceEuclidean = _distanceEuclidean;
        path = _path;
    }

}

public class MapPathfinder
{
    List<Node> nodelist = new List<Node>();
    public Dictionary<MapLoc, Node> nodeDict = new Dictionary<MapLoc, Node>();
    public Dictionary<PathOD, Path> PathDict = new Dictionary<PathOD, Path>();
    public MapPathfinder()
    {

    }
    public void SetNodeList(float[,] _elevation, float _seaLevel, delCost fCost, float _seaTravelCost = 0f)
    {
        int xDim = _elevation.GetLength(0);
        int yDim = _elevation.GetLength(1);
        Node[,] nodes = new Node[xDim, yDim];
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                nodes[x, y] = new Node(x, y);
                nodeDict[new MapLoc(x, y)] = nodes[x, y];
            }
        }
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                Node n = nodes[x, y];
                n.Visited = false;
                MapLoc l = new MapLoc(x, y);
                Dictionary<MapLoc, float> d = MapUtil.GetValidNeighbors(_elevation, l);
                foreach (KeyValuePair<MapLoc, float> kvp in d)
                {
                    n.neighborWeights[nodes[kvp.Key.x, kvp.Key.y]] = fCost(l, kvp.Key, _elevation, _seaLevel, _seaTravelCost);
                    n.neighbors.Add(nodes[kvp.Key.x, kvp.Key.y]);
                }
                nodelist.Add(n);
            }
        }
    }
    public void SetNodeList(List<Node> _nodes)
    {
        nodelist = _nodes;
        foreach (Node n in nodelist)
        {
            nodeDict[n.loc] = n;
        }
    }
    public Path GetPath(MapLoc origin, MapLoc destination)
    {
        PathOD p = new PathOD(origin, destination);
        bool pathFound = false;
        if (!PathDict.ContainsKey(p))
        {
            pathFound = AStarSearch(origin, destination);
        }
        return PathDict[p];
    }
    private bool AStarSearch(MapLoc origin, MapLoc destination)
    {
        List<Node> prioQueue;
        bool PathFound = false;
        AStarInit(out prioQueue, origin, destination);
        while (prioQueue.Any())
        {
            AStarWorkhorse(ref prioQueue, ref PathFound, origin, destination);
        }
        if (PathFound)
        {
            AddPath(origin, destination);
        }
        return PathFound;
    }
    private void AStarInit(out List<Node> prioQueue, MapLoc originMapLoc, MapLoc destinationMapLoc)
    {
        foreach (Node n in nodelist)
        {
            n.distanceTo[destinationMapLoc] = MapUtil.Distance(n.loc, destinationMapLoc);
        }
        nodeDict[originMapLoc].distanceFrom[originMapLoc] = 0f;
        prioQueue = new List<Node>();
        prioQueue.Add(nodeDict[originMapLoc]);
    }
    private void AStarWorkhorse(ref List<Node> prioQueue, ref bool PathFound, MapLoc originMapLoc, MapLoc destinationMapLoc)
    {
        prioQueue = prioQueue.OrderBy(x => x.distanceFrom[originMapLoc] + x.distanceTo[destinationMapLoc]).ToList();
        Node n = prioQueue.First();
        prioQueue.Remove(n);
        foreach (Node neighbor in n.neighbors)
        {
            if (!neighbor.Visited)
            {
                float newCost = n.distanceFrom[originMapLoc] + n.neighborWeights[neighbor];
                if (!neighbor.distanceFrom.ContainsKey(originMapLoc) || newCost < neighbor.distanceFrom[originMapLoc]) // this will cause problems if there is ever no cost between nodes!
                {
                    neighbor.distanceFrom[originMapLoc] = newCost;
                    neighbor.NearestTo[originMapLoc] = n;
                    if (!prioQueue.Contains(neighbor))
                    {
                        prioQueue.Add(neighbor);
                    }
                }

            }
        }

        n.Visited = true;
        if (n.loc.Equals(destinationMapLoc))
        {
            PathFound = true;
        }
    }
    private void AddPath(MapLoc origin, MapLoc destination, bool addBetweenPaths = false)
    {
        float dP = 0f;
        List<Node> shortestPath = new List<Node>();
        Node thisNode = nodeDict[destination];
        shortestPath.Add(thisNode);
        while (!thisNode.loc.Equals(origin))
        {
            dP += thisNode.neighborWeights[thisNode.NearestTo[origin]];
            thisNode = thisNode.NearestTo[origin];
            shortestPath.Add(thisNode);
        }
        shortestPath.Reverse();
        float dE = MapUtil.Distance(origin, destination);
        Path p = new Path(origin, destination, shortestPath, dP, dE);
        PathDict[new PathOD(origin, destination)] = p;
        if (addBetweenPaths)
        {
            List<PathOD> betweenPaths = new List<PathOD>();
            int iWindow = shortestPath.Count - 2;
            while (iWindow > 0)
            {
                for (int i = 0; i < (shortestPath.Count - iWindow); i++)
                {
                    PathOD betweenPath = new PathOD(shortestPath[i].loc, shortestPath[i + iWindow].loc);
                    betweenPaths.Add(betweenPath);
                }
                iWindow--;
            }
            foreach (PathOD pOD in betweenPaths)
            {
                AddPath(pOD.origin, pOD.destination, false);
            }
        }
    }
    public delegate float delCost(MapLoc l, MapLoc n, float[,] _elevation, float _seaLevel, float _seaTravelCost);
    public static float CostStandard(MapLoc l, MapLoc n, float[,] _elevation, float _seaLevel, float _seaTravelCost)
    {
        float nelev = _elevation[n.x, n.y];
        float lelev = _elevation[l.x, l.y];
        float diff = (nelev > _seaLevel && lelev > _seaLevel) ? Math.Abs(nelev - lelev) : _seaTravelCost;
        float dist = MapUtil.Distance(n, l);
        diff *= dist;
        diff += dist;
        return diff;
    }
}

public static class MapColor
{
    static public Color GetColorDiscrete(float value, Dictionary<float, Color> cdict)
    {
        List<float> flevels = new List<float>();
        foreach (KeyValuePair<float, Color> kvp in cdict)
        {
            flevels.Add(kvp.Key);
        }
        flevels.Sort();
        Color rColor = cdict[flevels[0]];
        foreach (float flevel in flevels)
        {
            rColor = cdict[flevel];
            if (value < flevel)
            {
                break;
            }
        }
        return rColor;
    }
    static public Color GetColorContinuous(float value, Dictionary<float, Color> cdict)
    {
        List<float> flevels = new List<float>();
        foreach (KeyValuePair<float, Color> kvp in cdict)
        {
            flevels.Add(kvp.Key);
        }
        flevels.Sort();
        Color rColor = cdict[flevels[0]];
        float flerp;
        for (int i = 0; i < flevels.Count - 1; i++)
        {
            if (value >= flevels[i] && value <= flevels[i + 1])
            {
                flerp = MapUtil.dNormalize(value, flevels[i], flevels[i + 1]);
                rColor = GetColorLerp(flerp, cdict[flevels[i]], cdict[flevels[i + 1]]);
            }
        }
        return rColor;
    }
    static public Color GetColorLerp(float value, Color cLower, Color cUpper)
    {
        float r = Mathf.Lerp(cLower.r, cUpper.r, value);
        float g = Mathf.Lerp(cLower.g, cUpper.g, value);
        float b = Mathf.Lerp(cLower.b, cUpper.b, value);
        Color C = new Color(r, g, b);
        return C;
    }
    static public Color GetColorCutoff(float value, float cutoff, Color cLower, Color cUpper)
    {
        Dictionary<float, Color> CutoffDict = new Dictionary<float, Color>();
        CutoffDict[cutoff] = cLower;
        CutoffDict[1f] = cUpper;
        Color rColor = GetColorDiscrete(value, CutoffDict);
        return rColor;
    }

    static public Dictionary<float, Color> PaletteBasicSpectrum(float c)
    {
        Dictionary<float, Color> d = new Dictionary<float, Color>();
        d[0f] = Color.blue;
        d[0.2f] = Color.cyan;
        d[0.4f] = Color.green;
        d[0.6f] = Color.yellow;
        d[0.8f] = Color.red;
        d[1f] = Color.magenta;
        return d;
    }
    static public Dictionary<float, Color> PaletteBasicNatural(float c)
    {
        Dictionary<float, Color> d = new Dictionary<float, Color>();
        d[0f] = Color.blue;
        float sealerp = Mathf.Lerp(0f, c, 0.8f);
        d[sealerp] = Color.blue;
        d[c] = GetColorLerp(0.5f, Color.blue, Color.cyan);
        float shore = c + 0.00001f;
        d[shore] = Color.green;
        float landlerp = Mathf.Lerp(shore, 1f, 0.5f);
        d[landlerp] = Color.yellow;

        float mountain = Mathf.Lerp(shore, 1f, 0.85f);
        d[mountain] = Color.red;
        d[mountain + 0.00001f] = Color.red;
        d[1f] = Color.red;
        return d;
    }
}


