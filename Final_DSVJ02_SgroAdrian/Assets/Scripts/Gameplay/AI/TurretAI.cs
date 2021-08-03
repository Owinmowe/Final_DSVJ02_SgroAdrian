namespace MarsArena
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(TurretMovement))]
    public class TurretAI : AggresiveEnemyAI
    {

        TurretMovement turretComponent;

        protected override void Awake()
        {
            base.Awake();
            turretComponent = GetComponent<TurretMovement>();
        }

        private void Start()
        {
            StartCoroutine(AttackPlayer());
        }

        IEnumerator AttackPlayer()
        {
            while (!locked)
            {
                if (PlayerInAttackDistance())
                {
                    turretComponent.AimToShoot(playerTrans.position);
                }
                yield return null;
            }
        }

    }

}
