using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwCtrl : MonoBehaviour {

    public int damage = 20;

    // Use this for initialization
    void Start () {
        StartCoroutine(this.SWctrl(0.02f));
	}

    IEnumerator SWctrl(float tm)
    {
        yield return new WaitForSeconds(tm);
        Destroy(this.gameObject, 0.0f);
    }
}
