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
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Management of the main menu events. Creates and spawns vehicles, switches them, enables/disables menus.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Main Menu/HR Main Menu Handler")]
public class HR_MainMenuHandler : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_MainMenuHandler _instance;
    public static HR_MainMenuHandler Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<HR_MainMenuHandler>();
            }

            return _instance;
        }
    }
    #endregion

    [Header("Spawn Location Of The Cars")]
    public Transform carSpawnLocation;

    private GameObject[] createdCars;       //	All created cars will be stored here.
    public RCC_CarControllerV3 currentCar;      //	Current car.
    public HR_ModApplier currentApplier;        //  Current mod applier.

    internal int carIndex = 0;      //	Current car index.

    [Header("UI Menus")]
    public GameObject optionsMenu;
    public GameObject carSelectionMenu;
    public GameObject modsSelectionMenu;
    public GameObject sceneSelectionMenu;
    public GameObject creditsMenu;

    [Header("UI Loading Section")]
    public GameObject loadingScreen;
    public Slider loadingBar;
    private AsyncOperation async;

    [Header("Other UIs")]
    public Text currency;
    public GameObject buyCarButton;
    public GameObject selectCarButton;
    public GameObject modCarPanel;

    public Text vehicleNameText;        //	Vehicle name text.
    public Text bestScoreOneWay;        //	Best score one way text.
    public Text bestScoreTwoWay;        //	Best score two ways text.
    public Text bestScoreTimeLeft;      //	Best score time left text.
    public Text bestScoreBomb;      //	Best score bomb text.

    internal AudioSource mainMenuSoundtrack;

    private void Awake() {

        // Setting time scale, volume, unpause, and target frame rate.
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.pause = false;
        Application.targetFrameRate = 60;

        //	Creating soundtracks for the main menu.
        if (HR_HighwayRacerProperties.Instance.mainMenuClips != null && HR_HighwayRacerProperties.Instance.mainMenuClips.Length > 0) {

            mainMenuSoundtrack = HR_CreateAudioSource.NewAudioSource(gameObject, "Main Menu Soundtrack", 0f, 0f, PlayerPrefs.GetFloat("MusicVolume", .35f), HR_HighwayRacerProperties.Instance.mainMenuClips[UnityEngine.Random.Range(0, HR_HighwayRacerProperties.Instance.mainMenuClips.Length)], true, true, false);
            mainMenuSoundtrack.ignoreListenerPause = true;

        }

        //	If test mode enabled, add 1000000 coins to the balance.
        if (HR_HighwayRacerProperties.Instance._1MMoneyForTesting)
            PlayerPrefs.SetInt("Currency", 1000000);

        //	Getting last selected car index.
        carIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex", 0);

        CreateCars();   //	Creating all selectable cars at once.
        SpawnCar();     //	Spawning only target car (carIndex).

    }

    private void Update() {

        //	Displaying currency.
        currency.text = HR_API.GetCurrency().ToString("F0");

        //	If loading, set value of the loading slider.
        if (async != null && !async.isDone)
            loadingBar.value = async.progress;

    }

    /// <summary>
    /// Creating all spawnable cars at once.
    /// </summary>
    private void CreateCars() {

        //	Creating a new array.
        createdCars = new GameObject[HR_PlayerCars.Instance.cars.Length];

        //	Setting array elements.
        for (int i = 0; i < createdCars.Length; i++) {

            createdCars[i] = (RCC.SpawnRCC(HR_PlayerCars.Instance.cars[i].playerCar.GetComponent<RCC_CarControllerV3>(), carSpawnLocation.position, carSpawnLocation.rotation, false, false, false)).gameObject;
            createdCars[i].GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = true;
            createdCars[i].SetActive(false);

        }

    }

    /// <summary>
    /// Spawns target car (carIndex).
    /// </summary>
    private void SpawnCar() {

        //	If price of the car is 0, or unlocked, save it as owned car.
        if (HR_PlayerCars.Instance.cars[carIndex].price <= 0 || HR_PlayerCars.Instance.cars[carIndex].unlocked)
            HR_API.UnlockVehice(carIndex);

        //	If current spawned car is owned, enable buy button, disable select button. Do opposite otherwise.
        if (HR_API.OwnedVehicle(carIndex)) {

            //  Displaying price null.
            if (buyCarButton.GetComponentInChildren<Text>())
                buyCarButton.GetComponentInChildren<Text>().text = "";

            // Enabling select button, disabling buy button.
            buyCarButton.SetActive(false);
            selectCarButton.SetActive(true);
            modCarPanel.SetActive(true);

        } else {

            //  Displaying price.
            if (buyCarButton.GetComponentInChildren<Text>())
                buyCarButton.GetComponentInChildren<Text>().text = "BUY FOR\n" + HR_PlayerCars.Instance.cars[carIndex].price.ToString("F0");

            //  Enabling buy button, disabling select button.
            selectCarButton.SetActive(false);
            buyCarButton.SetActive(true);
            modCarPanel.SetActive(false);

        }

        //	Disabling all cars at once. And then enabling only target car (carIndex). And make sure spawned cars are always at spawn point.
        for (int i = 0; i < createdCars.Length; i++) {

            if (createdCars[i].activeInHierarchy) {

                createdCars[i].SetActive(false);
                createdCars[i].transform.position = carSpawnLocation.position;
                createdCars[i].transform.rotation = carSpawnLocation.rotation;

            }

        }

        //	Enabling only target car (carIndex).
        createdCars[carIndex].SetActive(true);

        //	Setting current car.
        currentCar = createdCars[carIndex].GetComponent<RCC_CarControllerV3>();
        currentApplier = currentCar.GetComponent<HR_ModApplier>();

        //	Displaying car name text.
        if (vehicleNameText)
            vehicleNameText.text = HR_PlayerCars.Instance.cars[carIndex].vehicleName;

        HR_ModHandler.Instance.ChooseClass(null);

    }

    /// <summary>
    /// Purchases current car.
    /// </summary>
    public void BuyCar() {

        // If we own the car, don't consume currency.
        if (HR_API.OwnedVehicle(carIndex)) {

            Debug.LogError("Car is already owned!");
            return;

        }

        //	If currency is enough, save it and consume currency. Otherwise display the informer.
        if (HR_API.GetCurrency() >= HR_PlayerCars.Instance.cars[carIndex].price) {

            HR_API.ConsumeCurrency(HR_PlayerCars.Instance.cars[carIndex].price);

        } else {

            HR_UIInfoDisplayer.Instance.ShowInfo("Not Enough Coins", "You have to earn " + (HR_PlayerCars.Instance.cars[carIndex].price - HR_API.GetCurrency()).ToString() + " more coins to buy this wheel", HR_UIInfoDisplayer.InfoType.NotEnoughMoney);
            return;

        }

        //	Saving the car.
        HR_API.UnlockVehice(carIndex);

        //	And spawning again to check modders of the car.
        SpawnCar();

    }

    /// <summary>
    /// Selects the current car with carIndex.
    /// </summary>
    public void SelectCar() {

        PlayerPrefs.SetInt("SelectedPlayerCarIndex", carIndex);

    }

    /// <summary>
    /// Switch to next car.
    /// </summary>
    public void PositiveCarIndex() {

        carIndex++;

        if (carIndex >= createdCars.Length)
            carIndex = 0;

        SpawnCar();

    }

    /// <summary>
    /// Switch to previous car.
    /// </summary>
    public void NegativeCarIndex() {

        carIndex--;

        if (carIndex < 0)
            carIndex = createdCars.Length - 1;

        SpawnCar();

    }

    /// <summary>
    /// Enables target menu and disables all other menus.
    /// </summary>
    /// <param name="activeMenu"></param>
    public void EnableMenu(GameObject activeMenu) {

        optionsMenu.SetActive(false);
        carSelectionMenu.SetActive(false);
        modsSelectionMenu.SetActive(false);
        sceneSelectionMenu.SetActive(false);
        creditsMenu.SetActive(false);
        loadingScreen.SetActive(false);

        activeMenu.SetActive(true);

        if (activeMenu == modsSelectionMenu)
            BestScores();

    }

    /// <summary>
    /// Selects the scene with int.
    /// </summary>
    /// <param name="levelIndex"></param>
    public void SelectScene(int levelIndex) {

        SelectCar();
        EnableMenu(loadingScreen);
        async = SceneManager.LoadSceneAsync(levelIndex);

    }

    /// <summary>
    /// Selects the mode with int.
    /// </summary>
    /// <param name="_modeIndex"></param>
    public void SelectMode(int _modeIndex) {

        //	Saving the selected mode, and enabling scene selection menu.
        PlayerPrefs.SetInt("SelectedModeIndex", _modeIndex);
        EnableMenu(sceneSelectionMenu);

    }

    /// <summary>
    /// Displays best scores of all four modes.
    /// </summary>
    private void BestScores() {

        int[] scores = HR_API.GetHighScores();

        bestScoreOneWay.text = "BEST SCORE\n" + scores[0];
        bestScoreTwoWay.text = "BEST SCORE\n" + scores[1];
        bestScoreTimeLeft.text = "BEST SCORE\n" + scores[2];
        bestScoreBomb.text = "BEST SCORE\n" + scores[3];

    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame() {

        Application.Quit();

    }

}
