using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }
    [Header("AudioSource FX")]
    [SerializeField]
    private AudioSource audioSourceFX;

    [Header("AudioSource Music")]
    [SerializeField]
    private AudioSource audioSourceMusic;

    [Header("0 - ClearLine \n1 - Fall \n2 - GameOver")]
    public AudioClip[] FX;

    [Header("0 - MusicA \n1 - MusicB")]
    public AudioClip[] MX;

    private void Awake()
    {
        _instance = this;
    }

    public void PlayFX(int soundID)
    {
        //0 - Clear (Clear Line)
        //1 - Fall (Tetris Piece Felt)
        //2 - Success (GameOver)
        audioSourceFX.PlayOneShot(FX[soundID]);
    }

    public void PlayMx(int musicID)
    {
        //0 - Music A
        //1 - Music B
        audioSourceMusic.clip = MX[musicID];
        audioSourceMusic.Play();
    }

    public void StopMx()
    {
        if(audioSourceMusic.clip != null)
        {
            audioSourceMusic.Stop();
            audioSourceMusic.clip = null;
        }

    }
}
