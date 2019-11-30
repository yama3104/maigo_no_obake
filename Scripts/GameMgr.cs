using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {
    private float timeleft = 1.0f;
    static public int num_of_traveler;

    string[] countries = { "Castle_d", "Castle", "Tree", "Volcano", "Any_country" };
    Dictionary<string, int> name2id = new Dictionary<string, int> () { { "Castle_d", 0 }, { "Castle", 1 }, { "Tree", 2 }, { "Volcano", 3 }, };
    UnityEngine.Color[] country_colors = { Color.white, Color.blue, Color.green, Color.red, new Color (1, 1, 0, 1) };

    // 列を減らすキューをスタート地点から決める
    Dictionary<string, string> start2Queue = new Dictionary<string, string> () { { "Tree", "Tree_Queue" }, { "Volcano", "Volcano_Queue" }, { "Castle", "Castle_Queue" }, { "Castle_d", "Castle_d_Queue" }, };
    GameObject startGameObject, endGameObject;
    bool startSelected = false;
    private float time = 3000;
    public Text timer;

    readonly int NORMAL_MAX_NUM_OF_TRAVELER = 4;
    readonly int FEVER_MAX_NUM_OF_TRAVELER = 200;
    int max_num_of_traveler; //フィーバータイム作って変えるのもいいかも
    AudioSource audioSource;
    public AudioClip delete_se;

    // Start is called before the first frame update
    void Start () {
        audioSource = GetComponent<AudioSource> ();

        timer = GameObject.Find ("timer").GetComponent<Text> ();

        Traveler.resetScore ();

        num_of_traveler = 0;

        max_num_of_traveler = NORMAL_MAX_NUM_OF_TRAVELER;
        while (num_of_traveler < max_num_of_traveler) make_Travelers ();
    }

    // Update is called once per frame
    void Update () {
        // ゲーム進行のタイマー
        time -= Time.deltaTime;
        if (time < 0) {
            time = 0;
            SceneManager.LoadScene ("ResultScene");
        }
        timer.text = ((int) time).ToString ();

        Debug.Log ("update: " + num_of_traveler);
        if (num_of_traveler < max_num_of_traveler) StartCoroutine ("restock_travelers");
    }

    bool isRunning = false;
    IEnumerator restock_travelers () {
        if (isRunning) yield break;
        isRunning = true;

        yield return new WaitForSeconds (1);
        while (num_of_traveler < max_num_of_traveler) make_Travelers ();

        //Debug.Log ("coroutine: " + num_of_traveler);
        isRunning = false;
    }

    public void make_Travelers () {
        //行列が右と左どっちに伸びるか
        //float x_move = (this.transform.position.x>0) ? this.transform.position.x + qu.Count : this.transform.position.x - qu.Count;

        float pos_rnd_x = 8.0f, pos_rnd_y = 4.0f;

        while (!isUsablePosition (pos_rnd_x, pos_rnd_y)) {
            pos_rnd_x = this.transform.position.x + UnityEngine.Random.Range (-8.0f, 8.0f);
            pos_rnd_y = this.transform.position.y + UnityEngine.Random.Range (-4.0f, 4.0f);
        }

        GameObject obj = Resources.Load ("Prefabs/Traveler") as GameObject;
        obj = Instantiate (obj, new Vector3 (pos_rnd_x, pos_rnd_y, 0.0f), Quaternion.identity);

        Traveler traveler = obj.GetComponent<Traveler> ();

        //traveler.myCountry = this.gameObject;
        int idx = UnityEngine.Random.Range (0, countries.Length);

        traveler.destination_country = GameObject.Find (countries[idx]);
        obj.GetComponent<Renderer> ().material.color = country_colors[idx];
        num_of_traveler++;
    }

    // public static void show_combo (int groupSize) {
    //     StartCoroutine ("showCombo", groupSize);
    // }

    IEnumerator showCombo (int groupSize) {
        GameObject obj = Resources.Load ("Prefabs/Text") as GameObject;
        obj = Instantiate (obj, new Vector3 (0.0f, 0.0f, 0.0f), Quaternion.identity);
        GameObject canvas = GameObject.Find ("Canvas");
        obj.transform.SetParent (canvas.transform, false);
        Text t = obj.GetComponent<Text> ();
        t.text = groupSize.ToString ();

        yield return new WaitForSeconds (1);

        Destroy (obj);
    }

    public int get_num_of_traveler () {
        return num_of_traveler;
    }
    public void decrement_num_of_traveler () {
        Debug.Log ("呼ばれた！ in gamengr");
        num_of_traveler--;
    }

    bool isUsablePosition (float x, float y) {
        return getDistance (x, y, 8.0f, 4.0f) > 2 && getDistance (x, y, -8.0f, 4.0f) > 2 &&
            getDistance (x, y, 8.0f, -4.0f) > 2 && getDistance (x, y, -8.0f, -4.0f) > 2;
    }

    int getDistance (float x, float y, float x2, float y2) {
        double distance = Math.Sqrt ((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y));
        return (int) distance;
    }

    public void play_delete_se () {
        audioSource.PlayOneShot (delete_se);
    }
}

// if (Input.GetMouseButtonDown (0)) {
//     if (!startSelected) {
//         //スタート地点選択
//         startGameObject = null;
//         Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//         RaycastHit2D hit2d = Physics2D.Raycast ((Vector2) ray.origin, (Vector2) ray.direction);
//         if (hit2d) {
//             startGameObject = hit2d.transform.gameObject;
//             //startGameObject.GetComponent<Renderer>().material.color = Color.red;
//             startSelected = true;
//         }

//         //Debug.Log ("gamemgr:" + startGameObject.name);
//     } else {
//         //ゴール地点選択
//         endGameObject = null;
//         Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//         RaycastHit2D hit2d = Physics2D.Raycast ((Vector2) ray.origin, (Vector2) ray.direction);
//         if (hit2d) {
//             endGameObject = hit2d.transform.gameObject;
//             //startGameObject.GetComponent<Renderer> ().material.color = Color.white;
//             startSelected = false;
//             if (endGameObject == startGameObject) endGameObject = null;
//         }

//         if (endGameObject) {
//             //Debug.Log("gamemgr:"+endGameObject);
//             // タッチした場所にTraveler生成
//             myQueue mq = GameObject.Find (start2Queue[startGameObject.name]).GetComponent<myQueue> ();
//             GameObject obj = mq.Dequeue ();
//             Traveler traveler = obj.GetComponent<Traveler> ();
//             traveler.startTravel (endGameObject);

//             //GameObject obj = Resources.Load("Prefabs/Traveler") as GameObject;
//             //GameObject travel = Instantiate(obj, startGameObject.transform.position, Quaternion.identity);
//             //Traveler traveler = travel.GetComponent<Traveler>();
//         }
//     }
// }