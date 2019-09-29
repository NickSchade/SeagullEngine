using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Viewer 
{
    HomelandsGame _game;
    

    public eView _viewType;
    int _viewIndex;
    List<eView> _validViews;
    Dictionary<eView, View> _views;

    View _view;


    public Viewer(HomelandsGame game)
    {
        _game = game;

        _validViews = GetValidViews();
        _viewIndex = 0;
        _viewType = _validViews[_viewIndex];
        _views = GetViewDictFromViewList(_validViews);
        _view = _views[_viewType];
    }

    public LocationGraphicsData Draw(HomelandsLocation location, Stats stats)
    {
        return _view.Draw(location, stats);
    }

    Dictionary<eView, View> GetViewDictFromViewList(List<eView> validViews)
    {
        Dictionary<eView, View> viewDict = new Dictionary<eView, View>();
        foreach (eView viewType in validViews)
        {
            viewDict[viewType] = ViewFactory.Make(_game, viewType);
        }
        return viewDict;
    }
    
    public void ToggleNextView()
    {
        _viewIndex = (_viewIndex + 1) % _validViews.Count;
        SetView(_validViews[_viewIndex]);
    }
    List<eView> GetValidViews()
    {
        return new List<eView> { eView.God, eView.Vision, eView.Control, eView.Extraction, eView.Military };
    }
    void SetView(eView newView)
    {
        Debug.Log("View Changed From " + _viewType + " to " + newView);
        _viewType = newView;
        _view = _views[_viewType];
    }

}

