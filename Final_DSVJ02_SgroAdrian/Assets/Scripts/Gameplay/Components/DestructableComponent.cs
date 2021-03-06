namespace MarsArena
{

    using System;
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class DestructableComponent : MonoBehaviour, IDamageable
    {

        [SerializeField] float bodyDecayTime = 5f;

        [Header("Armor Related")]
        [SerializeField] float maxArmor = 100f;
        float currentArmor = 0;

        [Header("Shield Related")]
        [SerializeField] float maxShield = 50f;
        [SerializeField] float shieldRecoverSpeed = .5f;
        float currentShield = 0;
        bool shieldWorking = true;

        Animator anim;

        bool alive = true;

        public Action OnDestroy;
        public Action OnBodyDamage;
        public Action<float, float> OnLifeChanged;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            currentArmor = maxArmor;
            if(maxShield > 0)
            {
                currentShield = maxShield;
                StartCoroutine(ShieldRecovery());
            }
        }

        IEnumerator ShieldRecovery()
        {
            while (shieldWorking)
            {
                if (currentShield < maxShield)
                {
                    currentShield += shieldRecoverSpeed * Time.deltaTime;
                    currentShield = Mathf.Clamp(currentShield, 0, maxShield);
                    OnLifeChanged?.Invoke(currentArmor, currentShield);
                }
                yield return null;
            }
        }

        public void TakeDamage(float damage)
        {
            if (!alive) return;
            if (currentShield > 0)
            {
                currentShield -= damage;
                if (currentShield < 0)
                {
                    currentArmor -= currentShield;
                    shieldWorking = false;
                    anim.SetTrigger("Shield Destroyed");
                }
                else
                {
                    anim.SetTrigger("Shield Hit");
                }
            }
            else
            {
                currentArmor -= damage;
                if (currentArmor < 0)
                {
                    alive = false;
                    anim.SetTrigger("Body Destroyed");
                    OnDestroy?.Invoke();
                    Destroy(gameObject, bodyDecayTime);
                }
                else
                {
                    anim.SetTrigger("Body Hit");
                    OnBodyDamage?.Invoke();
                }
            }
            OnLifeChanged?.Invoke(currentArmor, currentShield);
        }

        public float GetMaxArmor() => maxArmor;
        public float GetMaxShield() => maxShield;

    }
}