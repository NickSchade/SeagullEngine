using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum ePlayerAction
{
    EndTurn,
    ChangeView,
    ChangePlayer,
    Pause,
    Help,

    ScrollUp,
    ScrollDown,
    ScrollLeft,
    ScrollRight
}

public static class InputConfigs
{
    public const KeyCode _changePlayers = KeyCode.P;
    public const KeyCode _changeView = KeyCode.V;
    public const KeyCode _endTurn = KeyCode.Return;
    public const KeyCode _pause = KeyCode.Space;
    public const KeyCode _help = KeyCode.H;

    public const KeyCode _mapScrollUp = KeyCode.W;
    public const KeyCode _mapScrollDown = KeyCode.S;
    public const KeyCode _mapScrollLeft = KeyCode.A;
    public const KeyCode _mapScrollRight = KeyCode.D;

    public static List<ePlayerAction> GetPlayerActions()
    {
        List<ePlayerAction> actions = Enum.GetValues(typeof(ePlayerAction)).Cast<ePlayerAction>().ToList();
        return actions;
    }

}

public static class MapNavConfigs
{
    public const bool _enableMapScroll = false;
    public const bool _enableMapZoom = false;
}

public class InputSettings
{
    static Dictionary<KeyCode, ePlayerAction> _actionsByKey;
    static Dictionary<ePlayerAction, KeyCode> _keysByAction;

    public static List<KeyCode> GetAllKeyCodes()
    {
        if (_actionsByKey == null)
            InitializeDualDictionaries();
        return _actionsByKey.Keys.ToList();
    }
    static void InitializeDualDictionaries()
    {
        _actionsByKey = new Dictionary<KeyCode, ePlayerAction>
        {
            {InputConfigs._mapScrollUp, ePlayerAction.ScrollUp },
            {InputConfigs._mapScrollDown, ePlayerAction.ScrollDown },
            {InputConfigs._mapScrollLeft, ePlayerAction.ScrollLeft },
            {InputConfigs._mapScrollRight, ePlayerAction.ScrollRight },


            {InputConfigs._changeView, ePlayerAction.ChangeView },
            {InputConfigs._changePlayers, ePlayerAction.ChangePlayer },
            {InputConfigs._endTurn, ePlayerAction.EndTurn },
            {InputConfigs._pause, ePlayerAction.Pause },
            {InputConfigs._help, ePlayerAction.Help },
        };
        _keysByAction = new Dictionary<ePlayerAction, KeyCode>();
        foreach (KeyCode kc in _actionsByKey.Keys)
        {
            ePlayerAction action = _actionsByKey[kc];
            _keysByAction[action] = kc;
        }

    }

    public static ePlayerAction GetAction(KeyCode _key)
    {
        if (_actionsByKey == null)
            InitializeDualDictionaries();
        return _actionsByKey[_key];
    }
    public static KeyCode GetKeyCode(ePlayerAction _action)
    {
        if (_actionsByKey == null)
            InitializeDualDictionaries();
        return _keysByAction[_action];
    }
}
