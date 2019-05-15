using System.Collections;
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

    /** 
        Using the selectedIndex and clever naming of the sprites, when Start is
        called, the selected hero sprite will be shown.
    */
    void Start()
    {
        string path = "Sprites/Player/hero" + GameManager.instance.selectedIndex + "_big";
        player_Sprite = Resources.Load<Sprite>(path);
        player_Renderer.sprite = player_Sprite;
    }

    // Update is called once per frame
    void Update()
    {
        HandleChangeLine();
        HandleJump();
    }

    /** 
        Creates a singleton that only persists within the current Scene
    */
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

    /** 
        Gets input from the user. Depending on the input moves the 
        player up and down and plays the move line sound
    */
    void HandleChangeLine()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = second_PosOfPlayer;
            SoundManager.instance.PlayMoveLineSound();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            anim.Play(change_Line_Animation);
            transform.localPosition = first_PosOfPlayer;
            SoundManager.instance.PlayMoveLineSound();
        }
    }

    /** 
        Gets input from the user, checks if they've already jumped. If not, 
        play the jump animation, set player_Jumped to true and play the jump sound
    */
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!player_Jumped)
            {
                anim.Play("PlayerJump");
                player_Jumped = true;
                SoundManager.instance.PlayJumpSound();
            }
        }
    }

    /** 
        This function will stop the players movespeed, set the player and it's
        shadow to be disabled, play the dead sound and gameover sound and finish
        up by calling the GameOver() found in the GameplayController.
    */
    void Die()
    {
        player_Died = true;
        player.SetActive(false);
        shadow.SetActive(false);
        GameplayController.instance.moveSpeed = 0f;
        //sound manager
        SoundManager.instance.PlayDeadSound();
        SoundManager.instance.PlayGameOverSound();
        GameplayController.instance.GameOver();
    }

    /** 
        This function will call the Die() and set the explosions position to that
        of the targets, enable the explosion gameObject, disable the target and
        play the game over sound

        @param {Collider2D} the other collider the player has collided with 
    */
    void DieWithObstacle(Collider2D target)
    {
        Die();
        explosion.transform.position = target.transform.position;
        explosion.SetActive(true);
        target.gameObject.SetActive(false);
    }

    /** 
        This function will set the player's sprite back to it's original sprite 
        after the delay has finished. This occurs when the player's sprite is 
        currently showing the powerup.

        @param {IEnumerator} returns delay of 7 seconds
    */
    IEnumerator TRexDuration()
    {
        yield return new WaitForSeconds(7f);
        if (trex_Trigger)
        {
            trex_Trigger = false;
            player_Renderer.sprite = player_Sprite;
        }
    }

    /** 
        Sets the explosions position to that of the targets. Then activates the
        explosion, disables the target and plays the dead sound.

        @param {Collider2D} the other collider the player has collided with 
    */
    void DestroyObstacle(Collider2D target)
    {
        explosion.transform.position = target.transform.position;
        explosion.SetActive(false);
        explosion.SetActive(true);
        target.gameObject.SetActive(false);
        SoundManager.instance.PlayDeadSound();
    }

    /** 
        This function detects collision with the player and other gameobjects.
        If the player collides with an obstacle and does not have the T-Rex
        power up, then die else destroy the obstacle. If the player collides 
        with the T-Rex object, power up the player and change it's sprite. If
        the player collides with stars, play the star particle effects, play the
        coin sound and update the star score.

         @param {Collider2D} The other Collider2D involved in this collision.
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            SoundManager.instance.PlayPowerUpSound();

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
            SoundManager.instance.PlayCoinSound();
            GameplayController.instance.UpdateStarScore();
        }
    }
}
