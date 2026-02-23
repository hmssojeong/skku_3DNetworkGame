using UnityEngine;
using Photon.Pun;

public abstract class PlayerAbility : MonoBehaviourPun
{
    protected PlayerController _owner {  get; private set; }

    protected virtual void Awake()
    {
        _owner = GetComponent<PlayerController>();
    }
}
