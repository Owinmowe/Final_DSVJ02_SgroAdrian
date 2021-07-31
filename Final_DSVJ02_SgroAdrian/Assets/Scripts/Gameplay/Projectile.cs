namespace MarsArena
{
    using UnityEngine;
    using System.Collections.Generic;

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] List<Renderer> rendersToColorize = null;
        [Header("On Impact")]
        [SerializeField] GameObject particlesGO = null;
        [SerializeField] float timeToDestroy = 5f;
        float contactDamage = 1;
        Rigidbody rb = null;
        Collider col = null;

        public void SetProjectile(float damage, int layer, Color projColor)
        {
            gameObject.layer = layer;
            contactDamage = damage;
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            foreach (Renderer r in rendersToColorize)
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
            rb.velocity = Vector3.zero;
            col.enabled = false;
            IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
            if(damageComponent != null)
            {
                damageComponent.TakeDamage(contactDamage);
            }
            particlesGO.SetActive(true);
            particlesGO.transform.up = collision.GetContact(0).normal;
            Destroy(gameObject, timeToDestroy);
        }

    }
}
