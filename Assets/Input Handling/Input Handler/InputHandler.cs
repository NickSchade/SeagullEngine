using UnityEngine;
using System.Collections;


public class InputHandler
{
    public HomelandsGame _game;

    public KeyHandlerHomelands _keyHandler;
    public MouseHandlerHomelands _mouseHandler;

    public InputHandler(HomelandsGame game)
    {
        _game = game;
        _mouseHandler = new MouseHandlerHomelands(_game._locations);
        _keyHandler = new KeyHandlerHomelands(_game);
    }

    public HomelandsTurnData HandleInput(InputHandlerInfo inputHandlerInfo)
    {
        if (MapNavConfigs._enableMapZoom)
            HandleZoom(inputHandlerInfo._mouseHandlerInfo);

        MouseHandlerOutput mho = _mouseHandler.GetMouseHandlerOutput(inputHandlerInfo._mouseHandlerInfo);
        KeyHandlerOutput kho = _keyHandler.HandleKeys(inputHandlerInfo._keyHandlerInfo);

        HomelandsTurnData htd = new HomelandsTurnData(kho, mho);
        return htd;
    }


    void HandleZoom(MouseHandlerInfo mhi)
    {
        float d = mhi._axis;
        if (d != 0)
        {
            if (d > 0f)
            {
                Camera.main.GetComponent<Camera>().orthographicSize -= 1;
            }
            else if (d < 0f)
            {
                Camera.main.GetComponent<Camera>().orthographicSize += 1;
            }
        }
    }
}
