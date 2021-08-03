namespace MarsArena
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] List<WeaponConfiguration> currentWeaponsConfigs = null;
        [SerializeField] Transform projectileSpawnPosition = null;
        [SerializeField] Color projectileColor = Color.white;
        float timeToShoot = 0;
        bool canShoot = true;
        int selectedWeapon = 0;

        public void Shoot(Vector3 dir)
        {
            if(canShoot)
            {
                timeToShoot = currentWeaponsConfigs[selectedWeapon].reloadTime;
                StartCoroutine(Reload());
                GameObject go = Instantiate(currentWeaponsConfigs[selectedWeapon].prefab, projectileSpawnPosition.transform.position, Quaternion.identity);
                Projectile projComponent = go.GetComponent<Projectile>();
                Vector3 launchDir = dir - projectileSpawnPosition.position;
                projComponent.SetProjectile(currentWeaponsConfigs[selectedWeapon].damage, gameObject.layer, projectileColor);
                projComponent.Launch(launchDir.normalized, currentWeaponsConfigs[selectedWeapon].speed);
            }
        }

        public void NextWeapon()
        {
            selectedWeapon++;
            if (selectedWeapon == currentWeaponsConfigs.Count) selectedWeapon = 0;
        }

        IEnumerator Reload()
        {
            canShoot = false;
            while (timeToShoot > 0)
            {
                timeToShoot -= Time.deltaTime;
                yield return null;
            }
            canShoot = true;
        }

    }
}
