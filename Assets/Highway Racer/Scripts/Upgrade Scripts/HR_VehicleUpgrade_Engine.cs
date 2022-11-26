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
/// Upgrades engine of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Engine")]
public class HR_VehicleUpgrade_Engine : MonoBehaviour {

    private RCC_CarControllerV3 carController;

    private int _engineLevel = 0;
    public int engineLevel {
        get {
            return _engineLevel;
        }
        set {
            if (value <= 5)
                _engineLevel = value;
        }
    }

    private float defEngine;
    [HideInInspector] public float maxEngine = 750f;

    void Awake() {

        //  Getting car controller and default brake torque.
        carController = GetComponentInParent<RCC_CarControllerV3>();
        defEngine = carController.maxEngineTorque;

    }

    private void OnEnable() {

        //  Setting upgraded engine torque if saved.
        engineLevel = PlayerPrefs.GetInt(transform.root.name + "EngineLevel");
        carController.maxEngineTorque = Mathf.Lerp(defEngine, maxEngine, engineLevel / 5f);

    }

    /// <summary>
    /// Updates engine torque and save it.
    /// </summary>
    public void UpdateStats() {

        carController.maxEngineTorque = Mathf.Lerp(defEngine, maxEngine, engineLevel / 5f);
        PlayerPrefs.SetInt(transform.root.name + "EngineLevel", engineLevel);

    }

    void Update() {

        //  Make sure max torque is not smaller.
        if (maxEngine < carController.maxEngineTorque)
            maxEngine = carController.maxEngineTorque;

    }

}
