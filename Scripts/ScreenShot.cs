using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    //ファイル名の指定
    [SerializeField,Tooltip("ファイル名の末尾に付く文字")]
    private string _imageTitle = "img";

    //保存先の指定 (末尾に / を付けてください)
    [SerializeField,Tooltip("ファイルの保存先 末尾の/ を含めてください")]
    private string _imagePath = "Assets/ScreenShots/";

    //タイムスタンプの形式
    [SerializeField]
    private int _timeStampType = 0;

    //撮影ボタンの表示切替
    [SerializeField,Tooltip("trueならGUIの撮影ボタンを表示します")]
    private bool _shotButtonActive = false;

    void Update()
    {
        //「Q」でボタンの表示切替
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _shotButtonActive = !_shotButtonActive;
        }

        //「P」で撮影
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(imageShooting(_imagePath,_imageTitle));
        }
    }

    //撮影ボタン設定
    void OnGUI()
    {
        if (_shotButtonActive == false) { return; }

        if (GUI.Button(new Rect(10, 10, 40, 20), "Shot"))
        {
            StartCoroutine(imageShooting(_imagePath,_imageTitle));
        }
    }

    //撮影処理
    //第一引数 ファイルパス / 第二引数 タイトル
    private IEnumerator imageShooting(string path, string title)
    {
        imagePathCheck(path);
        string name = getTimeStamp() + title + ".png";

        ScreenCapture.CaptureScreenshot(path + name);

        Debug.Log("Title: " + name);
        Debug.Log("Directory: " + path);
        AssetDatabase.Refresh();
        yield break;
    }

    //ファイルパスの確認
    private void imagePathCheck(string path)
    {
        if (Directory.Exists(path))
        {
            Debug.Log("The path exists");
        }
        else
        {
            //パスが存在しなければフォルダを作成
            Directory.CreateDirectory(path);
            Debug.Log("CreateFolder: " + path);
        }
    }

    //タイムスタンプ
    private string getTimeStamp()
    {
        string time;

        //タイムスタンプの設定書き足せます
        // 0 数字の連番 / 20180101125959
        // 1 数字の連番(年無し) / 0101125959
        // 2 年月日付き / 2018年01月01日12時59分59秒
        // 3 年月日付き(年無し) / 01月01日12時59分59秒
        switch (_timeStampType)
        {
            case 0:
                time = DateTime.Now.ToString("yyyyMMddHHmmss");
                return time;
                break;
            case 1:
                time = DateTime.Now.ToString("MMddHHmmss");
                return time;
                break;
            case 2:
                time = DateTime.Now.ToString("yyyy年MM月dd日HH時mm分ss秒");
                return time;
                break;
            case 3:
                time = DateTime.Now.ToString("MM月dd日HH時mm分ss秒");
                return time;
                break;
            default:
                time = DateTime.Now.ToString("yyyyMMddHHmmss");
                return time;
                break;
        }
    }
}
