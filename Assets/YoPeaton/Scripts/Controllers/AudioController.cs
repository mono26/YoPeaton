using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource mainAudioSource;

    [SerializeField]
    private AudioClip correctAnswerClip;
    [SerializeField]
    private AudioClip wrongAnswerClip;
    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource = this.GetComponent<AudioSource>();
    }


    public void PlayAnswerSound(string clip)
    {
        if (clip == "Correct")
        {
            mainAudioSource.clip = correctAnswerClip;
            mainAudioSource.Play();
            return;
        }

        if(clip == "Wrong")
        {
            mainAudioSource.clip = wrongAnswerClip;
            mainAudioSource.Play();
            return;
        }

        else
        {
            Debug.LogError("No clip found");
            return;
        }

    }
}
