using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eStartPosition { OppositeCorners, RandomCorners}

public abstract class StartPositioner
{
    protected HomelandsGame _game;
    public StartPositioner(HomelandsGame game)
    {
        _game = game;
    }
    protected abstract Dictionary<Player, Loc> GetStartingLocs();
    public Dictionary<Player, Pos> GetStartingPos()
    {
        Dictionary<Player, Loc> startingLocs = GetStartingLocs();

        Dictionary<Player, Pos> startingPos = new Dictionary<Player, Pos>();

        foreach (Player player in startingLocs.Keys)
        {
            Loc startingLoc = startingLocs[player];
            foreach (Pos pos in _game._locations.Keys)
            {
                if (pos.gridLoc.key() == startingLoc.key())
                {
                    startingPos[player] = pos;
                    break;
                }
            }
        }

        return startingPos;
    }

    public List<Loc> GetSquareLocs(int x, int y)
    {
        // corners for hex will be different
        List<Loc> squareLocs = new List<Loc>
        {
            new Loc(0,0),
            new Loc(x,y),
            new Loc(0,y),
            new Loc(x,0)
        };
        return squareLocs;
    }
}

public class OppositeCornersPositioner : StartPositioner
{
    public OppositeCornersPositioner(HomelandsGame game) : base(game) {}

    protected override Dictionary<Player,Loc> GetStartingLocs()
    {
        int xMax = _game._settings._mapSettings._xDim - 1;
        int yMax = _game._settings._mapSettings._yDim - 1;
        List<Loc> possibleLocs = GetSquareLocs(xMax, yMax);

        Dictionary<Player, Loc> startingLocs = new Dictionary<Player, Loc>();
        for (int i = 0; i < _game._playerSystem._players.Count; i++)
        {
            if (i < possibleLocs.Count - 1)
                startingLocs[_game._playerSystem._players[i]] = possibleLocs[i];
            else
                throw new System.NotImplementedException();
        }
        return startingLocs;
    }
}

public class RandomCornersPositioner : StartPositioner
{
    public RandomCornersPositioner(HomelandsGame game) : base(game) {}

    protected override Dictionary<Player, Loc> GetStartingLocs()
    {
        throw new System.NotImplementedException();
    }
}

public class FStartPositioner
{
    public static StartPositioner Make(eStartPosition type, HomelandsGame game)
    {
        if (type == eStartPosition.OppositeCorners)
        {
            return new OppositeCornersPositioner(game);
        }
        else if (type == eStartPosition.RandomCorners)
        {
            return new RandomCornersPositioner(game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
