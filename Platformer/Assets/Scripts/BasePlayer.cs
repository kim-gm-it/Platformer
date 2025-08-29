using Unity.Netcode;
using UnityEngine;

public class BasePlayer : NetworkBehaviour
{
    public float livesBar;
    public int livesPoint;
     public virtual void RefreshUI(){}
}
