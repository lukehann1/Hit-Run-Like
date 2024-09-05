using UnityEngine;

public class Crate : MonoBehaviour, IDamagable
{
    public int health = 3;
    ParticleSystem coinParticle;

    private void OnEnable()
    {
        coinParticle = GetComponentInChildren<ParticleSystem>();
    }

    public void OnDamage(int damageAmount, Transform attackerTransform = null)
    {
        health -= damageAmount;

        coinParticle.Play();

        if(health <= 0)
        {
            coinParticle.gameObject.transform.parent = null;
            gameObject.SetActive(false);
        }
    }
}
