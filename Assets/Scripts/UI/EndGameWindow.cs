using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameWindow : MonoBehaviour
{
    public Action GoToMenuEvent;
    public Text winText;
    public Text loseText;
 
    public void GoToMenu()
    {
        GoToMenuEvent?.Invoke();
    }
}
