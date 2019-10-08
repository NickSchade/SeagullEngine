using UnityEngine;
using System.Collections;

public interface IInputHandler
{
    IKeyHandler _keyHandler { get; set; }
    IMouseHandler _mouseHandler { get; set; }
    HomelandsTurnData HandleInput(InputHandlerInfo inputHandlerInfo);
}

public class InputHandler :IInputHandler
{
    public HomelandsGame _game;

    public IKeyHandler _keyHandler { get; set; }
    public IMouseHandler _mouseHandler { get; set; }

    public InputHandler(HomelandsGame game)
    {
        _game = game;
        _mouseHandler = new MouseHandlerHomelands(_game._locations);
        _keyHandler = new KeyHandlerHomelands(_game);
    }

    public HomelandsTurnData HandleInput(InputHandlerInfo inputHandlerInfo)
    {
        dStructurePlacement structureToBuild = _mouseHandler.HandleMouse(inputHandlerInfo._mouseHandlerInfo);
        bool keyHandle = _keyHandler.HandleKeys(inputHandlerInfo._keyHandlerInfo);

        HomelandsTurnData htd = new HomelandsTurnData(structureToBuild);
        return htd;
    }
}
