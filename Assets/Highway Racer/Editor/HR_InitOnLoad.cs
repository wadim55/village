//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2021 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HR_InitOnLoad {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    public static void EditorUpdate() {

        bool hasKey = false;

#if BCG_HR
        hasKey = true;
#endif

        if (!EditorPrefs.HasKey("BCG_HR" + HR_Version.version)) {

            EditorPrefs.SetInt("BCG_HR" + HR_Version.version, 1);
            EditorUtility.DisplayDialog("Restart", "Please restart your Unity Editor after the installation. Otherwise, inputs won't work properly.", "Ok");
            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing Highway Racer Complete Project. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started");
            EditorUtility.DisplayDialog("Current Controller Type", "Current controller type is ''Desktop''. You can swith it from Highway Racer --> Switch to Keyboard / Mobile. Also 1000000 cash is enabled by default. You can disable it from Highway Racer --> General Settings.", "Ok");
            Selection.activeObject = HR_HighwayRacerProperties.Instance;

        }

        if (!hasKey) {

            RCC_SetScriptingSymbol.SetEnabled("BCG_HR", true);

        }

    }

}