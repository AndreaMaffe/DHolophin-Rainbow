using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioClip backgroundMusic;
    private static AudioClip correctAnswerSound;
    private static AudioClip wrongAnswerSound;
    private static AudioClip pop1;
    private static AudioClip pop2;
    private static AudioClip fireworks;

    private static AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        backgroundMusic = Resources.Load<AudioClip>("Sounds/HappySoundtrack");
        correctAnswerSound = Resources.Load<AudioClip>("Sounds/CorrectAnswer");
        wrongAnswerSound = Resources.Load<AudioClip>("Sounds/WrongAnswer");
        pop1 = Resources.Load<AudioClip>("Sounds/Pop1");
        pop2 = Resources.Load<AudioClip>("Sounds/Pop2");
        fireworks = Resources.Load<AudioClip>("Sounds/FireworksSound");

        audioSource.clip = backgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.loop = true;

        DontDestroyOnLoad(this.gameObject);
    }

    public static void PlayBackgroundMusic()
    {
        audioSource.Play();
    }

    public static void PlayCorrectAnswerSound()
    {
        audioSource.PlayOneShot(correctAnswerSound, 1f);
    }

    public static void PlayWrongAnswerSound()
    {
        audioSource.PlayOneShot(wrongAnswerSound, 1f);
    }

    public static void PlayPop1()
    {
        audioSource.PlayOneShot(pop1, 1f);
    }

    public static void PlayPop2()
    {
        audioSource.PlayOneShot(pop2, 1f);
    }

    public static void PlayFireworksSound()
    {
        audioSource.PlayOneShot(fireworks, 5f);
    }

}
