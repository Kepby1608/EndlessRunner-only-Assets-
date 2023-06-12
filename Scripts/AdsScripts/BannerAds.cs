using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{
    [SerializeField] BannerPosition bannerPosition;

    [SerializeField] private string androidID = "Banner_Android";
    [SerializeField] private string iOSID = "Banner_iOS";

    private string adId;

    private void Awake()
    {
        adId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? iOSID
            : androidID;
    }

    private void Start()
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        StartCoroutine(LoadAdBanner());
    }
    private IEnumerator LoadAdBanner()
    {
        yield return new WaitForSeconds(1f);
        LoadBanner();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(adId, options);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        ShowBannerAd();
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }
    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
        Advertisement.Banner.Show(adId, options);
    }
    private void OnBannerClicked() { }
    private void OnBannerShown() { }
    private void OnBannerHidden() { }
}
