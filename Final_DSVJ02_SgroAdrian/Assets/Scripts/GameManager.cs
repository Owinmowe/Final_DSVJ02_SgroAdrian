namespace MarsArena
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {

        [SerializeField] Terrain marsTerrain = null;
        [SerializeField] TankMovement playerTank = null;

        [Header("Pylons")]
        [SerializeField] GameObject pylonPrefab = null;
        [SerializeField] Transform pylonParent = null;
        [SerializeField] int startingPylonsAmount = 50;
        [SerializeField] float pylonGroundOffset = .75f;
        [SerializeField] float pylonMaxDistanceSpawn = 450;

        [Header("Enemy Tanks")]
        [SerializeField] GameObject enemyTanksPrefab = null;
        [SerializeField] Transform tanksParent = null;
        [SerializeField] int startingTanksAmount = 10;
        [SerializeField] float tanksGroundOffset = .75f;
        [SerializeField] float tanksMaxDistanceSpawn = 450;

        [Header("Enemy Turrets")]
        [SerializeField] GameObject enemyTurretsPrefab = null;
        [SerializeField] Transform turretsParent = null;
        [SerializeField] int startingTurretsAmount = 20;
        [SerializeField] float turretsGroundOffset = 2f;
        [SerializeField] float turretsMaxDistanceSpawn = 450;

        public Action OnPlayerDestroyed;
        public Action<float> OnPlayerMoved;
        public Action<int> OnEnemyDestroyed;
        public Action<float, float> OnPlayerLifeChanged;

        // Start is called before the first frame update
        void Start()
        {
            playerTank.OnMove += PlayerMoved;
            var playerDestructableComponent = playerTank.GetComponent<DestructableComponent>();
            playerDestructableComponent.OnDestroy += PlayerDestroyed;
            playerDestructableComponent.OnLifeChanged += PlayerLifeChanged;


            for (int i = 0; i < startingPylonsAmount; i++)
            {
                CreatePylon();
            }

            for (int i = 0; i < startingTanksAmount; i++)
            {
                CreateEnemyTank();
            }

            for (int i = 0; i < startingTurretsAmount; i++)
            {
                CreateEnemyTurret();
            }
        }


        private void CreatePylon()
        {
            GameObject go = CreateEntity(pylonPrefab, pylonParent, pylonMaxDistanceSpawn, pylonGroundOffset);
            go.GetComponent<PylonAI>().PylonDestroyed += EnemyDestroyed;
        }

        private void CreateEnemyTank()
        {
            GameObject go = CreateEntity(enemyTanksPrefab, tanksParent, tanksMaxDistanceSpawn, tanksGroundOffset);
            //go.GetComponent<Tank>().OnDestroy += EnemyDestroyed;
        }

        private void CreateEnemyTurret()
        {
            GameObject go = CreateEntity(enemyTurretsPrefab, turretsParent, turretsMaxDistanceSpawn, turretsGroundOffset);
            go.GetComponent<TurretAI>().SetPlayerTransform(playerTank.transform);
            //go.GetComponent<Tank>().OnDestroy += EnemyDestroyed;
        }

        GameObject CreateEntity(GameObject prefab, Transform parent, float maxSpawnDis, float groundOffset = 0)
        {
            Vector3 pos = UnityEngine.Random.insideUnitSphere;
            pos *= maxSpawnDis;
            pos.x += marsTerrain.terrainData.size.x / 2;
            pos.z += marsTerrain.terrainData.size.z / 2;
            pos.y = marsTerrain.SampleHeight(pos) + groundOffset;
            var go = Instantiate(prefab, pos, Quaternion.identity, parent);
            RaycastHit hit;
            Physics.Raycast(go.transform.position, -go.transform.up, out hit);
            go.transform.up = hit.normal;
            return go;
        }

        void EnemyDestroyed(int pointsGot)
        {
            OnEnemyDestroyed?.Invoke(pointsGot);
        }

        void PlayerDestroyed()
        {
            OnPlayerDestroyed?.Invoke();
        }
        
        void PlayerLifeChanged(float armor, float shield)
        {
            OnPlayerLifeChanged?.Invoke(armor, shield);
        }

        void PlayerMoved(float moveAmount)
        {
            OnPlayerMoved?.Invoke(moveAmount);
        }

    }
}
