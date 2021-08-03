namespace MarsArena
{

    using System;
    using UnityEngine;

    [RequireComponent(typeof(DestructableComponent))]
    public class AggresiveEnemyAI : MonoBehaviour
    {
        [Header("General Gameplay")]
        [SerializeField] int pointsOnDestroy = 100;
        [Header("AI Behaviour")]
        [SerializeField] float distanceToAttackPlayer = 25f;
        [SerializeField] float distanceToMoveToPlayer = 50f;

        protected Transform playerTrans = null;
        protected DestructableComponent destructableComponent;

        public Action<int> OnDestroy;

        protected bool locked = false;

        protected virtual void Awake()
        {
            SetAIBasicComponents();
        }

        void SetAIBasicComponents()
        {
            destructableComponent = GetComponent<DestructableComponent>();
            destructableComponent.OnDestroy += Destroy;
        }

        protected bool PlayerInMoveDistance()
        {
            Vector3 offset = playerTrans.position - transform.position;
            return offset.sqrMagnitude < distanceToMoveToPlayer * distanceToMoveToPlayer;
        }

        protected bool PlayerInAttackDistance()
        {
            Vector3 offset = playerTrans.position - transform.position;
            return offset.sqrMagnitude < distanceToAttackPlayer * distanceToAttackPlayer;
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            playerTrans = playerTransform;
        }

        public void StopAttack()
        {
            locked = true;
        }

        private void Destroy()
        {
            StopAttack();
            OnDestroy?.Invoke(pointsOnDestroy);
        }

    }
}
