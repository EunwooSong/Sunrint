using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCtrl : MonoBehaviour
{
    public int damage = 100;
    public float speed = 1000.0f;
    
    void Start ()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);  //메테오 전진                               
        StartCoroutine(this.ExplosionMeteor(3.0f));                     //일정시간이 지난 후 메테오 삭제                  
    }

    IEnumerator ExplosionMeteor(float tm)
    {
        yield return new WaitForSeconds(tm);
        Destroy(this.gameObject, 0.0f);
    }
}
