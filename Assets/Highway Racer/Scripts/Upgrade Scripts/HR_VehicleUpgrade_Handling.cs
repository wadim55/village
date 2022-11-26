//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upgrades traction strength of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Handling")]
public class HR_VehicleUpgrade_Handling : MonoBehaviour {

    private RCC_CarControllerV3 carController;

    private int _handlingLevel = 0;
    public int handlingLevel {
        get {
            return _handlingLevel;
        }
        set {
            if (value <= 5)
                _handlingLevel = value;
        }
    }

    private float defHandling;
    [HideInInspector] public float maxHandling = .4f;

    void Awake() {

        //  Getting car controller and default traction helper strength.
        carController = GetComponentInParent<RCC_CarControllerV3>();
        defHandling = carController.steerHelperAngularVelStrength;

    }

    private void OnEnable() {

        //  Setting upgraded handling strength if saved.
        handlingLevel = PlayerPrefs.GetInt(transform.root.name + "HandlingLevel");
        carController.steerHelperAngularVelStrength = Mathf.Lerp(defHandling, maxHandling, handlingLevel / 5f);
        carController.steerHelperLinearVelStrength = Mathf.Lerp(defHandling, maxHandling, handlingLevel / 5f);

    }

    /// <summary>
    /// Updates handling strength and save it.
    /// </summary>
    public void UpdateStats() {

        carController.steerHelperAngularVelStrength = Mathf.Lerp(defHandling, maxHandling, handlingLevel / 5f);
        carController.steerHelperLinearVelStrength = Mathf.Lerp(defHandling, maxHandling, handlingLevel / 5f);
        PlayerPrefs.SetInt(transform.root.name + "HandlingLevel", handlingLevel);

    }

    void Update() {

        //  Make sure max handling is not smaller.
        if (maxHandling < carController.steerHelperAngularVelStrength)
            maxHandling = carController.steerHelperAngularVelStrength;

    }

}
