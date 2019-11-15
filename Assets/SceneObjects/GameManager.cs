using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // GRAPHICS
    public GraphicsManager _graphicsManager;
    public HudManager _hudManager;

    // INPUT
    public InputManager _inputManager;

    // GAME
    public GameConfigs _gameConfigs;
    public HomelandsGame _game;
    
    // Start is called before the first frame update
    void Start()
    {
        GameSettings settings = FGameSettings.Make(_gameConfigs);

        _game = FGame.Make(this, settings);

        _inputManager.Initialize(this, _game._inputHandler);
        _hudManager.InitializeUi(_game, settings);
    }

    // Update is called once per frame
    void Update()
    {
        InputHandlerInfo inputHandlerInfo = _inputManager.GetInput();

        TickInfo tick = _game.TakeTick(inputHandlerInfo);

        _graphicsManager.Draw(tick._graphicsData);

        _hudManager.UpdateUi(tick);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        Application.Quit();
    }
        
}
