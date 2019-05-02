using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public GameObject[] available_Heroes;
    public Text selectedText;
    public GameObject starIcon;
    public Image selectbtn;
    public Sprite button_green, button_blue;
    public Text starScoreText;

    private int currentIndex;
    private bool[] heroes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void InitializeCharacters()
    {
        currentIndex = 0;
        for(int i =0; i < available_Heroes.Length; i++)
        {
            available_Heroes[i].SetActive(false);
        }
        available_Heroes[currentIndex].SetActive(true);
        //heroes = GameManager;
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
    }


}
