using System.Collections;
using System.Collections.Generic;
using StateManager;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    TouchManager _touch_manager;
    Vector2 pos_before = new Vector2 ();
    Vector2 pos_after = new Vector2 ();

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

        // タッチされていたら処理
        if (touch_state._touch_flag) {
            //this.transform.position = Camera.main.ScreenToWorldPoint (touch_state._touch_position);


            if (touch_state._touch_phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) {
                // タッチした瞬間の処理   
                pos_before = Camera.main.ScreenToWorldPoint (touch_state._touch_position);
                Debug.Log ("began:" + pos_before + "" + pos_after);
            }
            if (touch_state._touch_phase == TouchPhase.Moved) {
                // 押している間の処理 
                pos_after = Camera.main.ScreenToWorldPoint (touch_state._touch_position);

                Vector2 delta_position = pos_after - pos_before;
                this.transform.position = new Vector3(delta_position.x, delta_position.y, -1f);
                pos_before = pos_after;

            }
        }

        Debug.Log (Camera.main.ScreenToWorldPoint (touch_state._touch_position));
        Debug.Log (touch_state._touch_position);
    }
}