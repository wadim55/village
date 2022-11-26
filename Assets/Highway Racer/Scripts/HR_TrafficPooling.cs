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

[AddComponentMenu("BoneCracker Games/Highway Racer/Traffic/HR Traffic Pooling")]
public class HR_TrafficPooling : MonoBehaviour {

    #region SINGLETON PATTERN
    public static HR_TrafficPooling instance;
    public static HR_TrafficPooling Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<HR_TrafficPooling>();
            return instance;
        }
    }
    #endregion

    public Transform[] lines;       // Traffic lines.

    private bool animateNow {       //  Animate the traffic now?
        get {
            return HR_GamePlayHandler.Instance.gameStarted;
        }
    }

    public TrafficCars[] trafficCars;       //  Traffic cars.

    [System.Serializable]
    public class TrafficCars {

        public GameObject trafficCar;
        public int frequence = 1;

    }

    private List<HR_TrafficCar> _trafficCars = new List<HR_TrafficCar>();       //  Spawned traffic cars.
    internal GameObject container;      //  Container of the spawned traffic cars.

    void Start() {

        CreateTraffic();

    }

    void Update() {

        if (animateNow)
            AnimateTraffic();

    }

    /// <summary>
    /// Spawns all traffic cars.
    /// </summary>
    private void CreateTraffic() {

        //  Creating container for the spawned traffic cars.
        container = new GameObject("Traffic Container");

        for (int i = 0; i < trafficCars.Length; i++) {

            for (int k = 0; k < trafficCars[i].frequence; k++) {

                GameObject go = Instantiate(trafficCars[i].trafficCar, Vector3.zero, Quaternion.identity);
                _trafficCars.Add(go.GetComponent<HR_TrafficCar>());
                go.SetActive(false);
                go.transform.SetParent(container.transform, true);

            }

        }

    }

    /// <summary>
    /// Animates the traffic cars.
    /// </summary>
    private void AnimateTraffic() {

        //  If there is no camera, return.
        if (!Camera.main.transform)
            return;

        //  If traffic car is below the camera or too far away, realign.
        for (int i = 0; i < _trafficCars.Count; i++) {

            if (Camera.main.transform.position.z > (_trafficCars[i].transform.position.z + 15) || Camera.main.transform.position.z < (_trafficCars[i].transform.position.z - 400))
                ReAlignTraffic(_trafficCars[i]);

        }

    }

    /// <summary>
    /// Realigns the traffic car.
    /// </summary>
    /// <param name="realignableObject"></param>
    private void ReAlignTraffic(HR_TrafficCar realignableObject) {

        if (!realignableObject.gameObject.activeSelf)
            realignableObject.gameObject.SetActive(true);

        int randomLine = Random.Range(0, lines.Length);

        realignableObject.currentLine = randomLine;
        realignableObject.transform.position = new Vector3(lines[randomLine].position.x, lines[randomLine].position.y, (Camera.main.transform.position.z + (Random.Range(300, 375))));

        switch (HR_GamePlayHandler.Instance.mode) {

            case (HR_GamePlayHandler.Mode.OneWay):
                realignableObject.transform.rotation = Quaternion.identity;
                break;
            case (HR_GamePlayHandler.Mode.TwoWay):
                if (realignableObject.transform.position.x <= 0f)
                    realignableObject.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 180f, 0f);
                else
                    realignableObject.transform.rotation = Quaternion.identity;
                break;
            case (HR_GamePlayHandler.Mode.TimeAttack):
                realignableObject.transform.rotation = Quaternion.identity;
                break;
            case (HR_GamePlayHandler.Mode.Bomb):
                realignableObject.transform.rotation = Quaternion.identity;
                break;

        }

        realignableObject.OnReAligned();

        if (CheckIfClipping(realignableObject.triggerCollider))
            realignableObject.gameObject.SetActive(false);

    }

    /// <summary>
    /// Checks if the new aligned car is clipping with another traffic car.
    /// </summary>
    /// <param name="trafficCarBound"></param>
    /// <returns></returns>
    private bool CheckIfClipping(BoxCollider trafficCarBound) {

        for (int i = 0; i < _trafficCars.Count; i++) {

            if (!trafficCarBound.transform.IsChildOf(_trafficCars[i].transform) && _trafficCars[i].gameObject.activeSelf) {

                if (HR_BoundsExtension.ContainBounds(trafficCarBound.transform, trafficCarBound.bounds, _trafficCars[i].triggerCollider.bounds))
                    return true;

            }

        }

        return false;

    }

}
