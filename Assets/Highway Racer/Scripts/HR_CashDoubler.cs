//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using UnityEngine.Advertisements;

//public class HR_CashDoubler : MonoBehaviour, IPointerClickHandler, IUnityAdsLoadListener, IUnityAdsShowListener {

//    public string _adUnitId;

//    private Button button;
//	private HR_PlayerHandler player;

//	public int totalDistanceMoneyMP {
//		get {
//			return HR_HighwayRacerProperties.Instance._totalDistanceMoneyMP;
//		}
//	}
//	public int totalNearMissMoneyMP {
//		get {
//			return HR_HighwayRacerProperties.Instance._totalNearMissMoneyMP;
//		}
//	}
//	public int totalOverspeedMoneyMP {
//		get {
//			return HR_HighwayRacerProperties.Instance._totalOverspeedMoneyMP;
//		}
//	}
//	public int totalOppositeDirectionMP {
//		get {
//			return HR_HighwayRacerProperties.Instance._totalOppositeDirectionMP;
//		}
//	}

//    public void Calculate(){

//		player = HR_GamePlayHandler.Instance.player.GetComponent<HR_PlayerHandler>();

//        int earnings = Mathf.FloorToInt(Mathf.Floor(player.distance * totalDistanceMoneyMP) + (player.nearMisses * totalNearMissMoneyMP) + Mathf.Floor(player.highSpeedTotal * totalOverspeedMoneyMP));
//		PlayerPrefs.SetInt("Currency", PlayerPrefs.GetInt("Currency", 0) + earnings);
//        GameObject.FindObjectOfType<HR_GameOverPanel>().totalMoney.text = (earnings * 2).ToString() + "(X2)";
//        Destroy(gameObject);

//	}

//	public void OnPointerClick(PointerEventData eventData) {

//        ShowAd();

//	}

//    void Awake() {

//        //Disable button until ad is ready to show
//        button = GetComponent<Button>();
//        button.interactable = false;

//        //Advertisement.Initialize("1234567");

//        if (!Advertisement.isInitialized) {

//            print("Unity ads not initialized. Use ''Advertisement.Initialize(''appId'');''");
//            button.interactable = false;
//            Destroy(gameObject);

//        }

//    }

//    // If the ad successfully loads, add a listener to the button and enable it:
//    public void OnUnityAdsAdLoaded(string adUnitId) {

//        Debug.Log("Ad Loaded: " + adUnitId);

//        if (adUnitId.Equals(_adUnitId)) {

//            // Configure the button to call the ShowAd() method when clicked:
//            button.onClick.AddListener(ShowAd);
//            // Enable the button for users to click:
//            button.interactable = true;

//        }

//    }

//    // Implement a method to execute when the user clicks the button.
//    public void ShowAd() {

//        // Disable the button: 
//        button.interactable = false;
//        // Then show the ad:
//        Advertisement.Show(_adUnitId, this);

//    }

//    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
//    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) {

//        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {

//            Debug.Log("Unity Ads Rewarded Ad Completed");
//            // Grant a reward.
//            Calculate();

//        }

//        if (adUnitId.Equals(_adUnitId) && !showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) {

//            Debug.Log("Unity Ads Rewarded Ad NOT Completed");
//            Destroy(gameObject);

//        }

//    }

//    // Implement Load and Show Listener error callbacks:
//    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) {

//        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
//        // Use the error details to determine whether to try to load another ad.

//    }

//    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {

//        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
//        // Use the error details to determine whether to try to load another ad.

//    }

//    public void OnUnityAdsShowStart(string adUnitId) { }
//    public void OnUnityAdsShowClick(string adUnitId) { }

//    void OnDestroy() {

//        // Clean up the button listeners:
//        button.onClick.RemoveAllListeners();

//    }

//}
