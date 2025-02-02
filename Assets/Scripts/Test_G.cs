using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_G : MonoBehaviour
{
    public Camera camera;

    public float shakeDuration = 2f;
    public float shakeAmount = 0.7f;
    public int shakeCount = 10;

    public void Start()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = camera.transform.localPosition;
        for (int i = 0; i < shakeCount; i++)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;
            camera.transform.localPosition = new Vector3(x, y, originalPos.z);
            yield return new WaitForSeconds(shakeDuration / shakeCount);
        }
        camera.transform.localPosition = originalPos;

    }
}
