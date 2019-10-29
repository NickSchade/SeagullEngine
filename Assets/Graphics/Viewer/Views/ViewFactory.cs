using UnityEngine;
using System.Collections;

public static class ViewFactory
{
    public static View Make(HomelandsGame game, eView viewType)
    {
        if (viewType == eView.God)
        {
            return new ViewGod(game);
        }
        else if (viewType == eView.Vision)
        {
            return new ViewVision(game);
        }
        else if (viewType == eView.Control)
        {
            return new ViewControl(game);
        }
        else if (viewType == eView.Extraction)
        {
            return new ViewExtraction(game);
        }
        else if (viewType == eView.Military)
        {
            return new ViewMilitary(game);
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
