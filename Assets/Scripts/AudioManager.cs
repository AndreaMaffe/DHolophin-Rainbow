using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private AudioClip backgroundMusic;
    private AudioClip correctAnswerSound;
    private AudioClip wrongAnswerSound;
    private AudioClip pop1;
    private AudioClip pop2;
    private AudioClip fireworks;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

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
    }

    public void PlayBackgroundMusic()
    {
        audioSource.Play();
    }

    public void PlayCorrectAnswerSound()
    {
        audioSource.PlayOneShot(correctAnswerSound, 1f);
    }

    public void PlayWrongAnswerSound()
    {
        audioSource.PlayOneShot(wrongAnswerSound, 1f);
    }

    public void PlayPop1()
    {
        audioSource.PlayOneShot(pop1, 1f);
    }

    public void PlayPop2()
    {
        audioSource.PlayOneShot(pop2, 1f);
    }

    public void PlayFireworksSound()
    {
        audioSource.PlayOneShot(fireworks, 5f);
    }

}
