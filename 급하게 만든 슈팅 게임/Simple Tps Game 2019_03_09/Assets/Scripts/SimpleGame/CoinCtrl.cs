using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour {

    public float max;
    public float min;
    public int money;
    public float rotSpeed;

    // Update is called once per frame
    void Start()
    {
        money = (int)Random.Range(min, max);
        Destroy(this.gameObject, 20.0f);
    }

    void Update () {
        //계속 회전
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0), Space.World);
	}
}
