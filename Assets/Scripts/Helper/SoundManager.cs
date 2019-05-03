using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource move_audio_source,
        jump_audio_source,
        powerup_die_audio_source,
        bg_audio_source;

    public AudioClip power_up_clip,
        die_clip,
        coin_clip,
        game_over_clip;

    void Awake()
    {
        MakeInstance();    
    }

    private void Start()
    {
        //test if we should play bg sound
        if (GameManager.instance.playSound)
        {
            bg_audio_source.Play();
        }
        else
        {
            bg_audio_source.Stop();
        }
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    //TODO optimize sound manager to use one audio source and just change the clips

    public void PlayMoveLineSound()
    {
        move_audio_source.Play();
    }

    public void PlayJumpSound()
    {
        jump_audio_source.Play();
    }

    public void PlayDeadSound()
    {
        powerup_die_audio_source.clip = die_clip;
        powerup_die_audio_source.Play();
    }

    public void PlayPowerUpSound()
    {
        powerup_die_audio_source.clip = power_up_clip;
        powerup_die_audio_source.Play();
    }

    public void PlayCoinSound()
    {
        powerup_die_audio_source.clip = coin_clip;
        powerup_die_audio_source.Play();
    }

    public void PlayGameOverSound()
    {
        bg_audio_source.Stop();
        bg_audio_source.clip = game_over_clip;
        bg_audio_source.loop = false;
        bg_audio_source.Play();
    }
}
