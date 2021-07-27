using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Tank))]
public class PlayerInput : MonoBehaviour
{
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
    }
}
