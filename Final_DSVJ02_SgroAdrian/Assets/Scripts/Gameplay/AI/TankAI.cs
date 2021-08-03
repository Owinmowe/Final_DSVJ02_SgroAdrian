namespace MarsArena
{

    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(TankMovement))]
    public class TankAI : AggresiveEnemyAI
    {
        [Header("Tank AI Specific")]
        [SerializeField][Range(5, 30)] float moveDirectionTolerance = 5f;
        TankMovement tankComponent;

        protected override void Awake()
        {
            base.Awake();
            tankComponent = GetComponent<TankMovement>();
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
                    tankComponent.TryToShoot(playerTrans.position);
                }
                else if (PlayerInMoveDistance())
                {
                    float directionAngle = Vector3.SignedAngle(transform.forward, playerTrans.position - transform.position, transform.up);
                    if (directionAngle > 0) tankComponent.Rotate(1);
                    else tankComponent.Rotate(-1);

                    if (Mathf.Abs(directionAngle) < moveDirectionTolerance)
                    {
                        tankComponent.Move(1);
                    }
                }
                yield return null;
            }
        }
    }
}