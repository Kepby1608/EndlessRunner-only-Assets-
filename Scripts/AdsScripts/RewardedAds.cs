using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAds : Singleton<RewardedAds>, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button buttonShowAd;

    [SerializeField] private string androidAdID = "Rewarded_Android";
    [SerializeField] private string iOSAdID = "Rewarded_iOS";

    public GameObject player;
    public Canvas preLosePanel;

    private string adID;
    private bool flag = false;

    private void Awake()
    {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSAdID
            : androidAdID;

        buttonShowAd.interactable = false; 
        flag = false;
    }

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        Advertisement.Load(adID, this);
    }

    public void ShowAd()
    {
        buttonShowAd.interactable = false;
        flag = true;
        Advertisement.Show(adID, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(adID))
        {
            buttonShowAd.onClick.AddListener(ShowAd);

            buttonShowAd.interactable = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adID}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adID}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    ////////////////////////////////////////////////////
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(adID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && flag)
        {
            Pause.Instance.ContinueGame();
            preLosePanel.gameObject.SetActive(false);
            PlayerController.Instance.flagDie = true;
            StartCoroutine(BlinkPlayerForSeconds(3f, 0.5f));
            StartCoroutine(DisableColliderForSeconds(3f));
            flag = false;
            PlayerController.Instance.pauseButton.gameObject.SetActive(true);
        }
    }

    private IEnumerator DisableColliderForSeconds(float seconds)
    {
        PlayerController.Instance.unTouchible = true;
        player.GetComponent<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(seconds);

        player.GetComponent<MeshCollider>().enabled = true;
        PlayerController.Instance.unTouchible = false;
    }

    private IEnumerator BlinkPlayerForSeconds(float totalSeconds, float blinkInterval)
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalSeconds)
        {
            player.GetComponent<MeshRenderer>().enabled = !player.GetComponent<MeshRenderer>().enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        player.GetComponent<MeshRenderer>().enabled = true;
    }
    ///////////////////////////////////////////////////

    private void OnDestroy()
    {
        buttonShowAd.onClick.RemoveAllListeners();
    }
}