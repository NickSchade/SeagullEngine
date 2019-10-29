using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnUiRealtime : MonoBehaviour
{
    public Text turnNumber;
    public Text timeLeft;
    public Slider progressSlider;

    HomelandsGame _game;
    public void InitializeUi(HomelandsGame game)
    {
        gameObject.SetActive(true);
        _game = game;
    }

    public void UpdateUi(TickInfo tick)
    {
        turnNumber.text = "Turn #"+tick._turnNumber.ToString();
        timeLeft.text = (Mathf.Round(tick._secondsUntilNextTurn*10f)/10f).ToString()+"s";
        progressSlider.value = tick._progressToNextTurn;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
