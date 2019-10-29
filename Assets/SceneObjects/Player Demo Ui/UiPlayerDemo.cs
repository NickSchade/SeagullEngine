using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPlayerDemo : MonoBehaviour
{
    public GameObject playerName;
    public GameObject playerResource;

    Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }
    public void Draw()
    {
        PlayerDemographics pd = _player.GetDemographics();

        Text name = playerName.GetComponent<Text>();
        name.text = pd.name;
        name.color = pd.color;

        Text resource = playerResource.GetComponent<Text>();
        resource.text = pd.resources.ToString();
    }
}
