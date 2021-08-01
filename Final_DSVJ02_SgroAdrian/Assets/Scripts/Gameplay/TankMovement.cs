namespace MarsArena
{
    using System;
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(TurretMovement), typeof(DestructableComponent))]
    public class TankMovement : MonoBehaviour
    {

        [Header("Body Movement")]
        [SerializeField] float bodyMovementSpeed = 5f;
        [SerializeField] float bodyRotationSpeed = 1f;
        [SerializeField] float groundCorrectionSpeed = 5f;
        [SerializeField] float groundCheckDistance = 5f;
        [SerializeField] LayerMask groundLayer = default;
        float currentMovementAmount = 0;

        TurretMovement turretComponent = null;
        DestructableComponent destructableComponent = null;

        bool noMovement = false;

        public Action<float> OnMove;

        private void Awake()
        {
            turretComponent = GetComponent<TurretMovement>();
            destructableComponent = GetComponent<DestructableComponent>();
            destructableComponent.OnDestroy += StopAllMovement;
        }

        void StopAllMovement()
        {
            noMovement = true;
        }

        public void Move(float ver)
        {
            if (noMovement) return;
            Vector3 movementVector = bodyMovementSpeed * ver * transform.forward * Time.deltaTime;
            transform.position += movementVector;
            currentMovementAmount += movementVector.magnitude;
            OnMove?.Invoke(currentMovementAmount);
        }

        public void Rotate(float hor)
        {
            if (noMovement) return;
            Quaternion rotation = Quaternion.AngleAxis(hor * bodyRotationSpeed, transform.up);
            transform.rotation *= rotation;
        }

        void Update()
        {
            AlignWithGround();
        }

        private void AlignWithGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, groundCheckDistance, groundLayer))
            {
                Quaternion qTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, qTo, groundCorrectionSpeed * Time.deltaTime);
            }
        }

        public void TryToShoot(Vector3 dir)
        {
            if (noMovement) return;
            turretComponent.AimToShoot(dir);
        }
    }
}