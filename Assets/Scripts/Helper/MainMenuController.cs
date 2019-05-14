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
    public Image selectedHeroImg;

    private void Awake()
    {
        ChangeSelectedHeroImg();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    /** 
        Sets the hero menu to be active and gets the star score from the
        GameManager.
    */
    public void HeroMenu()
    {
        hero_Menu.SetActive(true);
        starScoreText.text = GameManager.instance.starScore.ToString();
    }

    /** 
        From the main menu, only the hero menu can only be shown on top of the
        home screen. Disable the hero menu.
    */
    public void HomeButton()
    {
        hero_Menu.SetActive(false);
    }

    /** 
        Checks the playSound boolean in the GameManager. Changes the music
        enabled sprite depeding on whether the boolean is true or false.
    */
    public void MusicButton()
    {
        if (GameManager.instance.playSound)
        {
            music_Img.sprite = music_off;
            GameManager.instance.playSound = false;
        }
        else
        {
            GameManager.instance.playSound = true;
            music_Img.sprite = music_on;
        }
    }

    /** 
        Using the selectedIndex stored in the GameManager, thanks to clever
        naming of the sprites, show the correct sprite on the main menu for the
        selected character.
    */
    public void ChangeSelectedHeroImg()
    {
        string path = "Sprites/Player/hero" + GameManager.instance.selectedIndex + "_big";
        selectedHeroImg.sprite = Resources.Load<Sprite>(path);
    }
}
