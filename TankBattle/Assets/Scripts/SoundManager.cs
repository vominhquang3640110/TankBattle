using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("In Game")]
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource BGM_InGame;
    [SerializeField] private AudioClip ClickButton;
    [SerializeField] private AudioClip GetCoin;
    [SerializeField] private AudioClip MatchingSuccess;
    [SerializeField] private AudioClip StepChange;
    [SerializeField] private AudioClip Win;
    [SerializeField] private AudioClip Lose;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (BGM.isPlaying)
        {
            StopAudioSource(BGM);
        }
        PlayAudioSource(BGM);
    }
    public void StopBGM()
    {
        StopAudioSource(BGM);
    }

    public void PlayBGMInGame()
    {
        if (BGM_InGame.isPlaying)
        {
            StopBGMInGame();
        }
        PlayAudioSource(BGM_InGame);
    }
    public void StopBGMInGame()
    {
        StopAudioSource(BGM_InGame);
    }

    public void PlayClickButton()
    {
        PlayOneShotAudioClip(ClickButton);
    }

    public void PlayGetCoin()
    {
        PlayOneShotAudioClip(GetCoin);
    }

    public void PlayWin()
    {
        PlayOneShotAudioClip(Win);
    }
    public void PlayLose()
    {
        PlayOneShotAudioClip(Lose);
    }

    public void PlayMatchingSuccess()
    {
        PlayOneShotAudioClip(MatchingSuccess);
    }

    public void PlayStepChange()
    {
        PlayOneShotAudioClip(StepChange);
    }

    public void PlayAudioSource(AudioSource audioSource)
    {
        audioSource.Play();
    }
    public void StopAudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    public void PlayOneShotAudioClip(AudioClip audioClip)
    {
        SFXAudioSource.PlayOneShot(audioClip);
    }
}
