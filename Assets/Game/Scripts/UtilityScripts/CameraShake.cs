using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public bool Shaking;
    private float ShakeDecay;
    private float ShakeIntensity;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;

    void Start()
    {
        Shaking = false;
    }

    void Update()
    {
        if (ShakeIntensity > 0)
        {
            transform.localPosition = OriginalPos + Random.insideUnitSphere * ShakeIntensity;
            transform.localRotation = new Quaternion(OriginalRot.x + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.y + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.z + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f,
                                      OriginalRot.w + Random.Range(-ShakeIntensity, ShakeIntensity) * .2f);

            ShakeIntensity -= ShakeDecay;
        }
        else if (Shaking)
        {
            transform.localPosition = new Vector3(0,.75f,0);
            transform.localRotation = Quaternion.identity;
            Shaking = false;
        }
    }

    public void DoShake(float _shakeIntensity, float _shakeDecay)
    {
        OriginalPos = transform.localPosition;
        OriginalRot = transform.localRotation;

        ShakeIntensity = _shakeIntensity; //0.3f;
        ShakeDecay = _shakeDecay; //0.02f;
        Shaking = true;
    }
}
