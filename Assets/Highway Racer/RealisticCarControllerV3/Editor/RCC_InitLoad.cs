//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2022 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public class RCC_InitLoad : EditorWindow {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    public static void EditorUpdate() {

        bool hasKey = false;

#if BCG_RCC
        hasKey = true;
#endif

        if (!hasKey) {

            if (!EditorPrefs.HasKey("BCG_RCC" + RCC_Version.version)) {

                EditorPrefs.SetInt("BCG_RCC" + RCC_Version.version, 1);
                EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing and using Realistic Car Controller. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started!");
                EditorUtility.DisplayDialog("New Input System", "RCC is using new input system. Legacy input system is deprecated. Make sure your project has Input System installed through the Package Manager. Import screen will ask you to install dependencies, choose Yes.", "Ok");

            }

        } else {

            RCC_SetScriptingSymbol.SetEnabled("BCG_RCC", true);

        }

        //RCC_Installation.Check();

    }

}
