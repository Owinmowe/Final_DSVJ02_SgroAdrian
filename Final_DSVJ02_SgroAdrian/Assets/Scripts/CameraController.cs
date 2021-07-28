namespace MarsArena
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        [Header("Player camera adjustment")]
        [SerializeField] GameObject player = null;
        [SerializeField] Vector3 offsetFromPlayer = Vector3.zero;
        // Update is called once per frame
        void LateUpdate()
        {
            if (player != null)
            {
                transform.position = player.transform.position + offsetFromPlayer;
            }
        }
    }
}
