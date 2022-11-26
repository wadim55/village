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

[AddComponentMenu("BoneCracker Games/Highway Racer/UI/HR UI Gameover Panel")]
public class HR_UIGameOverPanel : MonoBehaviour {

    public GameObject content;

    [Header("UI Texts On Scoreboard")]
    public Text totalScore;
    public Text subTotalMoney;
    public Text totalMoney;

    public Text totalDistance;
    public Text totalNearMiss;
    public Text totalOverspeed;
    public Text totalOppositeDirection;

    public Text totalDistanceMoney;
    public Text totalNearMissMoney;
    public Text totalOverspeedMoney;
    public Text totalOppositeDirectionMoney;



    void OnEnable() {

        HR_PlayerHandler.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;

    }

    void HR_PlayerHandler_OnPlayerDied(HR_PlayerHandler player, int[] scores) {

        StartCoroutine(DisplayResults(player, scores));

    }

    public IEnumerator DisplayResults(HR_PlayerHandler player, int[] scores) {

        yield return new WaitForSecondsRealtime(1f);

        content.SetActive(true);

        totalScore.text = Mathf.Floor(player.score).ToString("F0");
        totalDistance.text = (player.distance).ToString("F2");
        totalNearMiss.text = (player.nearMisses).ToString("F0");
        totalOverspeed.text = (player.highSpeedTotal).ToString("F1");
        totalOppositeDirection.text = (player.opposideDirectionTotal).ToString("F1");

        totalDistanceMoney.text = scores[0].ToString("F0");
        totalNearMissMoney.text = scores[1].ToString("F0");
        totalOverspeedMoney.text = scores[2].ToString("F0");
        totalOppositeDirectionMoney.text = scores[3].ToString("F0");

        totalMoney.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();

        gameObject.BroadcastMessage("Animate");
        gameObject.BroadcastMessage("GetNumber");

    }

    void OnDisable() {

        HR_PlayerHandler.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;

    }

}
