#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using UnityEngine;

public class ProtoBrainEffect : MonoBehaviour
{
    public Player player;
    public void Start()
    {
        player = gameObject.GetComponentInParent<Player>();
    }
}