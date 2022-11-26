//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Player manager that containts current score, near misses.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RCC_CarControllerV3))]
[RequireComponent(typeof(HR_ModApplier))]
[AddComponentMenu("BoneCracker Games/Highway Racer/Player/HR Player Handler")]
public class HR_PlayerHandler : MonoBehaviour {

    private RCC_CarControllerV3 carController;      //	Car controller.
    private Rigidbody rigid;        //	Rigidbody.

    [Range(250f, 1000f)] public float maxEngineTorque = 300f;        //	Maximum upgradable engine torque.
    [Range(2000f, 6000f)] public float maxBrakeTorque = 2000f;        //	Maximum upgradable brake torque.
    [Range(.1f, .5f)] public float maxHandlingStrength = .1f;     //	Maximum upgradable handling strength.
    [Range(200f, 400f)] public float maxSpeed = 360f;     //	Maximum upgradable speed.

    private bool gameOver = false;      //	Game is over now?
    private bool gameStarted { get { return HR_GamePlayHandler.Instance.gameStarted; } }

    internal float score;       //	Current score
    internal float timeLeft = 100f;     //	Time left.
    internal int combo;     //	Current near miss combo.
    internal int maxCombo;      //	Highest combo count.

    internal float speed = 0f;
    internal float distance = 0f;
    internal float highSpeedCurrent = 0f;
    internal float highSpeedTotal = 0f;
    internal float opposideDirectionCurrent = 0f;
    internal float opposideDirectionTotal = 0f;

    private int minimumSpeedForGainScore {
        get {
            return HR_HighwayRacerProperties.Instance._minimumSpeedForGainScore;
        }
    }
    private int minimumSpeedForHighSpeed {
        get {
            return HR_HighwayRacerProperties.Instance._minimumSpeedForHighSpeed;
        }
    }

    public int totalDistanceMoneyMP {
        get {
            return HR_HighwayRacerProperties.Instance._totalDistanceMoneyMP;
        }
    }
    public int totalNearMissMoneyMP {
        get {
            return HR_HighwayRacerProperties.Instance._totalNearMissMoneyMP;
        }
    }
    public int totalOverspeedMoneyMP {
        get {
            return HR_HighwayRacerProperties.Instance._totalOverspeedMoneyMP;
        }
    }
    public int totalOppositeDirectionMP {
        get {
            return HR_HighwayRacerProperties.Instance._totalOppositeDirectionMP;
        }
    }

    private Vector3 previousPosition;

    private string currentTrafficCarNameLeft;
    private string currentTrafficCarNameRight;

    internal int nearMisses;
    private float comboTime;

    internal bool bombTriggered = false;
    internal float bombHealth = 100f;

    private AudioSource hornSource;

    public delegate void onPlayerSpawned(HR_PlayerHandler player);
    public static event onPlayerSpawned OnPlayerSpawned;

    public delegate void onNearMiss(HR_PlayerHandler player, int score, HR_UIDynamicScoreDisplayer.Side side);
    public static event onNearMiss OnNearMiss;

    public delegate void onPlayerDied(HR_PlayerHandler player, int[] scores);
    public static event onPlayerDied OnPlayerDied;

    void Awake() {

        //	Getting components.
        carController = GetComponent<RCC_CarControllerV3>();
        rigid = GetComponent<Rigidbody>();
        rigid.drag = 0f;
        rigid.angularDrag = 0.1f;

        HR_VehicleUpgrade_Engine upgradeEngine = GetComponentInChildren<HR_VehicleUpgrade_Engine>();
        HR_VehicleUpgrade_Brake upgradeBrake = GetComponentInChildren<HR_VehicleUpgrade_Brake>();
        HR_VehicleUpgrade_Handling upgradeHandling = GetComponentInChildren<HR_VehicleUpgrade_Handling>();
        HR_VehicleUpgrade_Speed upgradeSpeed = GetComponentInChildren<HR_VehicleUpgrade_Speed>();

        //	Setting maximum upgradable values to the correspondind component.
        if (upgradeEngine)
            upgradeEngine.maxEngine = maxEngineTorque;

        if (upgradeBrake)
            upgradeBrake.maxBrake = maxBrakeTorque;

        if (upgradeHandling)
            upgradeHandling.maxHandling = maxHandlingStrength;

        if (upgradeSpeed)
            upgradeSpeed.maxSpeed = maxSpeed;

    }

    void OnEnable() {

        //	Listening event when player spawned.
        if (OnPlayerSpawned != null)
            OnPlayerSpawned(this);

        //	If engine is not running, start the engine.
        if (!carController.engineRunning)
            carController.StartEngine();

        //	Creating horn audio source.
        hornSource = HR_CreateAudioSource.NewAudioSource(gameObject, "Horn", 10f, 100f, 1f, HR_HighwayRacerProperties.Instance.hornClip, true, false, false);

        CheckGroundGap();

    }

    void Update() {

        //	If scene doesn't include gameplay manager, return.
        if (!HR_GamePlayHandler.Instance)
            return;

        //	If game is not started yet, return.
        if (gameOver || !gameStarted)
            return;

        //	Speed of the car.
        speed = carController.speed;

        // Total distance traveled.
        distance += Vector3.Distance(previousPosition, transform.position) / 1000f;
        previousPosition = transform.position;

        //	Is speed is high enough, gain score.
        if (speed >= minimumSpeedForGainScore)
            score += carController.speed * (Time.deltaTime * .05f);

        //	If speed is higher than high speed, gain score.
        if (speed >= minimumSpeedForHighSpeed) {

            highSpeedCurrent += Time.deltaTime;
            highSpeedTotal += Time.deltaTime;

        } else {

            highSpeedCurrent = 0f;

        }

        // If car is at opposite direction, gain score.
        if (speed >= (minimumSpeedForHighSpeed / 2f) && transform.position.x <= 0f && HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.TwoWay) {

            opposideDirectionCurrent += Time.deltaTime;
            opposideDirectionTotal += Time.deltaTime;

        } else {

            opposideDirectionCurrent = 0f;

        }

        //	If mode is time attack, reduce the timer.
        if (HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.TimeAttack) {

            timeLeft -= Time.deltaTime;

            // If timer hits 0, game over.
            if (timeLeft < 0) {

                timeLeft = 0;
                GameOver();

            }

        }

        comboTime += Time.deltaTime;

        //	If game mode is bomb...
        if (HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.Bomb) {

            //	Bomb will be triggered below 80 km/h.
            if (speed > 80f) {

                if (!bombTriggered)
                    bombTriggered = true;

                else
                    bombHealth += Time.deltaTime * 5f;

            } else if (bombTriggered) {

                bombHealth -= Time.deltaTime * 10f;

            }

            bombHealth = Mathf.Clamp(bombHealth, 0f, 100f);

            //	If bomb health hits 0, blow and game over.
            if (bombHealth <= 0f) {

                GameObject explosion = Instantiate(HR_HighwayRacerProperties.Instance.explosionEffect, transform.position, transform.rotation);
                explosion.transform.SetParent(null);
                rigid.isKinematic = true;
                GameOver();

            }

        }

        if (comboTime >= 2)
            combo = 0;

        CheckStatus();

    }

    void FixedUpdate() {

        //	If scene doesn't include gameplay manager, return.
        if (!HR_GamePlayHandler.Instance)
            return;

        //	If game is started, check near misses with raycasts.
        if (!gameOver && gameStarted)
            CheckNearMiss();

    }

    /// <summary>
    /// Checks near vehicles by drawing raycasts to the left and right sides.
    /// </summary>
    private void CheckNearMiss() {

        RaycastHit hit;

        Debug.DrawRay(carController.COM.position, (-transform.right * 2f), Color.white);
        Debug.DrawRay(carController.COM.position, (transform.right * 2f), Color.white);

        // Raycasting to the left side.
        if (Physics.Raycast(carController.COM.position, (-transform.right), out hit, 2f, HR_HighwayRacerProperties.Instance.trafficCarsLayer) && !hit.collider.isTrigger) {

            //	If hits, get it's name.
            currentTrafficCarNameLeft = hit.transform.name;

        } else {

            if (currentTrafficCarNameLeft != null && speed > HR_HighwayRacerProperties.Instance._minimumSpeedForGainScore) {

                nearMisses++;
                combo++;
                comboTime = 0;

                if (maxCombo <= combo)
                    maxCombo = combo;

                score += 100f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
                OnNearMiss(this, (int)(100f * Mathf.Clamp(combo / 1.5f, 1f, 20f)), HR_UIDynamicScoreDisplayer.Side.Left);

                currentTrafficCarNameLeft = null;

            } else {

                currentTrafficCarNameLeft = null;

            }

        }

        // Raycasting to the right side.
        if (Physics.Raycast(carController.COM.position, (transform.right), out hit, 2f, HR_HighwayRacerProperties.Instance.trafficCarsLayer) && !hit.collider.isTrigger) {

            //	If hits, get it's name.
            currentTrafficCarNameRight = hit.transform.name;

        } else {

            if (currentTrafficCarNameRight != null && speed > HR_HighwayRacerProperties.Instance._minimumSpeedForGainScore) {

                nearMisses++;
                combo++;
                comboTime = 0;

                if (maxCombo <= combo)
                    maxCombo = combo;

                score += 100f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
                OnNearMiss(this, (int)(100f * Mathf.Clamp(combo / 1.5f, 1f, 20f)), HR_UIDynamicScoreDisplayer.Side.Right);

                currentTrafficCarNameRight = null;

            } else {

                currentTrafficCarNameRight = null;

            }

        }

        // Raycasting to the front side. Used for taking down the lane.
        if (Physics.Raycast(carController.COM.position, (transform.forward), out hit, 40f, HR_HighwayRacerProperties.Instance.trafficCarsLayer) && !hit.collider.isTrigger) {

            Debug.DrawRay(carController.COM.position, (transform.forward * 20f), Color.red);

            if (carController.highBeamHeadLightsOn)
                hit.transform.SendMessage("ChangeLines");

        }

        // Horn and siren.
        if (hornSource) {

            hornSource.volume = Mathf.Lerp(hornSource.volume, carController.highBeamHeadLightsOn ? 1f : 0f, Time.deltaTime * 25f);

            if (carController.highBeamHeadLightsOn) {

                HR_VehicleUpgrade_Siren upgradeSiren = GetComponentInChildren<HR_VehicleUpgrade_Siren>();

                if (upgradeSiren && upgradeSiren.isActiveAndEnabled)
                    hornSource.clip = HR_HighwayRacerProperties.Instance.sirenAudioClip;

                if (!hornSource.isPlaying)
                    hornSource.Play();

            } else {

                hornSource.Stop();

            }

        }

    }

    void OnCollisionEnter(Collision col) {

        //	If scene doesn't include gameplay manager, return.
        if (!HR_GamePlayHandler.Instance)
            return;

        //	Calculating collision impulse.
        float impulse = col.impulse.magnitude / 1000f;

        //	If impulse is below the limit, return.
        if (impulse < HR_HighwayRacerProperties.Instance._minimumCollisionForGameOver)
            return;

        // Resetting combo to 0.
        combo = 0;

        // If hit is not a traffic car, return.
        if ((1 << col.gameObject.layer) != HR_HighwayRacerProperties.Instance.trafficCarsLayer.value)
            return;

        // If mode is bomb mode, reduce the bomb health.
        if (HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.Bomb) {

            bombHealth -= impulse * 3f;
            return;

        }

        //	Freezing the car and game over.
        rigid.isKinematic = true;
        GameOver();

    }

    /// <summary>
    /// Checks position of the car. If exceeds limits, respawns it.
    /// </summary>
    private void CheckStatus() {

        if (rigid.isKinematic)
            return;

        if (!gameStarted)
            return;

        //	If speed is below 5, or X position of the car exceeds limits, respawn it.
        if (speed < 5f || Mathf.Abs(transform.position.x) > 10f || Mathf.Abs(transform.position.y) > 10f) {

            transform.position = new Vector3(0f, 2f, transform.position.z + 10f);
            transform.rotation = Quaternion.identity;
            rigid.angularVelocity = Vector3.zero;
            rigid.velocity = new Vector3(0f, 0f, 20f);

        }

    }

    /// <summary>
    /// Game Over.
    /// </summary>
    private void GameOver() {

        gameOver = true;
        carController.canControl = false;
        carController.engineRunning = false;

        int[] scores = new int[4];
        scores[0] = Mathf.FloorToInt(distance * totalDistanceMoneyMP);
        scores[1] = Mathf.FloorToInt(nearMisses * totalNearMissMoneyMP);
        scores[2] = Mathf.FloorToInt(highSpeedTotal * totalOverspeedMoneyMP);
        scores[3] = Mathf.FloorToInt(opposideDirectionTotal * totalOppositeDirectionMP);

        for (int i = 0; i < scores.Length; i++)
            HR_API.AddCurrency(scores[i]);

        OnPlayerDied(this, scores);

    }

    /// <summary>
    /// Eliminates ground gap distance on when spawned.
    /// </summary>
    void CheckGroundGap() {

        WheelCollider wheel = GetComponentInChildren<WheelCollider>();
        float distancePivotBetweenWheel = Vector3.Distance(new Vector3(0f, transform.position.y, 0f), new Vector3(0f, wheel.transform.position.y, 0f));

        RaycastHit hit;

        if (Physics.Raycast(wheel.transform.position, -Vector3.up, out hit, 10f))
            transform.position = new Vector3(transform.position.x, hit.point.y + distancePivotBetweenWheel + (wheel.radius / 1f) + (wheel.suspensionDistance / 2f), transform.position.z);

    }

    private void Reset() {

        carController = GetComponent<RCC_CarControllerV3>();
        rigid = GetComponent<Rigidbody>();

        maxEngineTorque = carController.maxEngineTorque + 50;
        maxBrakeTorque = carController.brakeTorque + 500;
        maxHandlingStrength = carController.steerHelperAngularVelStrength + .2f;
        maxSpeed = carController.maxspeed + 40f;

    }

}
