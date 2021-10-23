using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cinemach_Shake : MonoBehaviour
{
    [SerializeField, Tooltip("Intensity of Camera Shake")]
    float maganitude = 0.5f;
    [SerializeField, Tooltip("Duration of Camera Shake")]
    float duration_Time = 0.25f;

    private CinemachineVirtualCamera Camera;

    void Awake()
    {
        Camera = GetComponent<CinemachineVirtualCamera>();
    }
    void Shake()
    {
        float elapsed_Time = 0.0f;
        CinemachineBasicMultiChannelPerlin cineMachPerlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        while (elapsed_Time < duration_Time)
        {
            float intensity = Random.Range(-5f, 5f) * maganitude;

            Debug.Log("Camera should be shaking");
            
            cineMachPerlin.m_AmplitudeGain = maganitude;
            elapsed_Time += Time.deltaTime;
        }
        
    }

    void Shake_event(GameObject x)
    {
        CinemachineBasicMultiChannelPerlin cineMachPerlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Shake();
        cineMachPerlin.m_AmplitudeGain = 0;
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
