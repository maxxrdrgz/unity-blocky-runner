﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject player,
        shadow,
        explosion;
    
    public Vector3 first_PosOfPlayer,
        second_PosOfPlayer;

    private Animator anim;
    private string jump_Animation = "PlayerJump",
        change_Line_Animation = "ChangeLine";
    private SpriteRenderer player_Renderer;
    public Sprite trex_Sprite, player_Sprite;
    private bool trex_Trigger;
    private GameObject[] star_Effect;

    [HideInInspector]
    public bool player_Died;

    [HideInInspector]
    public bool player_Jumped;


    private void Awake()
    {
        MakeInstance();
        anim = player.GetComponent<Animator>();
        player_Renderer = player.GetComponent<SpriteRenderer>();
        star_Effect = GameObject.FindGameObjectsWithTag(Tags.STAR_EFFECT);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleChangeLine();
        HandleJump();
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void HandleChangeLine()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = second_PosOfPlayer;
            //play the sound
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = first_PosOfPlayer;
            //play the sound
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player_Jumped)
            {
                anim.Play("PlayerJump");
                player_Jumped = true;
            }
        }
    }

    void Die()
    {
        player_Died = true;
        player.SetActive(false);
        shadow.SetActive(false);
        GameplayController.instance.moveSpeed = 0f;
        //GameplayController.instance.GameOver();
        //sound manager
        //player dead
        //game over
    }

    void DieWithObstacle(Collider2D target)
    {
        Die();
        explosion.transform.position = target.transform.position;
        explosion.SetActive(true);
        target.gameObject.SetActive(false);
        //sound manager play player dead
    }

    IEnumerator TRexDuration()
    {
        yield return new WaitForSeconds(7f);
        if (trex_Trigger)
        {
            trex_Trigger = false;
            player_Renderer.sprite = player_Sprite;
        }
    }

    void DestroyObstacle(Collider2D target)
    {
        explosion.transform.position = target.transform.position;
        explosion.SetActive(false);
        explosion.SetActive(true);
        target.gameObject.SetActive(false);
        //sound manager
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.tag);
        if(collision.tag == Tags.OBSTACLE)
        {
            if (!trex_Trigger)
            {
                DieWithObstacle(collision);
            }
            else
            {
                DestroyObstacle(collision);
            }
        }

        if(collision.tag == Tags.T_REX)
        {
            trex_Trigger = true;
            player_Renderer.sprite = trex_Sprite;
            collision.gameObject.SetActive(false);
            //sound manager to play music

            StartCoroutine(TRexDuration());
        }

        if(collision.tag == Tags.STAR)
        {
            for(int i = 0; i < star_Effect.Length; i++)
            {
                if (!star_Effect[i].activeInHierarchy)
                {
                    star_Effect[i].transform.position = collision.transform.position;
                    star_Effect[i].SetActive(true);
                    break;
                }
            }
            collision.gameObject.SetActive(false);
            // sound manager
            // gameplay controller to increase star score
        }
    }
}
