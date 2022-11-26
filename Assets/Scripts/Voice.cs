using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    [SerializeField] private AudioClip[] MyClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()  //подписываемся на событие при включенном объекте
    {
        EVENT.girlOffer += StartVoice;
    }

    private void OnDisable() //отписываемся от событие при отключенном объекте
    {
        EVENT.girlOffer -= StartVoice;
    }

    private void StartVoice(int numberVoice)
    {
        print(numberVoice);
        _audioSource.clip = MyClip[numberVoice];
        if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
    
}
