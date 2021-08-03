namespace MarsArena
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(DestructableComponent))]
    public class PylonAI : MonoBehaviour
    {

        [SerializeField] int pointsOnDestroy = 100;

        public Action<int> OnDestroy;

        private void Awake()
        {
            GetComponent<DestructableComponent>().OnDestroy += OnPylonDestroyed;
        }

        void OnPylonDestroyed()
        {
            OnDestroy?.Invoke(pointsOnDestroy);
        }

    }
}
