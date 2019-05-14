using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameData gameData;

    [HideInInspector]
    public int starScore, scoreCount, selectedIndex;

    [HideInInspector]
    public bool[] heroes;

    [HideInInspector]
    public bool playSound = true;

    void Awake()
    {
        MakeSingleton();
        InitializeGameData();
        //save_file_path = Application.persistentDataPath + "GameData.dat";
    }

    private void Start()
    {
        print(Application.persistentDataPath + "GameData.dat");
        if(gameData != null)
        {
            print("data loaded");
        }
    }

    /** 
        Creates singleton of the GameManager instance object
    */
    void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /** 
        Attempts to load the game data. If no game data is found, sets up
        initial values for the scores, heros and indexes. Then saves the initial
        game data.
    */
    void InitializeGameData()
    {
        LoadGameData();

        //initialize game if no save file was found
        if (gameData == null)
        {
            starScore = 0;
            scoreCount = 0;
            selectedIndex = 0;

            heroes = new bool[9];
            heroes[selectedIndex] = true;
            for (int i = 1; i < heroes.Length; i++)
            {
                heroes[i] = false;
            }

            gameData = new GameData();
            gameData.Heroes = heroes;
            gameData.StarScore = starScore;
            gameData.ScoreCount = scoreCount;
            gameData.SelectedIndex = selectedIndex;

            SaveGameData();
        }
    }

    /** 
        Creates the game save file using the FileStream. Sets the gameData 
        object with the available data stored in the GameManager, and then
        serializes the data to the save game file. Closes the file once
        finished.
    */
    public void SaveGameData()
    {
        FileStream file = null;
        try {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "GameData.dat");
            if(gameData != null)
            {
                gameData.Heroes = heroes;
                gameData.StarScore = starScore;
                gameData.ScoreCount = scoreCount;
                gameData.SelectedIndex = selectedIndex;
                bf.Serialize(file, gameData);
            }
        } catch(Exception e) {

        } finally {
            if(file != null)
            {
                file.Close();
            }
        }
    }

    /** 
        Creates a file stream and attempts to open the last game save file.
        Using the BinaryFormatter, deserialize the gamedata and type cast it
        back to the GameData object. Store the loaded data in the GameManager.
        Close the file once finisehd.
    */
    public void LoadGameData()
    {
        FileStream file = null;

        try {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Open(Application.persistentDataPath + "GameData.dat", FileMode.Open);
            gameData = (GameData)bf.Deserialize(file);
            if(gameData != null)
            {
                starScore = gameData.StarScore;
                scoreCount = gameData.ScoreCount;
                heroes = gameData.Heroes;
                selectedIndex = gameData.SelectedIndex;
            }
        } catch (Exception e) {
            
        } finally {
            if(file != null)
            {
                file.Close();
            }
        }
    }
}
