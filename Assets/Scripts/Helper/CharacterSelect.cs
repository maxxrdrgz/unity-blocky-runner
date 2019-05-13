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

    /** 
        Initializes the characters found in the character select menu. Gets the
        last selectedIndex from the GameManager and sets the character from that
        index to active. Lastly, loads available heros from the GameManager 
        instance object
    */
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

    /** 
        This function will cycle to the next hero in the available_Heroes array
        by increasing the currentIndex. Checks if current index is at the last
        position in the array and resets the index to 0 if true. Lastly checks
        if character has been unlocked.
    */
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

    /** 
        Much like NextHero() except it cycles to the previous hero in the
        available_Heroes array by decreasing the currentIndex. Checks if current
        index is at the starting position in the array and sets itself to
        available_Heroes.length-1 if true. Lastly checks if the character has
        been unlocked.
    */
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

    /** 
        The heroes array contains the unlocked heroes stored in
        the GameManager. Given the currentIndex, this function will check if the
        hero is unlocked. If the user is looking at the selected character, the
        select button sprite will be green and the text will show 'Selected'. If
        the hero[currentIndex] is unlocked but not selected, the select button 
        will be blue and prompt the user to select. If the hero is not unlocked
        the user will be prompted to purchase the hero.
    */
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

    /** 
        This function changes the hero selected. If the hero is locked, it will
        check to see if the user has enough stars to unlock the hero. If so,
        it will subtract the heroPrice from the users starScore, unlock the hero
        in the hero array and save the current game data. If the hero is already
        unlocked, change the current hero to be the selected hero and save the
        game data.
    */
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
