using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioVolumeChanger : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _endVolumeValue;

    private const float MaxVolumeDelta = 0.001f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void IncreaseVolume()
    {
        _endVolumeValue = 1;
        StartCoroutine(FadeIn(_endVolumeValue, MaxVolumeDelta));
    }

    public void FadeOutVolume()
    {
        _endVolumeValue = 0;
        StartCoroutine(FadeIn(_endVolumeValue, MaxVolumeDelta));
    }

    private IEnumerator FadeIn(float target, float maxDelta)
    {
        while (_audioSource.volume != target)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, target, maxDelta);
            yield return null;
        }
    }
}