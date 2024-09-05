using UnityEngine;

public interface IDamagable 
{
    public void OnDamage(int damageAmount, Transform attackerTransform = null);
}

