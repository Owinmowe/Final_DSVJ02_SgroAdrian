namespace MarsArena
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(TankMovement))]
    public class PlayerInput : MonoBehaviour
    {
        [Header("Cursor Configurations")]
        [SerializeField] LayerMask shootMask = default;
        [SerializeField] float maxCheckDistance = 100f;

        TankMovement tankComponent = null;
        // Start is called before the first frame update
        public Action OnPausedGame;

        private void Awake()
        {
            tankComponent = GetComponent<TankMovement>();
        }

        // Update is called once per frame
        void Update()
        {
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");
            if (Mathf.Abs(ver) > 0)
            {
                tankComponent.Move(ver);
            }
            if (Mathf.Abs(hor) > 0)
            {
                tankComponent.Rotate(hor);
            }
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                float x = Input.GetAxis("Mouse X");
                float y = Input.GetAxis("Mouse Y");
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxCheckDistance, shootMask))
                {
                    tankComponent.TryToShoot(hit.point);
                }
            }
        }

        public void PauseGame()
        {
            OnPausedGame?.Invoke();
        }
    }
}
