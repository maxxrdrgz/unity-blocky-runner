using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject hero_Menu;
    public Text starScoreText;
    public Image music_Img;
    public Sprite music_on, music_off;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void HeroMenu()
    {
        hero_Menu.SetActive(true);
        //display star score
    }

    public void HomeButton()
    {
        hero_Menu.SetActive(false);
    }
}
