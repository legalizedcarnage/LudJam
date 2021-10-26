using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 pos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed  < duration) {
            float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;
            transform.localPosition = new Vector3(x,y,pos.z);
            elapsed+=Time.deltaTime;
            Debug.Log("shake");
            yield return null;
        }
        transform.localPosition = pos;
    }
}
