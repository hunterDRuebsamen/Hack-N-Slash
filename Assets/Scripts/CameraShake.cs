using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField, Tooltip("Intensity of Camera Shake")]
    float shakeIntensity = 0.5f;
    [SerializeField, Tooltip("Duration of Camera Shake")]
    float shakeDuration = 5.25f;

    const float shakeAmplitude = 2.0f;
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;

    void Start() {
        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
    }

    private IEnumerator Shake()
    {
        //Shake(shakeDuration, shakeIntensity);
        Debug.Log("Camera SHake");
        Noise(shakeAmplitude,shakeIntensity);
        yield return new WaitForSeconds(shakeDuration);
        Noise(0,0);
    }

    private void Noise(float amplitudeGain, float frequencyGain) {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;    
    }
    private void Shake_event(GameObject x)
    {
        StartCoroutine(Shake());
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