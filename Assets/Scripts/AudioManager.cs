using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioClip backgroundMusic;
    private static AudioClip correctAnswerSound;
    private static AudioClip wrongAnswerSound;

    private static AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        backgroundMusic = Resources.Load<AudioClip>("Sounds/HappySoundtrack");
        correctAnswerSound = Resources.Load<AudioClip>("Sounds/CorrectAnswer");
        wrongAnswerSound = Resources.Load<AudioClip>("Sounds/WrongAnswer");
    }

    public static void PlayBackgroundMusic()
    {
        audioSource.PlayOneShot(backgroundMusic, 0.3f);
    }

    public static void PlayCorrectAnswerSound()
    {
        audioSource.PlayOneShot(correctAnswerSound, 1f);
    }

    public static void PlayWrongAnswerSound()
    {
        audioSource.PlayOneShot(wrongAnswerSound, 1f);
    }




}
