using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float ver)
    {
        transform.position += movementSpeed * ver * transform.forward * Time.deltaTime;
    }

    public void Rotate(float hor)
    {
        Quaternion rotation = Quaternion.AngleAxis(hor * rotationSpeed, transform.up);
        transform.rotation *= rotation;
    }

}
