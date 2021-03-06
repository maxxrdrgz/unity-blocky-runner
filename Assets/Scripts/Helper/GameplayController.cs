﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{

    public static GameplayController instance;
    public float moveSpeed, distance_Factor = 1f;
    public GameObject obstacles_Obj;
    public GameObject[] obstacles_List;
    public GameObject pausePanel;
    public Animator pauseAnim;
    public GameObject gameOverPanel;
    public Animator gameOverAnim;
    public Text finalScore_Text, bestScore_Text, finalStarScore_Text;

    [HideInInspector]
    public bool obstacles_Is_Active;

    private float distance_Move;
    private bool gameJustStarted;
    private string Coroutine_Name = "SpawnObstacles";
    private Text score_Text;
    private Text starScore_Text;
    private int starScore_count, score_count;


    void Awake()
    {
        MakeInstance();
        score_Text = GameObject.Find("ScoreText").GetComponent<Text>();
        starScore_Text = GameObject.Find("StarText").GetComponent<Text>();

    }

    /** 
        Initializes the gameJustStarted bool, calls the GetObstacles() function
        and starts the SpawnObstacles coroutine.
    */
    private void Start()
    {
        gameJustStarted = true;
        GetObstacles();
        StartCoroutine(Coroutine_Name);
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    /** 
        Creates a singleton that only persists within the current Scene
    */
    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    /** 
        Slowly ramps up the speed, until it reaches a max of 12f. While the
        player is still alive, change the main cameras transform position.
        Then call the UpdateDistance() function.
    */
    void MoveCamera()
    {
        if (gameJustStarted)
        {
            // check if player is alive
            if (!PlayerController.instance.player_Died)
            {
                if (moveSpeed < 12.0f)
                {
                    moveSpeed += Time.deltaTime * 5.0f;
                }
                else
                {
                    moveSpeed = 12f;
                    gameJustStarted = false;
                }
            }
        }

        // check if player is alive
        if (!PlayerController.instance.player_Died)
        {
            Camera.main.transform.position += new Vector3(
                moveSpeed * Time.deltaTime, 
                0f, 
                0f
            );
            UpdateDistance();
        }
    }

    /** 
        distance_Move is a counter that increases as the player is still alive
        and traveling. Increases the score count and the longer the player lives
        increase the moveSpeed depending on how far the play has gotten.
    */
    void UpdateDistance()
    {
        distance_Move += Time.deltaTime * distance_Factor;
        float round = Mathf.Round(distance_Move);
        //count and show the score
        score_count = (int)round;
        score_Text.text = score_count.ToString();
        if(round >= 30.0f && round < 60.0f)
        {
            moveSpeed = 14f;
        }
        else if(round >= 60f)
        {
            moveSpeed = 16f;
        }
    }

    /** 
        Creates a new list based on the number of obstacle configurations found
        under that obstacles_Obj. Stores each configuration inside of the
        obstacles list.
    */
    void GetObstacles()
    {
        obstacles_List = new GameObject[obstacles_Obj.transform.childCount];
        for(int i = 0; i < obstacles_List.Length ; i++)
        {
            obstacles_List[i] = obstacles_Obj.GetComponentsInChildren<ObstacleHolder>(true)[i].gameObject;
        }

    }

    /** 
        Using the obstacle list, if the player is still alive, randomly select 
        an index within the bounds of the list. With the generated index,
        set the list of obstacles to be active. Sets a new list of objects to be
        active 85% of the time.

        @returns {IEnumerator} returns time delay of 0.6 seconds
    */
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            if (!PlayerController.instance.player_Died)
            {
                if (!obstacles_Is_Active)
                {
                    if(Random.value <= 0.85f)
                    {
                        int randomIndex = 0;
                        //do this until we get a list of obstacles that are not active
                        do
                        {
                            randomIndex = Mathf.RoundToInt(Random.Range(0f, 20f));
                        } while (obstacles_List[randomIndex].activeInHierarchy);
                        obstacles_List[randomIndex].SetActive(true);
                        obstacles_Is_Active = true;
                    }
                }
            }
            yield return new WaitForSeconds(0.6f);
        }
    }

    public void UpdateStarScore()
    {
        starScore_count++;
        starScore_Text.text = starScore_count.ToString();
    }

    /** 
        Pauses the game by setting time scale to 0 and activates the pause menu
        in the gameobject hierarchy. Plays the slide in animation applied to the
        pause menu.
    */
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        pauseAnim.Play("SlideIn");
    }

    public void ResumeGame()
    {
        pauseAnim.Play("SlideOut");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HomeButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /** 
        Stop the games time, display the game over menu and play the slide in
        animation. Updates the final score/final star score and stores the
        score count and the star score in the GameManager. Save the game data,
        when game over occurs.
    */
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        gameOverAnim.Play("SlideIn");
        finalScore_Text.text = score_count.ToString();
        finalStarScore_Text.text = starScore_count.ToString();
        if(GameManager.instance.scoreCount < score_count)
        {
            GameManager.instance.scoreCount = score_count;
        }

        bestScore_Text.text = "Best: " + GameManager.instance.scoreCount.ToString();
        GameManager.instance.starScore += starScore_count;
        GameManager.instance.SaveGameData();
    }
}
