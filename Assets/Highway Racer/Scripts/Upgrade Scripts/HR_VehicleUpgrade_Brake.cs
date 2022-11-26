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
/// Upgrades brake torque of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Brake")]
public class HR_VehicleUpgrade_Brake : MonoBehaviour {

    private RCC_CarControllerV3 carController;

    private int _brakeLevel = 0;
    public int brakeLevel {
        get {
            return _brakeLevel;
        }
        set {
            if (value <= 5)
                _brakeLevel = value;
        }
    }

    private float defBrake;
    [HideInInspector] public float maxBrake = 4000f;

    void Awake() {

        //  Getting car controller and default brake torque.
        carController = GetComponentInParent<RCC_CarControllerV3>();
        defBrake = carController.brakeTorque;

    }

    private void OnEnable() {

        //  Setting upgraded brake torque if saved.
        brakeLevel = PlayerPrefs.GetInt(transform.root.name + "BrakeLevel");
        carController.brakeTorque = Mathf.Lerp(defBrake, maxBrake, brakeLevel / 5f);

    }

    /// <summary>
    /// Updates brake torque and save it.
    /// </summary>
    public void UpdateStats() {

        carController.brakeTorque = Mathf.Lerp(defBrake, maxBrake, brakeLevel / 5f);
        PlayerPrefs.SetInt(transform.root.name + "BrakeLevel", brakeLevel);

    }

    void Update() {

        //  Make sure max brake is not smaller.
        if (maxBrake < carController.brakeTorque)
            maxBrake = carController.brakeTorque;

    }

}
