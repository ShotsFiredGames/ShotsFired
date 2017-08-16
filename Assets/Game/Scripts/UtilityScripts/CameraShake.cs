using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;
    float shakeSpeed;

    void Start()
    {
        Shaking = false;
        shakeSpeed = .2f;
    }

    void Update()
    {
        if (ShakeIntensity > 0)
        {
            transform.localPosition = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            transform.localRotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * shakeSpeed,
                                      OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * shakeSpeed,
                                      OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * shakeSpeed,
                                      OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * shakeSpeed);

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            transform.localPosition = new Vector3(0,.75f,0);
            transform.localRotation = Quaternion.identity;
            Shaking = false;
        }
    }

    public void DoShake(float _shakeIntensity, float _shakeDecay, float _shakeSpeed)
    {
        OriginalPos = transform.localPosition;
        OriginalRot = transform.localRotation;

        ShakeIntensity = _shakeIntensity; //0.3f;
        ShakeDecay = _shakeDecay; //0.02f;
        shakeSpeed = _shakeSpeed; //.2f
        Shaking = true;
    }
}
