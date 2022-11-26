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
/// Modification manager used with UI.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Main Menu/HR Mod Handler")]
public class HR_ModHandler : RCC_Singleton<HR_ModHandler> {

    //Classes
    private HR_ModApplier currentApplier;       //	Current applier component of the player car.

    //UI Panels.
    [Header("Modify Panels")]
    public GameObject colorClass;
    public GameObject wheelClass;
    public GameObject modificationClass;
    public GameObject upgradesClass;
    public GameObject decalsClass;
    public GameObject neonsClass;
    public GameObject spoilerClass;
    public GameObject sirenClass;

    //UI Buttons.
    [Header("Modify Buttons")]
    public Button bodyPaintButton;
    public Button rimButton;
    public Button customizationButton;
    public Button upgradeButton;
    public Button decalsButton;
    public Button neonsButton;
    public Button spoilersButton;
    public Button sirensButton;

    private Color orgButtonColor;

    //UI Texts.
    [Header("Upgrade Levels Texts")]
    public Text engineUpgradeLevel;
    public Text handlingUpgradeLevel;
    public Text brakeUpgradeLevel;
    public Text nosUpgradeLevel;
    public Text speedUpgradeLevel;

    // UI Sliders.
    [Header("Upgrade Sliders")]
    public Slider engine;
    public Slider handling;
    public Slider brake;
    public Slider speed;

    void Awake() {

        //Getting original color of the button.
        orgButtonColor = bodyPaintButton.image.color;

    }

    void Update() {

        //  Getting HR_ModApplier script of the player car.
        if (HR_MainMenuHandler.Instance.currentApplier)
            currentApplier = HR_MainMenuHandler.Instance.currentApplier;

        // If no any player car, return.
        if (!currentApplier)
            return;

        // Setting interactable states of the buttons depending on upgrade managers. 
        //	Ex. If spoiler manager not found, spoiler button will be disabled.
        upgradeButton.interactable = currentApplier.upgradeManager;
        decalsButton.interactable = currentApplier.decalManager;
        spoilersButton.interactable = currentApplier.spoilerManager;
        sirensButton.interactable = currentApplier.sirenManager;
        neonsButton.interactable = currentApplier.neonManager;
        rimButton.interactable = currentApplier.wheelManager;
        bodyPaintButton.interactable = currentApplier.paintManager;

        // Feeding upgrade level texts for enigne, brake, handling.
        if (currentApplier.upgradeManager) {

            if (engineUpgradeLevel)
                engineUpgradeLevel.text = currentApplier.upgradeManager.engineLevel.ToString("F0");
            if (handlingUpgradeLevel)
                handlingUpgradeLevel.text = currentApplier.upgradeManager.handlingLevel.ToString("F0");
            if (brakeUpgradeLevel)
                brakeUpgradeLevel.text = currentApplier.upgradeManager.brakeLevel.ToString("F0");
            if (nosUpgradeLevel)
                nosUpgradeLevel.text = currentApplier.upgradeManager.nosState ? "ON" : "OFF";
            if (speedUpgradeLevel)
                speedUpgradeLevel.text = currentApplier.upgradeManager.speedLevel.ToString("F0");

        }

        //  Displaying stats of the current car if found.
        if (currentApplier) {

            engine.value = Mathf.Lerp(.1f, 1f, (currentApplier.carController.maxEngineTorque) / 1000f);
            handling.value = Mathf.Lerp(.1f, 1f, currentApplier.carController.steerHelperAngularVelStrength / 1f);
            brake.value = Mathf.Lerp(.1f, 1f, currentApplier.carController.brakeTorque / 6000f);
            speed.value = Mathf.Lerp(.1f, 1f, currentApplier.carController.maxspeed / 400f);

        } else {

            engine.value = 0;
            handling.value = 0;
            brake.value = 0;
            speed.value = 0;

        }

    }

    /// <summary>
    /// Opens up the taget class panel.
    /// </summary>
    /// <param name="activeClass"></param>
    public void ChooseClass(GameObject activeClass) {

        colorClass.SetActive(false);
        wheelClass.SetActive(false);
        modificationClass.SetActive(false);
        upgradesClass.SetActive(false);
        decalsClass.SetActive(false);
        neonsClass.SetActive(false);
        spoilerClass.SetActive(false);
        sirenClass.SetActive(false);

        if (activeClass)
            activeClass.SetActive(true);

        CheckButtonColors(null);

    }

    /// <summary>
    /// Checks colors of the UI buttons. Ex. If paint class is enabled, color of the button will be green. 
    /// </summary>
    /// <param name="activeButton"></param>
    public void CheckButtonColors(Button activeButton) {

        bodyPaintButton.image.color = orgButtonColor;
        rimButton.image.color = orgButtonColor;
        customizationButton.image.color = orgButtonColor;
        upgradeButton.image.color = orgButtonColor;
        decalsButton.image.color = orgButtonColor;
        neonsButton.image.color = orgButtonColor;
        spoilersButton.image.color = orgButtonColor;
        sirensButton.image.color = orgButtonColor;

        if (activeButton)
            activeButton.image.color = new Color(0f, 1f, 0f);

    }

    /// <summary>
    /// Sets auto rotation of the showrooom camera.
    /// </summary>
    /// <param name="state"></param>
    public void ToggleAutoRotation(bool state) {

        Camera.main.gameObject.GetComponent<HR_ShowroomCamera>().ToggleAutoRotation(state);

    }

    /// <summary>
    /// Sets horizontal angle of the showroom camera.
    /// </summary>
    /// <param name="hor"></param>
    public void SetHorizontal(float hor) {

        Camera.main.gameObject.GetComponent<HR_ShowroomCamera>().orbitX = hor;

    }
    /// <summary>
    /// Sets vertical angle of the showroom camera.
    /// </summary>
    /// <param name="ver"></param>
    public void SetVertical(float ver) {

        Camera.main.gameObject.GetComponent<HR_ShowroomCamera>().orbitY = ver;

    }

    /// <summary>
    /// Sets decal location.
    /// </summary>
    /// <param name="index"></param>
    public void SetDecalIndex(int index) {

        HR_VehicleUpgrade_DecalManager dm = currentApplier.GetComponentInChildren<HR_VehicleUpgrade_DecalManager>();

        if (dm != null)
            dm.selectedIndex = index;

    }

}
