namespace MarsArena
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class Tank : MonoBehaviour, IDamageable
    {
        [Header("Armor Related")]
        [SerializeField] float maxArmor = 100f;
        float currentArmor = 0;
        bool destroyed = false;

        [Header("Shield Related")]
        [SerializeField] float maxShield = 50f;
        [SerializeField] float shieldRecoverSpeed = .5f;
        float currentShield = 0;
        bool shieldWorking = true;

        [Header("Body Movement")]
        [SerializeField] float bodyMovementSpeed = 5f;
        [SerializeField] float bodyRotationSpeed = 1f;
        [SerializeField] float groundCorrectionSpeed = 5f;
        [SerializeField] float groundCheckDistance = 5f;
        [SerializeField] LayerMask groundLayer = default;
        float currentMovementAmount = 0;

        [Header("Turret Related")]
        [SerializeField] WeaponComponent weapon = null;
        [SerializeField] float turretRotationSpeed = 1f;
        IEnumerator rotationCoroutine = null;

        Animator anim;

        public Action OnDestroy;
        public Action OnBodyDamage;
        public Action<float, float> OnLifeChanged;
        public Action<float> OnMove;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            currentArmor = maxArmor;
            currentShield = maxShield;
            StartCoroutine(ShieldRecovery());
        }

        public void Move(float ver)
        {
            if (destroyed) return;
            Vector3 movementVector = bodyMovementSpeed * ver * transform.forward * Time.deltaTime;
            transform.position += movementVector;
            currentMovementAmount += movementVector.magnitude;
            OnMove?.Invoke(currentMovementAmount);
        }

        public void Rotate(float hor)
        {
            if (destroyed) return;
            Quaternion rotation = Quaternion.AngleAxis(hor * bodyRotationSpeed, transform.up);
            transform.rotation *= rotation;
        }

        void Update()
        {
            AlignWithGround();
        }

        private void AlignWithGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance, groundLayer))
            {
                Quaternion qTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, qTo, groundCorrectionSpeed * Time.deltaTime);
            }
        }

        public void TryToShoot(Vector3 dir)
        {
            if (rotationCoroutine != null)
            {
                StopCoroutine(rotationCoroutine);
            }
            rotationCoroutine = AimTurret(dir);
            StartCoroutine(rotationCoroutine);
        }

        IEnumerator AimTurret(Vector3 dir)
        {
            Quaternion startingRotation = weapon.transform.rotation;
            Quaternion finalRotation = Quaternion.identity;
            Vector3 aimDirection = dir - weapon.transform.position;
            finalRotation.SetLookRotation(aimDirection, transform.up);
            float t = 0;
            while (t < 1)
            {
                weapon.transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
                t += Time.deltaTime * turretRotationSpeed;
                yield return null;
            }
            weapon.Shoot(dir);
        }

        IEnumerator ShieldRecovery()
        {
            while (shieldWorking)
            {
                if(currentShield < maxShield)
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
                    anim.SetTrigger("Body Destroyed");
                    OnDestroy?.Invoke();
                }
                else
                {
                    OnBodyDamage?.Invoke();
                }
            }

        }

    }
}