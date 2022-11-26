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
/// Switches decal material.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Decal")]
public class HR_VehicleUpgrade_Decal : MonoBehaviour {

    private HR_VehicleUpgrade_DecalManager decalManager;        //  Decal manager.
    private MeshRenderer decal;     //  Renderer.
    internal int lastSelected = -1;     //  Last selected decal material index.

    private void Awake() {

        decal = GetComponentInChildren<MeshRenderer>();       //  Getting mesh renderer.
        decalManager = GetComponentInParent<HR_VehicleUpgrade_DecalManager>();      //  Getting decal manager.
        lastSelected = PlayerPrefs.GetInt(transform.root.name + transform.name, -1);        //  Getting last selected decal material.

        // If last selected found, set it.
        if (lastSelected == -1)
            decal.material = decalManager.nullMaterial;
        else
            UpdateDecal(GetComponentInParent<HR_VehicleUpgrade_DecalManager>().materials[lastSelected]);

    }

    /// <summary>
    /// Updates decal with target material.
    /// </summary>
    /// <param name="mat"></param>
    public void UpdateDecal(Material mat) {

        decal.material = mat;
        PlayerPrefs.SetInt(transform.root.name + transform.name, lastSelected);

    }

    /// <summary>
    /// Clears decal material.
    /// </summary>
    public void ClearDecal() {

        PlayerPrefs.SetInt(transform.root.name + transform.name, -1);
        decal.material = decalManager.nullMaterial;

    }

}
