using Unity.Cinemachine;
using UnityEngine;

public class RecoilShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] private float _powerAmount;
    [SerializeField] private float _maxPower;

    public void ScreenShake(Vector3 dir)
    {
        screenShake.GenerateImpulseWithVelocity(dir);
    }
}
