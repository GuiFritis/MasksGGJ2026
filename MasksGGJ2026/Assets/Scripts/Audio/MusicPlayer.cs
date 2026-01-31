using System;
using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static WaitForSeconds WAIT_FOR_SECONDS_0_1 = new WaitForSeconds(0.1f);

    public static Action<AudioClip, bool> ChangeAudio;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _sceneMusic;
    private Coroutine _musicCoroutine;

    void Awake()
    {
        ChangeAudio += ChangeAudioClip;
    }

    void Start()
    {
        if(_sceneMusic != null)
        {
            _audioSource.clip = _sceneMusic;
            _audioSource.loop = true;
            _audioSource.Play();
        }       
    }


    private IEnumerator CheckMusicEnd()
    {
        while (_audioSource.isPlaying)
        {
            yield return WAIT_FOR_SECONDS_0_1;
        }
        _audioSource.clip = _sceneMusic;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    private void ChangeAudioClip(AudioClip clip, bool isSceneMusic)
    {
        if (_musicCoroutine != null)
        {
            StopCoroutine(_musicCoroutine);
        }
        if (isSceneMusic)
        {
            _sceneMusic = clip;
        }
        else
        {
            _audioSource.loop = false;
            _musicCoroutine = StartCoroutine(CheckMusicEnd());
        }
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    void OnDisable()
    {
        ChangeAudio -= ChangeAudioClip;
    }
}
