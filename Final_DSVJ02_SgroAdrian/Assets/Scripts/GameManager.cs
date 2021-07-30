namespace MarsArena
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {

        [SerializeField] Terrain marsTerrain = null;
        [SerializeField] Tank playerTank = null;
        [Header("Pylons")]
        [SerializeField] GameObject pylonPrefab = null;
        [SerializeField] int startingPylonsAmount = 50;
        [SerializeField] float groundOffset = .75f;
        [SerializeField] float maxDistanceSpawnFromCenter = 450;

        public Action<int> OnEnemyDestroyed;
        public Action<float, float> OnPlayerLifeChanged;

        // Start is called before the first frame update
        void Start()
        {
            playerTank.OnDestroy += PlayerDestroyed;
            playerTank.OnLifeChanged += PlayerLifeChanged;


            for (int i = 0; i < startingPylonsAmount; i++)
            {
                CreatePylon();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void EnemyDestroyed(int pointsGot)
        {
            OnEnemyDestroyed?.Invoke(pointsGot);
        }

        private void CreatePylon()
        {
            Vector3 pos = UnityEngine.Random.insideUnitSphere;
            pos *= maxDistanceSpawnFromCenter;
            pos.x += marsTerrain.terrainData.size.x / 2;
            pos.z += marsTerrain.terrainData.size.z / 2;
            pos.y = marsTerrain.SampleHeight(pos) + groundOffset;
            var go = Instantiate(pylonPrefab, pos, Quaternion.identity, transform);
            RaycastHit hit;
            Physics.Raycast(go.transform.position, -go.transform.up, out hit);
            go.transform.up = hit.normal;
            go.GetComponent<Pylon>().OnDestroy += EnemyDestroyed;
        }

        void PlayerDestroyed()
        {

        }
        
        void PlayerLifeChanged(float armor, float shield)
        {
            OnPlayerLifeChanged(armor, shield);
        }

    }
}
