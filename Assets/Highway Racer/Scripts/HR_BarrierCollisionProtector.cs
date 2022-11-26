﻿//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Limits the linear right and left velocity of the player vehicle when hits the trigger (barrier).
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Gameplay/HR Barrier Collision Protector")]
public class HR_BarrierCollisionProtector : MonoBehaviour {

    public CollisionSide collisionSide;
    public enum CollisionSide { Left, Right }

    void OnTriggerStay(Collider col) {

        RCC_CarControllerV3 carController = col.transform.root.GetComponent<RCC_CarControllerV3>();

        if (!carController)
            return;

        Rigidbody playerRigid = carController.rigid;

        playerRigid = carController.rigid;

        if (collisionSide == CollisionSide.Right)
            playerRigid.AddForce(-Vector3.right * 50f, ForceMode.Acceleration);
        else
            playerRigid.AddForce(Vector3.right * 50f, ForceMode.Acceleration);

        playerRigid.velocity = new Vector3(0f, playerRigid.velocity.y, playerRigid.velocity.z);
        playerRigid.angularVelocity = new Vector3(playerRigid.angularVelocity.x, 0f, 0f);

    }

    void OnDrawGizmos() {

        Gizmos.color = new Color(1f, .5f, 0f, .75f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);

    }

}
