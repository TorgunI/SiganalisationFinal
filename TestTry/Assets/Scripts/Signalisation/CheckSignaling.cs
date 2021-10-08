using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(Signaling))]
[RequireComponent(typeof(AudioVolumeChanger))]

public class CheckSignaling : MonoBehaviour
{
    private Signaling _signaling;
    private AudioVolumeChanger _audioVolumeChanger;

    private void OnEnable()
    {
        _signaling = GetComponent<Signaling>();
        _audioVolumeChanger = GetComponent<AudioVolumeChanger>();

        _signaling.Signaled += OnEndPointReached;
    }

    private void OnDisable()
    {
        _signaling.Signaled -= OnEndPointReached;
    }

    private void OnEndPointReached()
    {
        if (_signaling.IsSignaling == false)
        {
            _audioVolumeChanger.FadeOutVolume();
            return;
        }

        _audioVolumeChanger.IncreaseVolume();
    }
}
