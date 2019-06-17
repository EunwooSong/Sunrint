using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPlayerColor : MonoBehaviour {

    private PhotonView pv;

	// Use this for initialization
	void Start ()
    {
        pv = GetComponent<PhotonView>();    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //자신 탱크 색 - 하늘색
		if (pv.isMine) GetComponent<MeshRenderer>().material.color = new Color(0.412f, 0.757f, 1.0f, 1.0f);
        //상대방 탱크 색 - 빨강색
        else GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.259f, 0.129f, 1.0f);
    }
}
