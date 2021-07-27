namespace MarsArena
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WeaponComponent : MonoBehaviour
    {
        public void Shoot(Vector3 dir)
        {
            Debug.Log("Shoot to direction: " + dir);
        }
    }
}
