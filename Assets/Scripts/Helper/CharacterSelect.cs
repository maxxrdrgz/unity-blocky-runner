using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] available_Heroes;
    public Text selectedText;
    public GameObject starIcon;
    public Image selectbtn_Img;
    public Sprite button_green, button_blue;
    public Text starScoreText;

    private int currentIndex;
    private bool[] heroes;
    private int heroPrice = 1000;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacters();
    }

    void InitializeCharacters()
    {
        currentIndex = GameManager.instance.selectedIndex;
        for(int i =0; i < available_Heroes.Length; i++)
        {
            available_Heroes[i].SetActive(false);
        }
        available_Heroes[currentIndex].SetActive(true);
        heroes = GameManager.instance.heroes;
    }

    public void NextHero()
    {
        available_Heroes[currentIndex].SetActive(false);
        if(currentIndex + 1 == available_Heroes.Length)
        {
            currentIndex = 0;
        }
        else
        {
            currentIndex++;
        }
        available_Heroes[currentIndex].SetActive(true);
        CheckIfCharacterisUnlocked();
    }

    public void PreviousHero()
    {
        available_Heroes[currentIndex].SetActive(false);
        if (currentIndex - 1 == -1)
        {
            currentIndex = available_Heroes.Length-1;
        }
        else
        {
            currentIndex--;
        }
        available_Heroes[currentIndex].SetActive(true);
        CheckIfCharacterisUnlocked();
    }

    public void CheckIfCharacterisUnlocked()
    {
        // if hero is unlocked
        if (heroes[currentIndex])
        {
            starIcon.SetActive(false);
            if(currentIndex == GameManager.instance.selectedIndex)
            {
                selectbtn_Img.sprite = button_green;
                selectedText.text = "Selected";
            }
            else
            {
                selectbtn_Img.sprite = button_blue;
                selectedText.text = "Select?";
            }
        }
        else
        {
            selectbtn_Img.sprite = button_blue;
            starIcon.SetActive(true);
            selectedText.text = heroPrice.ToString();
        }
    }

    public void SelectHero()
    {
        // if hero is locked
        if (!heroes[currentIndex])
        {
            if (GameManager.instance.starScore >= heroPrice)
            {
                GameManager.instance.starScore -= heroPrice;
                selectbtn_Img.sprite = button_green;
                starIcon.SetActive(false);
                selectedText.text = "Selected";
                heroes[currentIndex] = true;
                starScoreText.text = GameManager.instance.starScore.ToString();
                GameManager.instance.selectedIndex = currentIndex;
                GameManager.instance.heroes = heroes;
                GameManager.instance.SaveGameData();
            }
            else
            {
                print("not enough stars");
            }
        }
        else
        {
            selectbtn_Img.sprite = button_green;
            selectedText.text = "Selected";
            GameManager.instance.selectedIndex = currentIndex;
            GameManager.instance.SaveGameData();
        }
    }
}
