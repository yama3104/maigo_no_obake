using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMgr : MonoBehaviour {
    private BannerView bannerView;
    bool isTest = false;

    // Start is called before the first frame update
    void Start () {
        string appId;
        if (isTest) {
            appId = "ca-app-pub-3940256099942544~3347511713"; //test Id
        } else {
            appId = "ca-app-pub-6378485568392983~2106753979"; //my app Id
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize (appId);

        this.RequestBanner ();
        //bannerView.Show ();
    }

    // Update is called once per frame
    void Update () {

    }

    //これじゃないとバナー広告消えない(Destroy関数は使えなかった)
    void OnDisable () {
        bannerView.Destroy ();
    }
    // ボタンが押されたとき呼び出される
    public void OnClick () {
        SceneManager.LoadScene ("GameScene");
    }

    private void RequestBanner () {
        string adUnitId;
        if (isTest) {
            adUnitId = "ca-app-pub-3940256099942544/6300978111";
        } else {
            adUnitId = "ca-app-pub-6378485568392983/8560481680";
        }

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder ().Build ();

        // Load the banner with the request.
        bannerView.LoadAd (request);
    }
}