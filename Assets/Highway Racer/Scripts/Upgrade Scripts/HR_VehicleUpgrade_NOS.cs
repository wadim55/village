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
/// Upgrades NOS of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR NOS")]
public class HR_VehicleUpgrade_NOS : MonoBehaviour {

    private RCC_CarControllerV3 carController;
    [HideInInspector] public bool nosState = false;

    void Awake() {

        //  Getting car controller.
        carController = GetComponentInParent<RCC_CarControllerV3>();

    }

    private void OnEnable() {

        //  Setting NOS if saved.
        nosState = PlayerPrefs.HasKey(transform.root.name + "NOS");
        carController.useNOS = nosState;

    }

    /// <summary>
    /// Updates NOS and save it.
    /// </summary>
    public void UpdateStats() {

        carController.useNOS = nosState;

        if (nosState)
            PlayerPrefs.SetInt(transform.root.name + "NOS", 1);

    }

}
