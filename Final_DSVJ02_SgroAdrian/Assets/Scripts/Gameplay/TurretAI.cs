namespace MarsArena
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Turret))]
    public class TurretAI : MonoBehaviour
    {
        [Header("AI Behaviour")]
        [SerializeField] float distanceToAttackPlayer = 25f;
        Transform playerTrans = null;
        Turret turretComponent;
        bool wait = false;

        private void Awake()
        {
            turretComponent = GetComponent<Turret>();
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            playerTrans = playerTransform;
        }

        private void Update()
        {
            Vector3 offset = playerTrans.position - transform.position;
            if (offset.sqrMagnitude < distanceToAttackPlayer * distanceToAttackPlayer)
            {
                turretComponent.AimToShoot(playerTrans.position);
            }
        }
    }

}
