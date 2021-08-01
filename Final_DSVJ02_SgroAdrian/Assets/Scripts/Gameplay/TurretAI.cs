namespace MarsArena
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(TurretMovement), typeof(DestructableComponent))]
    public class TurretAI : MonoBehaviour
    {
        [Header("AI Behaviour")]
        [SerializeField] float distanceToAttackPlayer = 25f;

        bool destroyed = false;
        Transform playerTrans = null;
        TurretMovement turretComponent;
        DestructableComponent destructableComponent;

        private void Awake()
        {
            turretComponent = GetComponent<TurretMovement>();
            destructableComponent = GetComponent<DestructableComponent>();
            destructableComponent.OnDestroy += StopAttack;
        }

        private void Start()
        {
            StartCoroutine(AttackPlayer());
        }

        void StopAttack()
        {
            destroyed = true;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            playerTrans = playerTransform;
        }

        IEnumerator AttackPlayer()
        {
            while (!destroyed)
            {
                Vector3 offset = playerTrans.position - transform.position;
                if (offset.sqrMagnitude < distanceToAttackPlayer * distanceToAttackPlayer)
                {
                    turretComponent.AimToShoot(playerTrans.position);
                }
                yield return null;
            }
        }

    }

}
