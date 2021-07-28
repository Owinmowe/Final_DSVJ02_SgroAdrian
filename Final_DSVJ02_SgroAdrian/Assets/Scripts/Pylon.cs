namespace MarsArena
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Pylon : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxArmor;
        float currentArmor;

        Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            currentArmor = maxArmor;
        }

        public void TakeDamage(float damage)
        {
            currentArmor -= damage;
            if(currentArmor < 0)
            {
                anim.SetTrigger("Destroy");
            }
            else
            {
                anim.SetTrigger("Hit");
            }
        }

        void DestroyAfterAnimation()
        {
            Destroy(gameObject);
        }
    }
}
