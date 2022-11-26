//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Gameplay management. Spawns player vehicle, sets volume, set mods, listens player events.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Gameplay/HR Gameplay Handler")]
public class HR_GamePlayHandler : MonoBehaviour {

    #region SINGLETON PATTERN
    public static HR_GamePlayHandler _instance;
    public static HR_GamePlayHandler Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<HR_GamePlayHandler>();
            }

            return _instance;
        }
    }
    #endregion

    [Header("Time Of The Scene")]
    public DayOrNight dayOrNight;
    public enum DayOrNight { Day, Night }

    [Header("Current Mode")]
    internal Mode mode;
    internal enum Mode { OneWay, TwoWay, TimeAttack, Bomb }

    [Header("Spawn Location Of The Cars")]
    public Transform spawnLocation;

    private HR_PlayerHandler player;

    private int selectedCarIndex = 0;
    private int selectedModeIndex = 0;

    internal bool gameStarted = false;
    internal bool paused = false;
    private readonly float minimumSpeed = 20f;

    public delegate void onPaused();
    public static event onPaused OnPaused;

    public delegate void onResumed();
    public static event onResumed OnResumed;

    internal AudioSource gameplaySoundtrack;

    void Awake() {

        //  Make sure time scale is 1. We are setting volume to 0, we'll be increase it smoothly in update method.
        Time.timeScale = 1f;
        AudioListener.volume = 0f;
        AudioListener.pause = false;

        //  Creating soundtrack.
        if (HR_HighwayRacerProperties.Instance.gameplayClips != null && HR_HighwayRacerProperties.Instance.gameplayClips.Length > 0) {

            gameplaySoundtrack = HR_CreateAudioSource.NewAudioSource(gameObject, "GamePlay Soundtrack", 0f, 0f, .35f, HR_HighwayRacerProperties.Instance.gameplayClips[UnityEngine.Random.Range(0, HR_HighwayRacerProperties.Instance.gameplayClips.Length)], true, true, false);
            gameplaySoundtrack.volume = PlayerPrefs.GetFloat("MusicVolume", .35f);
            gameplaySoundtrack.ignoreListenerPause = true;

        }

        //  Getting selected player car index and mode index.
        selectedCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex");
        selectedModeIndex = PlayerPrefs.GetInt("SelectedModeIndex");

        //  Setting proper mode.
        switch (selectedModeIndex) {

            case 0:
                mode = Mode.OneWay;
                break;
            case 1:
                mode = Mode.TwoWay;
                break;
            case 2:
                mode = Mode.TimeAttack;
                break;
            case 3:
                mode = Mode.Bomb;
                break;

        }

    }

    void Start() {

        SpawnCar();     //  Spawning the player vehicle.
        StartCoroutine(WaitForGameStart());     //  And wait for the countdown.

    }

    void OnEnable() {

        //  Listening events of the player.
        HR_PlayerHandler.OnPlayerSpawned += HR_PlayerHandler_OnPlayerSpawned;
        HR_PlayerHandler.OnNearMiss += HR_PlayerHandler_OnNearMiss;
        HR_PlayerHandler.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;

    }

    /// <summary>
    /// When player spawned.
    /// </summary>
    /// <param name="player"></param>
    private void HR_PlayerHandler_OnPlayerSpawned(HR_PlayerHandler player) {

        gameStarted = false;
        RCC.SetControl(player.GetComponent<RCC_CarControllerV3>(), false);
        StartCoroutine(WaitForGameStart());

    }

    /// <summary>
    /// When player near misses traffic vehicle.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="score"></param>
    /// <param name="side"></param>
    void HR_PlayerHandler_OnNearMiss(HR_PlayerHandler player, int score, HR_UIDynamicScoreDisplayer.Side side) {



    }

    /// <summary>
    /// When player dies.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="scores"></param>
    void HR_PlayerHandler_OnPlayerDied(HR_PlayerHandler player, int[] scores) {

        StartCoroutine(OnGameOver(1f));

    }

    /// <summary>
    /// Countdown before the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForGameStart() {

        yield return new WaitForSeconds(4);

        RCC.SetControl(player.GetComponent<RCC_CarControllerV3>(), true);
        gameStarted = true;

    }

    void Update() {

        //  Adjusting volume smoothly.
        float targetVolume = 1f;

        if (AudioListener.volume < targetVolume && !paused && Time.timeSinceLevelLoad > .5f) {

            if (AudioListener.volume < targetVolume) {

                targetVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
                AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, targetVolume, Time.deltaTime);

            }

        }

    }

    /// <summary>
    /// Spawning player car.
    /// </summary>
    void SpawnCar() {

        player = (RCC.SpawnRCC(HR_PlayerCars.Instance.cars[selectedCarIndex].playerCar.GetComponent<RCC_CarControllerV3>(), spawnLocation.position, spawnLocation.rotation, true, false, true)).GetComponent<HR_PlayerHandler>();
        player.transform.position = spawnLocation.transform.position;
        player.transform.rotation = Quaternion.identity;

        RCC_Customization.LoadStats(player.GetComponent<RCC_CarControllerV3>());

        player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, minimumSpeed / 1.75f);

        StartCoroutine(CheckDayTime());

    }

    /// <summary>
    /// Checking time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckDayTime() {

        yield return new WaitForFixedUpdate();

        if (dayOrNight == DayOrNight.Night)
            player.GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = true;
        else
            player.GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = false;

    }

    /// <summary>
    /// Pauses the game after the crash and saves the highscore.
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator OnGameOver(float delayTime) {

        yield return new WaitForSecondsRealtime(delayTime);
        OnPaused();

        switch (mode) {

            case Mode.OneWay:
                PlayerPrefs.SetInt("bestScoreOneWay", (int)player.GetComponent<HR_PlayerHandler>().score);
                break;
            case Mode.TwoWay:
                PlayerPrefs.SetInt("bestScoreTwoWay", (int)player.GetComponent<HR_PlayerHandler>().score);
                break;
            case Mode.TimeAttack:
                PlayerPrefs.SetInt("bestScoreTimeAttack", (int)player.GetComponent<HR_PlayerHandler>().score);
                break;
            case Mode.Bomb:
                PlayerPrefs.SetInt("bestScoreBomb", (int)player.GetComponent<HR_PlayerHandler>().score);
                break;

        }

    }

    /// <summary>
    /// Main menu.
    /// </summary>
    public void MainMenu() {

        SceneManager.LoadScene(0);

    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void RestartGame() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    /// <summary>
    /// Pause or resume the game.
    /// </summary>
    public void Paused() {

        paused = !paused;

        if (paused)
            OnPaused();
        else
            OnResumed();

    }

    void OnDisable() {

        HR_PlayerHandler.OnPlayerSpawned -= HR_PlayerHandler_OnPlayerSpawned;
        HR_PlayerHandler.OnNearMiss -= HR_PlayerHandler_OnNearMiss;
        HR_PlayerHandler.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;

    }

}
