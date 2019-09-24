using UnityEngine;
using System.Collections;

public static class ViewFactory
{
    public static View Make(HomelandsGame game, eView viewType)
    {
        if (viewType == eView.God)
        {
            return new ViewGod();
        }
        else if (viewType == eView.Vision)
        {
            return new ViewVision();
        }
        else if (viewType == eView.Control)
        {
            return new ViewControl();
        }
        else if (viewType == eView.Extraction)
        {
            return new ViewExtraction();
        }
        else if (viewType == eView.Military)
        {
            return new ViewMilitary();
        }
        else
        {
            throw new System.NotImplementedException();
        }
    }
}
