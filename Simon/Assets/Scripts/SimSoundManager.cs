using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 게임 사운드 스크립트
 */

public class SimSoundManager : MonoBehaviour
{
    #region 변수
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
    #endregion

    #region Singleton
    private static SimSoundManager sound;
    public static SimSoundManager Sound
    {
        get { return sound; }
    }

    private void Awake()
    {
        sound = GetComponent<SimSoundManager>();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (WalkStart)
        {
            if (MoveAudioSource.isPlaying == false)
                PlayWalkSound();
        }

    }

    public void PlayWalkSound() //걷기 사운드
    {
        MoveAudioSource.clip = WalkClip;
        if (MoveAudioSource.volume != 0)
        {
            MoveAudioSource.volume = 1f;
        }
        MoveAudioSource.Play();
    }

    public void ClickSimonButton() //simon 버튼 직접 클릭 사운드
    {
        EffectAudioSource.PlayOneShot(ClickSimonClip);
    }

    public void StageClear() //클리어 사운드
    {
        EffectAudioSource.PlayOneShot(SuccessClip);
    }

    public void StageFail() //fail 사운드
    {
        EffectAudioSource.PlayOneShot(FailClip);
    }

    public void ButtonOn() //Simon 버튼 제시 사운드
    {
        EffectAudioSource.PlayOneShot(ButtonClip);
    }
}
