using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.7f;

    private Vector3 originalPos;
    bool isShake = false;

    public void TriggerShake()
    {
        //Debug.Log("TriggerShake " + originalPos);
        originalPos = transform.localPosition;
        isShake = true;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            //Debug.Log("shake ");
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos; // Reset to original position
        isShake = false;
    }

    public bool getIsShake()
    {
        return isShake;
    }
}
