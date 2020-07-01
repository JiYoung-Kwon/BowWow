using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    public AudioSource JumpAudioSource;
    public AudioClip JumpClip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayJumpSound()
    {
        JumpAudioSource.PlayOneShot(JumpClip);
    }

}
