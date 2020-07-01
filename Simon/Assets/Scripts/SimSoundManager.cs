using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimSoundManager : MonoBehaviour
{
    public AudioSource EffectAudioSource;
    public AudioSource MoveAudioSource;
    public AudioSource BGMAudioSource;

    public AudioClip ClickSimonClip;
    public AudioClip SuccessClip;
    public AudioClip FailClip;
    public AudioClip ButtonClip;

    public AudioClip WalkClip;

    public bool WalkStart = false;
    public bool JumpStart = false;

    //싱글톤
    private static SimSoundManager sound;
    public static SimSoundManager Sound
    {
        get { return sound; }
    }

    private void Awake()
    {
        sound = GetComponent<SimSoundManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (WalkStart)
        {
            if (MoveAudioSource.isPlaying == false)
                PlayWalkSound();
        }

    }

    public void PlayWalkSound()
    {
        MoveAudioSource.clip = WalkClip;
        if (MoveAudioSource.volume != 0)
        {
            MoveAudioSource.volume = 1f;
        }
        MoveAudioSource.Play();
    }

    public void ClickSimonButton()
    {
        EffectAudioSource.PlayOneShot(ClickSimonClip);
    }

    public void StageClear()
    {
        EffectAudioSource.PlayOneShot(SuccessClip);
    }

    public void StageFail()
    {
        EffectAudioSource.PlayOneShot(FailClip);
    }

    public void ButtonOn()
    {
        EffectAudioSource.PlayOneShot(ButtonClip);
    }
}
