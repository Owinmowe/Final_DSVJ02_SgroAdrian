using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class PlayerInput : MonoBehaviour
{
    [Header("Cursor Configurations")]
    [SerializeField] LayerMask shootMask = default;
    [SerializeField] float maxCheckDistance = 100f;

    Tank tankComponent = null;
    // Start is called before the first frame update

    private void Awake()
    {
        tankComponent = GetComponent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        if(Mathf.Abs(ver) > 0)
        {
            tankComponent.Move(ver);
        }
        if(Mathf.Abs(hor) > 0)
        {
            tankComponent.Rotate(hor);
        }
        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, maxCheckDistance, shootMask))
            {
                tankComponent.TryToShoot(hit.point);
            }
        }
    }
}
