using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class myQueue : MonoBehaviour {
    private float timeleft;
    float x, y;
    int num_of_traveler = 0;
    Queue<GameObject> qu = new Queue<GameObject> ();
    Queue<GameObject> qu_after = new Queue<GameObject> ();
    string[] countries = { "Castle_d", "Castle", "Tree", "Volcano", "Any_country" };
    Dictionary<string, int> name2id = new Dictionary<string, int> () { { "Castle_d", 0 }, { "Castle", 1 }, { "Tree", 2 }, { "Volcano", 3 },
    };
    UnityEngine.Color[] country_colors = { Color.white, Color.blue, Color.green, Color.red, Color.yellow};

    // Start is called before the first frame update
    void Start () {
        //自身の色付け
        this.gameObject.GetComponent<Renderer>().material.color = country_colors[name2id[this.gameObject.name]]; 
    }

    // Update is called once per frame
    void Update () {
        //だいたい1秒ごとに待ち行列を増やす
        // timeleft -= Time.deltaTime;
        // if (timeleft <= 0.0) {
        //     timeleft = 3.0f;

        //     //行列が右と左どっちに伸びるか
        //     //float x_move = (this.transform.position.x>0) ? this.transform.position.x + qu.Count : this.transform.position.x - qu.Count;

        //     float pos_rnd_x = this.transform.position.x + Random.Range (-2.0f, 2.0f);
        //     float pos_rnd_y = this.transform.position.y + Random.Range (-2.0f, 2.0f);

        //     if (num_of_traveler < 50) {
        //         GameObject obj = Resources.Load ("Prefabs/Traveler") as GameObject;
        //         obj = Instantiate (obj, new Vector3 (pos_rnd_x, pos_rnd_y, 0.0f), Quaternion.identity);
        //         qu.Enqueue (obj);
        //         Traveler traveler = obj.GetComponent<Traveler> ();
        
        //         traveler.myCountry = this.gameObject;
        //         int idx = name2id[this.gameObject.name];
        //         //while (idx == name2id[this.gameObject.name]) 
        //         idx = Random.Range (0, 4); //今いる国以外の国番号を選択
                
        //         traveler.destination_country = GameObject.Find(countries[idx]);
        //         obj.GetComponent<Renderer>().material.color = country_colors[idx];
        //         num_of_traveler++;

        //         //queue = new GameObject[qu.Count];
        //         //qu.CopyTo(queue, 0);
        //     }

        //     //Debug.Log("myueue:"+qu.Count +" "+queue.Length);
        //     //if(i<5) i++;

        //     //show_Queue(queue);
        // }

    }

    // public void show_Queue(GameObject[] q){
    //     // float position_now = this.transform.position.x - qu.Count + 1;
    //     // Debug.Log("myqueue:" + position_now);
    //     Instantiate(q[q.Length-1], new Vector3(this.transform.position.x - (q.Length-1), y = this.transform.position.y, 0.0f), Quaternion.identity);

    // }

    // public void moveQueue () {
    //     //待ち行列を前へ移動
    //     for (int i = 0; i < qu.Count; i++) {
    //         GameObject game_obj = qu.Dequeue ();
    //         Traveler tl = game_obj.GetComponent<Traveler> ();
    //         tl.move_forward ();
    //         qu.Enqueue (game_obj);

    //     }
    // }

    // public GameObject Dequeue () {
    //     GameObject obj = qu.Dequeue ();

    //     moveQueue ();

    //     if (obj) return obj;
    //     else return null;
    // }
}