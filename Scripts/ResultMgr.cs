using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultMgr : MonoBehaviour {
    public Text score_result, highscore_result;
    private BannerView bannerView;
    bool isTest = false;
    readonly string key_highscore = "highscore";

    // Start is called before the first frame update
    void Start () {
        score_result = GameObject.Find ("score_result").GetComponent<Text> ();
        highscore_result = GameObject.Find ("highscore_result").GetComponent<Text> ();

        int score_now = Traveler.getScore ();
        score_result.text = $"スコア：{score_now.ToString()}";　　
        int highscore = 0;
        if (PlayerPrefs.HasKey (key_highscore)) highscore = PlayerPrefs.GetInt (key_highscore);
        highscore_result.text = $"ハイスコア：{highscore.ToString()}";
        if (score_now > highscore) {
            PlayerPrefs.SetInt (key_highscore, score_now);
            PlayerPrefs.Save ();
        }

        string appId;
        if (isTest) {
            appId = "ca-app-pub-3940256099942544~3347511713"; //test Id
        } else {
            appId = "ca-app-pub-6378485568392983~2106753979"; //my app Id
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize (appId);

        this.RequestBanner ();
    }

    // Update is called once per frame
    void Update () { }

    void OnDisable () {
        bannerView.Destroy ();
    }

    // ボタンが押されたとき呼び出される
    public void OnClick () {
        SceneManager.LoadScene ("TitleScene");
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