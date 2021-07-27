namespace MarsArena
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tank : MonoBehaviour, IDamageable
    {
        [Header("Game Stats")]
        [SerializeField] float maxArmor = 100f;
        [SerializeField] float maxShield = 50f;
        float currentArmor = 0;
        float currentShield = 0;

        [Header("Body Movement")]
        [SerializeField] float bodyMovementSpeed = 5f;
        [SerializeField] float bodyRotationSpeed = 1f;

        [Header("Turret Movement")]
        [SerializeField] WeaponComponent weapon = null;
        [SerializeField] float turretRotationSpeed = 1f;
        IEnumerator rotationCoroutine = null;

        private void Start()
        {
            currentArmor = maxArmor;
            currentShield = maxShield;
        }

        public void Move(float ver)
        {
            transform.position += bodyMovementSpeed * ver * transform.forward * Time.deltaTime;
        }

        public void Rotate(float hor)
        {
            Quaternion rotation = Quaternion.AngleAxis(hor * bodyRotationSpeed, transform.up);
            transform.rotation *= rotation;
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

        public void TakeDamage(float damage)
        {
            if (currentShield > 0)
            {
                currentShield -= damage;
                if (currentShield < 0)
                {
                    currentArmor -= currentShield;
                }
            }
            else
            {
                currentArmor -= damage;
            }
        }

    }
}