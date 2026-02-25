using Unity.Cinemachine;
using UnityEngine;

public class RecoilShake : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource screenShake;
    [SerializeField] private float _powerAmount;

public void ScreenShake(Vector3 dir)
    {
        screenShake.GenerateImpulseWithVelocity(dir * _powerAmount);
    }
}
