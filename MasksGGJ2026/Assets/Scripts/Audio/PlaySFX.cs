using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioSO _audioSO;

    public void SetAudioSO(AudioSO audioSO)
    {
        _audioSO = audioSO;
    }

    public void PlayAudioClip()
    {
        SFX_Pool.Instance.Play(_audioSO);
    }
}
