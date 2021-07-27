using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("Body Movement")]
    [SerializeField] float bodyMovementSpeed = 5f;
    [SerializeField] float bodyRotationSpeed = 1f;

    [Header("Turret Movement")]
    [SerializeField] WeaponComponent weapon = null;
    [SerializeField] float turretRotationSpeed = 1f;
    IEnumerator rotationCoroutine = null;

    public void Move(float ver)
    {
        transform.position += bodyMovementSpeed * ver * transform.forward * Time.deltaTime;
    }

    public void Rotate(float hor)
    {
        Quaternion rotation = Quaternion.AngleAxis(hor * bodyRotationSpeed, transform.up);
        transform.rotation *= rotation;
    }

    public void TryToShoot(Vector3 dir)
    {
        if(rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        rotationCoroutine = AimTurret(dir);
        StartCoroutine(rotationCoroutine);
    }

    IEnumerator AimTurret(Vector3 dir)
    {
        Quaternion startingRotation = weapon.transform.rotation;
        Quaternion finalRotation = Quaternion.identity;
        Vector3 dir2 = dir - weapon.transform.position;
        finalRotation.SetLookRotation(dir2, transform.up);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * turretRotationSpeed;
            weapon.transform.rotation = Quaternion.Slerp(startingRotation, finalRotation, t);
            yield return null;
        }
        weapon.Shoot(dir);
    }

}
