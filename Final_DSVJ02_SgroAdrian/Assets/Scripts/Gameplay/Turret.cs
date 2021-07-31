namespace MarsArena
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Turret : MonoBehaviour
    {
        [SerializeField] WeaponComponent weapon = null;
        [SerializeField] float turretRotationSpeed = 1f;
        [SerializeField] float cooldownBetweenTargets = .25f;
        float currentCooldown = 0;
        IEnumerator rotationCoroutine = null;

        private void Update()
        {
            if(currentCooldown > 0) currentCooldown -= Time.deltaTime;
        }

        public void AimToShoot(Vector3 dir)
        {
            if (currentCooldown > 0) return;
            currentCooldown = cooldownBetweenTargets;
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
    }
}
