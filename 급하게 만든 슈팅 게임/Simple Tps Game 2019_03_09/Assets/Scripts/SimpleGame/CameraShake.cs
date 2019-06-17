using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    Vector3 originPos;

    void Start()
    {
        originPos = transform.localPosition;
    }

    public IEnumerator Shake(float power, float duration)
    {
        float timer = 0.0f;

        while (timer <= duration)
        {
            //insideUnitCircle - 반경(?)이 1인 구에서 랜덤하게 값을 정해 반환
            transform.localPosition = (Vector3)Random.insideUnitCircle * power + originPos;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originPos;
    }
}
