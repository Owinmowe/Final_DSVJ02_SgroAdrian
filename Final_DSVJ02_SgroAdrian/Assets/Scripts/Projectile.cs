namespace MarsArena
{
    using UnityEngine;
    using System.Collections.Generic;

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] List<Renderer> rendersToColorize;
        float contactDamage = 1;
        Rigidbody rb;

        public void SetProjectile(float damage, int layer, Color projColor)
        {
            gameObject.layer = layer;
            contactDamage = damage;
            rb = GetComponent<Rigidbody>();
            foreach(Renderer r in rendersToColorize)
            {
                r.material.SetColor("_EmissionColor", projColor);
            }
        }

        public void Launch(Vector3 normalizedDir, float speed)
        {

            rb.AddForce(normalizedDir * speed);
        }

        private void OnCollisionEnter(Collision collision)
        {
            IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
            if(damageComponent != null)
            {
                damageComponent.TakeDamage(contactDamage);
            }
            Destroy(gameObject);
        }

    }
}
