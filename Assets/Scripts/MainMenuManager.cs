using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void ButtonPressed(int buttonID)
    {
        switch(buttonID)
        {
            //Play Game
            case 0:
                SceneManager.LoadScene("GameScene");
                break;
            //Close Game
            case 1:
                Application.Quit();
                break;
        }
    }
}
