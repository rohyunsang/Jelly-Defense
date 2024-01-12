using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float speed;
    public CapsuleCollider capsuleCollider;
    public TrailRenderer trailEffect;
}
