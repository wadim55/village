//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Modification applier for vehicles. Needs to be attached to the vehicle.
/// 7 Upgrade managers for paint, wheel, upgrade, neon, decal, spoiler, and siren.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Player/HR Mod Applier")]
public class HR_ModApplier : MonoBehaviour {

    #region All upgrade managers

    private HR_VehicleUpgrade_UpgradeManager _upgradeManager;
    public HR_VehicleUpgrade_UpgradeManager upgradeManager {

        get {

            if (_upgradeManager == null)
                _upgradeManager = GetComponentInChildren<HR_VehicleUpgrade_UpgradeManager>();

            return _upgradeManager;

        }

    }

    private HR_VehicleUpgrade_PaintManager _paintManager;
    public HR_VehicleUpgrade_PaintManager paintManager {

        get {

            if (_paintManager == null)
                _paintManager = GetComponentInChildren<HR_VehicleUpgrade_PaintManager>();

            return _paintManager;

        }

    }

    private HR_VehicleUpgrade_WheelManager _wheelManager;
    public HR_VehicleUpgrade_WheelManager wheelManager {

        get {

            if (_wheelManager == null)
                _wheelManager = GetComponentInChildren<HR_VehicleUpgrade_WheelManager>();

            return _wheelManager;

        }

    }

    private HR_VehicleUpgrade_DecalManager _decalManager;
    public HR_VehicleUpgrade_DecalManager decalManager {

        get {

            if (_decalManager == null)
                _decalManager = GetComponentInChildren<HR_VehicleUpgrade_DecalManager>();

            return _decalManager;

        }

    }

    private HR_VehicleUpgrade_SpoilerManager _spoilerManager;
    public HR_VehicleUpgrade_SpoilerManager spoilerManager {

        get {

            if (_spoilerManager == null)
                _spoilerManager = GetComponentInChildren<HR_VehicleUpgrade_SpoilerManager>();

            return _spoilerManager;

        }

    }

    private HR_VehicleUpgrade_NeonManager _neonManager;
    public HR_VehicleUpgrade_NeonManager neonManager {

        get {

            if (_neonManager == null)
                _neonManager = GetComponentInChildren<HR_VehicleUpgrade_NeonManager>();

            return _neonManager;

        }

    }

    private HR_VehicleUpgrade_SirenManager _sirenManager;
    public HR_VehicleUpgrade_SirenManager sirenManager {

        get {

            if (_sirenManager == null)
                _sirenManager = GetComponentInChildren<HR_VehicleUpgrade_SirenManager>();

            return _sirenManager;

        }

    }

    #endregion

    private RCC_CarControllerV3 _carController;
    public RCC_CarControllerV3 carController {

        get {

            if (!_carController)
                _carController = GetComponent<RCC_CarControllerV3>();

            return _carController;

        }

    }

}
