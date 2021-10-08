using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class Signaling : MonoBehaviour
{
    private UnityEvent _signaled = new UnityEvent();

    private AudioSource _audioAlarm;

    public bool IsSignaling { get; private set; }

    public event UnityAction Signaled
    {
        add => _signaled.AddListener(value);
        remove => _signaled.RemoveListener(value);
    }

    private void Awake()
    {
        _audioAlarm = GetComponent<AudioSource>();

        IsSignaling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            _audioAlarm.loop = !IsSignaling;
            IsSignaling = !IsSignaling;

            _signaled?.Invoke();
        }
    }
}