using System;
using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    public Transform mainCamera;
    public Vector3 positionStrength;
    public Vector3 rotationStrength;

    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }
    private void OnEnable() => Shake += CameraShake;
    private void OnDisable() => Shake -= CameraShake;

    private void CameraShake()
    {
        mainCamera.DOComplete();
        mainCamera.DOShakePosition(0.3f, positionStrength);
        mainCamera.DOShakeRotation(0.3f, rotationStrength);
    }
}
