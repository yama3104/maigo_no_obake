using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;
using UnityEngine.UI;
public class Traveler : MonoBehaviour {
    public static int point = 0;
    public GameObject other_obj;
    //public GameObject myCountry;
    public GameObject destination_country;
    UnityEngine.Color mycolor;
    Color myYellow = new Color (1, 1, 0, 1);
    TouchManager _touch_manager;
    bool isSelected = false; //ドラッグしてる本体
    bool isGrabbed = false; //くっついてる奴ら
    public Text score;
    readonly float SMALL_SIZE_TRAVELER = 2.0f;
    readonly float BIG_SIZE_TRAVELER = 6.0f;
    public static int size_of_traveler_group = 0;
    public float hue = 0.0f;
    public AudioClip grouping_se;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start () {
        // タッチ管理マネージャ生成
        this._touch_manager = new TouchManager ();

        audioSource = GetComponent<AudioSource> ();

        score = GameObject.Find ("score").GetComponent<Text> ();

        mycolor = this.gameObject.GetComponent<Renderer> ().material.color;

        StartCoroutine ("destroy_goldTraveler");
    }

    IEnumerator destroy_goldTraveler () {
        if (mycolor == myYellow) {
            //Debug.Log ("This is yellow fox!!!");
            //3秒停止
            yield return new WaitForSeconds (5);

            GameMgr mgr = new GameMgr ();
            mgr.decrement_num_of_traveler ();
            Destroy (this.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        // タッチ状態更新
        this._touch_manager.update ();

        // タッチ取得
        TouchManager touch_state = this._touch_manager.getTouch ();

        // Debug.Log (touch_state._touch_phase);

        // タッチされていたら処理
        if (touch_state._touch_flag) {
            Vector3 pos = Camera.main.ScreenToWorldPoint (touch_state._touch_position);

            //Debug.Log ("traveler:" + touch_state._touch_position);

            Ray ray = Camera.main.ScreenPointToRay (_touch_manager._touch_position);
            RaycastHit2D hit2d = Physics2D.Raycast ((Vector2) ray.origin, (Vector2) ray.direction);

            // タッチした瞬間の処理   
            if (touch_state._touch_phase == TouchPhase.Began || Input.GetMouseButtonDown (0)) {
                //Debug.Log ("タッチされた！:Traveler");
                if (hit2d && hit2d.transform.gameObject == this.gameObject) {
                    isSelected = true;
                    isGrabbed = true;

                    // クリックしたら拡大
                    if (isSelected) this.gameObject.transform.localScale = new Vector3 (BIG_SIZE_TRAVELER, BIG_SIZE_TRAVELER, 1);
                }
            }

            if (touch_state._touch_phase == TouchPhase.Ended) {
                isSelected = false;
                isGrabbed = false;

                this.gameObject.transform.localScale = new Vector3 (SMALL_SIZE_TRAVELER, SMALL_SIZE_TRAVELER, 1);
                delete_children ();
            }

            if (isSelected) {
                this.transform.position = new Vector3 (pos.x, pos.y, 0f);
            }
        }

        if (mycolor == myYellow) {
            hue += 0.02f;
            if (hue > 1.0f) hue = 0.0f;
            //HSVで指定
            this.GetComponent<Renderer> ().material.color = Color.HSVToRGB (hue, 1, 1);
        }
    }

    void OnTriggerEnter2D (Collider2D col) {
        //if (col.gameObject.tag == "tree")
        //travelerどうしくっつける
        if (col.gameObject.tag == "traveler") {
            if (isGrabbed &&
                !col.gameObject.GetComponent<Traveler> ().isGrabbed &&
                col.gameObject.GetComponent<Traveler> ().mycolor == mycolor &&
                this.transform.childCount <= 2) {

                col.gameObject.GetComponent<Traveler> ().isGrabbed = true;
                col.gameObject.transform.localScale = new Vector3 (BIG_SIZE_TRAVELER, BIG_SIZE_TRAVELER, 1);
                col.gameObject.transform.parent = this.transform;
                audioSource.PlayOneShot (grouping_se);
                //Debug.Log (size_of_traveler_group);
            }
        }

        GameObject root_traveler = getRootTraveler ();

        //ポイント加算
        root_traveler.GetComponent<Traveler> ().delete_all_travelers (col);

        //GameMgr gmr = GameObject.Find ("GameMgr").GetComponent<GameMgr> ();
        //if (size_of_traveler_group > 0) gmr.show_combo(size_of_traveler_group);
        //if(size_of_traveler_group>0) root_traveler.GetComponent<Traveler> ().show_combo (size_of_traveler_group);

        size_of_traveler_group = 0;

        // delete_this_ogject (col);
        // //このやり方だと直接の子供しか処理できない
        // foreach (Transform child in transform) {
        //     child.gameObject.GetComponent<Traveler> ().delete_this_ogject (col);
        // }
    }

    //一番上の親を探す
    GameObject getRootTraveler () {
        if (this.transform.parent == null) {
            return this.gameObject;
        } else {
            return this.transform.parent.gameObject.GetComponent<Traveler> ().getRootTraveler ();
        }
    }

    //TravelerのRootから子供を全部消す
    void delete_all_travelers (Collider2D col) {
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                child.gameObject.GetComponent<Traveler> ().delete_all_travelers (col);
            }
        }

        delete_this_ogject (col);

    }

    //ポイント加算、traveler削除
    void delete_this_ogject (Collider2D col) {
        if (!(col.gameObject.name == "Tree" || col.gameObject.name == "Volcano" || 　
                col.gameObject.name == "Castle" || col.gameObject.name == "Castle_d")) return;
        //Debug.Log ("traveler:" + other_obj);
        if (col.gameObject == destination_country) {
            point += 10;
        } else if (mycolor == myYellow) {
            point += 50;
        } else {
            point++;
        }
        score.text = point.ToString ();
        //audioSource.PlayOneShot (delete_se);
        GameMgr gmr = GameObject.Find ("GameMgr").GetComponent<GameMgr> ();
        gmr.play_delete_se ();

        size_of_traveler_group++; //消しながらグループの大きさをカウントする

        GameMgr mgr = new GameMgr ();
        mgr.decrement_num_of_traveler ();
        Destroy (this.gameObject);
    }

    //親子関係をリセット
    public void delete_children () {
        // Debug.Log ("子供削除");
        //Debug.Log (transform.childCount);
        //if (this.transform.parent == null) return; //一番上のノードなら終了
        if (transform.childCount > 0) {
            foreach (Transform child in transform) {
                child.gameObject.GetComponent<Traveler> ().delete_children ();
                //child.gameObject.transform.localScale = new Vector3 (2, 2, 1);
                //child.parent = null;
            }
        }

        this.transform.parent = null;
        this.transform.localScale = new Vector3 (SMALL_SIZE_TRAVELER, SMALL_SIZE_TRAVELER, 1);
    }

    public static int getScore () {
        return point;
    }

    public static void resetScore () {
        point = 0;
    }

    public void startTravel (GameObject gameObject) {
        //other_obj = GameObject.Find("tree");
        other_obj = gameObject;
        Vector2 other_obj_v;
        other_obj_v.x = other_obj.transform.position.x;
        other_obj_v.y = other_obj.transform.position.y;
        Vector2 v;
        v.x = other_obj_v.x - this.transform.position.x;
        v.y = other_obj_v.y - this.transform.position.y;
        GetComponent<Rigidbody2D> ().velocity = v / 3;
    }

    public void move_forward () {
        Vector3 pos = this.transform.position;
        pos.x = (pos.x > 0) ? pos.x - 1.0f : pos.x + 1.0f;
        this.transform.position = pos;
    }
}