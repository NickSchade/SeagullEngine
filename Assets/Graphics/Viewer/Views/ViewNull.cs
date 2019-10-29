using UnityEngine;
using System.Collections;

public class ViewNull : View
{
    public ViewNull(HomelandsGame game) : base(game)
    {
    }

    public override LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        Color c = Color.black;
        LocationGraphicsData lgd = new LocationGraphicsData(c);
        return lgd;
    }
}
