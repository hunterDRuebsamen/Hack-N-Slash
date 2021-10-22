using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField, Tooltip("Intensity of Camera Shake")]
    float maganitude = 0.5f;
    [SerializeField, Tooltip("Duration of Camera Shake")]
    float duration_Time = 0.25f;

    void Shake(float duration, float mag)
    {
        Vector3 Original_position = transform.localPosition;
        float elapsed_Time = 0.0f;

        while (elapsed_Time < duration)
        {
            float x = Random.Range(-1f, 1f) * mag;
            float y = Random.Range(-1f, 1f) * mag;

            Debug.Log("Camera should be shaking");
            transform.localPosition = new Vector3(x, y, Original_position.x);
            elapsed_Time += Time.deltaTime;
        }
        Debug.Log("Camera should be reset to original position");
        transform.localPosition = Original_position;
    }

    void Shake_event(GameObject x)
    {
        Shake(duration_Time, maganitude);
    }

    private void OnEnable()
    {
        EnemyBase.onEnemyDeath += Shake_event;
    }
    private void OnDisable()
    {
        EnemyBase.onEnemyDeath -= Shake_event;
    }

}
