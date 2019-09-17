using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public enum eView { God, Vision, Control, Extraction, Military };
public class HomelandsViewer 
{
    eView _currentView;
    int _viewIndex;
    List<eView> _validViews;
    public HomelandsViewer()
    {
        _validViews = GetValidViews();
        _viewIndex = 0;
        _currentView = _validViews[_viewIndex];
    }
    public void ToggleNextView()
    {
        _viewIndex = (_viewIndex + 1) % _validViews.Count;
        SetView(_validViews[_viewIndex]);
    }
    List<eView> GetValidViews()
    {
        return new List<eView> { eView.God, eView.Vision };
    }
    public void SetView(eView newView)
    {
        Debug.Log("View Changed From " + _currentView + " to " + newView);
        _currentView = newView;
    }

}

