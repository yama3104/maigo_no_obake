using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;

public class Carrier : MonoBehaviour {
    TouchManager _touch_manager;

    //　FixedJoint
    private FixedJoint fixedJoint;
    //　外れる力
    [SerializeField]
    private float breakForce = 200f;
    //　外れる角度
    [SerializeField]
    private float breakTorque = 200f;

    Vector2 pos_before = new Vector2 ();
    Vector2 pos_after = new Vector2 ();
    //　刺さっているかどうか
    //private bool isSticking;

    // Start is called before the first frame update
    void Start () {
        // タッチ管理マネージャ生成
        this._touch_manager = new TouchManager ();
    }

    // Update is called once per frame
    void Update () {
        // タッチ状態更新
        this._touch_manager.update ();

        // タッチ取得
        TouchManager touch_state = this._touch_manager.getTouch ();

        //Debug.Log("touchposition"+touch_state._touch_position);

        // タッチされていたら処理
        if (touch_state._touch_flag) {

            Debug.Log (touch_state._touch_phase);

            if (touch_state._touch_phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) {
                // タッチした瞬間の処理   
                pos_before = Camera.main.ScreenToWorldPoint (touch_state._touch_position);
                Debug.Log ("began:" + pos_before + "" + pos_after);
            }
            if (touch_state._touch_phase == TouchPhase.Moved) {
                // 押している間の処理 
                pos_after = Camera.main.ScreenToWorldPoint (touch_state._touch_position);

                Vector2 delta_position = pos_after - pos_before;
                this.transform.Translate (delta_position.x, delta_position.y, 0);
                pos_before = pos_after;

            }

            //Debug.Log (pos_before + "" + pos_after);

        }

    }

    //　衝突ありの場合
    void OnCollisionEnter2D (Collision2D col) {

        JudgeEnemy (col.collider);

    }

    //　接触なしの場合
    void OnTriggerEnter2D (Collider2D col) {
        Debug.Log (col.gameObject.tag);

        JudgeEnemy (col);

    }

    //　敵かどうか判断しJointの設定をする
    void JudgeEnemy (Collider2D col) {

        if (col.gameObject.tag == "traveler") {
            if (this.transform.childCount <= 5) col.gameObject.transform.parent = this.transform;

            // if(fixedJoint == null) {
            // 	gameObject.AddComponent<FixedJoint>();
            // 	fixedJoint = GetComponent<FixedJoint>();
            // 	fixedJoint.connectedBody = col.gameObject.GetComponent<Rigidbody>();
            // 	fixedJoint.breakForce = breakForce;
            // 	fixedJoint.breakTorque = breakTorque;
            // 	fixedJoint.enableCollision = true;
            // 	fixedJoint.enablePreprocessing = true;
            // 	isSticking = true;
            // 	//　Rigidbodyの速度を0にし、スリープ状態にして止める
            // 	GetComponent <Rigidbody>().velocity = Vector3.zero;
            // 	GetComponent <Rigidbody>().Sleep ();
            // }
        } else {
            return;
        }
    }
}