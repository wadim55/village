﻿//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class HR_HighwayRacerProperties : ScriptableObject {

    public static HR_HighwayRacerProperties instance;
    public static HR_HighwayRacerProperties Instance {

        get {

            if (instance == null)
                instance = Resources.Load("HR_HighwayRacerProperties") as HR_HighwayRacerProperties;

            return instance;

        }

    }

    public int _minimumSpeedForGainScore;
    public int _minimumSpeedForHighSpeed;
    public int _minimumCollisionForGameOver;

    public Color _defaultBodyColor;
    public bool _tiltCamera;
    public int _totalDistanceMoneyMP;
    public int _totalNearMissMoneyMP;
    public int _totalOverspeedMoneyMP;
    public int _totalOppositeDirectionMP;

    public bool _1MMoneyForTesting;

    public GameObject[] selectablePlayerCars;
    public GameObject[] upgradableWheels;
    public GameObject explosionEffect;

    public AudioClip[] mainMenuClips;
    public AudioClip[] gameplayClips;
    public AudioClip buttonClickAudioClip;
    public AudioClip nearMissAudioClip;
    public AudioClip labelSlideAudioClip;
    public AudioClip countingPointsAudioClip;
    public AudioClip bombTimerAudioClip;
    public AudioClip hornClip;
    public AudioClip sirenAudioClip;

    public LayerMask trafficCarsLayer;

}
