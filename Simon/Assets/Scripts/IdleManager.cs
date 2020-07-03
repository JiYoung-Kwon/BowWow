using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ally(idle)에 들어가는 사운드 스크립트
 */

public class IdleManager : MonoBehaviour
{
    public AudioSource JumpAudioSource;
    public AudioClip JumpClip;

    public void PlayJumpSound() //점프 효과음
    {
        JumpAudioSource.PlayOneShot(JumpClip);
    }

}
