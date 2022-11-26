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
using UnityEditor;

[CustomEditor(typeof(HR_PlayerHandler))]
public class HR_PlayerEditor : Editor {

    HR_PlayerHandler prop;

    public override void OnInspectorGUI() {

        serializedObject.Update();
        prop = (HR_PlayerHandler)target;

        DrawDefaultInspector();

        if (!Application.isPlaying) {

            if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null) {

                EditorGUILayout.HelpBox("You'll need to create a new prefab for the vehicle first.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.red;

                if (GUILayout.Button("Create Prefab"))
                    CreatePrefab();

                GUI.color = defColor;

            } else {

                EditorGUILayout.HelpBox("Don't forget to save changes.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Save Prefab"))
                    SavePrefab();

                GUI.color = defColor;

            }

            bool foundPrefab = false;

            for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++) {

                if (HR_PlayerCars.Instance.cars[i].playerCar != null) {

                    if (prop.transform.name == HR_PlayerCars.Instance.cars[i].playerCar.transform.name) {

                        foundPrefab = true;
                        break;

                    }

                }

            }

            if (!foundPrefab) {

                EditorGUILayout.HelpBox("Player vehicles list doesn't include this vehicle yet!", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Add Prefab To Player Vehicles List")) {

                    if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null)
                        CreatePrefab();
                    else
                        SavePrefab();

                    AddToList();

                }

                GUI.color = defColor;

            }

        }

        serializedObject.ApplyModifiedProperties();

    }

    void CreatePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/Highway Racer/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Created Prefab");

    }

    void SavePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/Highway Racer/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Saved Prefab");

    }

    void AddToList() {

        List<HR_PlayerCars.Cars> playerCars = new List<HR_PlayerCars.Cars>();

        playerCars.Clear();
        playerCars.AddRange(HR_PlayerCars.Instance.cars);
        HR_PlayerCars.Cars newCar = new HR_PlayerCars.Cars();
        newCar.vehicleName = "New Player Vehicle " + Random.Range(0, 100).ToString();
        newCar.playerCar = PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject);
        playerCars.Add(newCar);
        HR_PlayerCars.Instance.cars = playerCars.ToArray();
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);
        Selection.activeObject = HR_PlayerCars.Instance;

        Debug.Log("Added Prefab To The Player Vehicles List");

    }

}
