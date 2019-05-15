using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Animator anim;
    private string walk_Animation = "PlayerWalk";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /** 
        Plays the walk animation and sets the player_Jumped bool to false
    */
    void PlayerWalkAnimation()
    {
        anim.Play(walk_Animation);
        if (PlayerController.instance.player_Jumped)
        {
            PlayerController.instance.player_Jumped = false;
        }
    }

    /** 
        Disables the gameobject when called
    */
    void AnimationEnded()
    {
        gameObject.SetActive(false);
    }

    /** 
        Set the time scale to 1 and disable the gameobject this script is 
        attached to
    */
    void PausePanelClose()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
