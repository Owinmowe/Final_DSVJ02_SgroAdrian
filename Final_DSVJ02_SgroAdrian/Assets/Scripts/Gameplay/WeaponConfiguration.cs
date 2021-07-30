namespace MarsArena
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Weapon Configuration", menuName = "Weapon/Configuration")]
    public class WeaponConfiguration : ScriptableObject
    {
        public GameObject prefab = null;
        public float reloadTime = 1f;
        public float speed = 50f;
        public float damage = 5f;
    }
}
