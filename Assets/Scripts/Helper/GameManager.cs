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

    // Start is called before the first frame update
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
