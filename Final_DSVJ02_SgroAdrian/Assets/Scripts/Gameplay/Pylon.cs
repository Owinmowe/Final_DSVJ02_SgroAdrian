namespace MarsArena
{
    using System;
    using UnityEngine;

    public class Pylon : MonoBehaviour, IDamageable
    {
        [SerializeField] int pointsOnDestroy = 100;
        [SerializeField] float maxArmor = 100;
        float currentArmor;

        Animator anim;

        public Action<int> OnDestroy;

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
                OnDestroy?.Invoke(pointsOnDestroy);
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
