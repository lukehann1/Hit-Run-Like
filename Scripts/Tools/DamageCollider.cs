using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] int damage;
    Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }


    public void ColliderStatus(bool status)
    {
        col.enabled = status;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer)
            return;

        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.OnDamage(damage, transform);
    }
}
