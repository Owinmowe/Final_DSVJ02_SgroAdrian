namespace MarsArena
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {

        [Header("General Gameplay")]
        [SerializeField] Terrain marsTerrain = null;
        [SerializeField] TankMovement playerTank = null;
        [SerializeField] float maxTimePerSession = 200f;

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
        public Action OnTimeUp;
        public Action<bool> OnPlayerPressedPause;
        public Action<float> OnTimeChanged;
        public Action<float> OnPlayerMoved;
        public Action<int> OnEnemyDestroyed;
        public Action<float, float> OnPlayerLifeChanged;

        bool gamePaused = false;
        int playerTotalPoints = 0;
        int playerPylonsDestroyed = 0;
        float distanceMoved = 0;
        float currentTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 0;
            playerTank.OnMove += PlayerMoved;
            playerTank.GetComponent<PlayerInput>().OnPausedGame = TogglePause;
            var playerDestructableComponent = playerTank.GetComponent<DestructableComponent>();
            playerDestructableComponent.OnDestroy += PlayerDestroyed;
            playerDestructableComponent.OnLifeChanged += PlayerLifeChanged;


            for (int i = 0; i < startingPylonsAmount; i++)
            {
                CreatePylon();
            }

            for (int i = 0; i < startingTanksAmount; i++)
            {
                CreateEnemyWithAi(enemyTanksPrefab, tanksParent, tanksMaxDistanceSpawn, tanksGroundOffset);
            }

            for (int i = 0; i < startingTurretsAmount; i++)
            {
                CreateEnemyWithAi(enemyTurretsPrefab, turretsParent, turretsMaxDistanceSpawn, turretsGroundOffset);
            }
        }

        private void Update()
        {
            if (!gamePaused)
            {
                currentTime += Time.deltaTime;
                OnTimeChanged?.Invoke(currentTime);
                if(currentTime > maxTimePerSession)
                {
                    TimeIsUp();
                }
            }
        }

        private void CreatePylon()
        {
            GameObject go = CreateEntity(pylonPrefab, pylonParent, pylonMaxDistanceSpawn, pylonGroundOffset);
            go.GetComponent<PylonAI>().OnDestroy += EnemyDestroyed;
            go.GetComponent<PylonAI>().OnDestroy += PylonDestroyed;
        }

        private void CreateEnemyWithAi(GameObject prefab, Transform parent, float spawnDistance, float groundOffset)
        {
            GameObject go = CreateEntity(prefab, parent, spawnDistance, groundOffset);
            var EnemyAi = go.GetComponent<AggresiveEnemyAI>();
            EnemyAi.SetPlayerTransform(playerTank.transform);
            EnemyAi.OnDestroy += EnemyDestroyed;
            OnPlayerDestroyed += EnemyAi.StopAttack;
            OnTimeUp += EnemyAi.StopAttack;
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

        void PylonDestroyed(int points)
        {
            playerPylonsDestroyed++;
        }

        void EnemyDestroyed(int pointsGot)
        {
            playerTotalPoints += pointsGot;
            OnEnemyDestroyed?.Invoke(playerTotalPoints);
        }

        void PlayerDestroyed()
        {
            SceneManager.Get().SetLastSessionArenaData(false, playerTotalPoints, playerPylonsDestroyed, distanceMoved);
            OnPlayerDestroyed?.Invoke();
        }
        
        void TimeIsUp()
        {
            SceneManager.Get().SetLastSessionArenaData(true, playerTotalPoints, playerPylonsDestroyed, distanceMoved);
            gamePaused = true;
            StopGame();
            OnTimeUp?.Invoke();
        }

        void PlayerLifeChanged(float armor, float shield)
        {
            OnPlayerLifeChanged?.Invoke(armor, shield);
        }

        void PlayerMoved(float moveAmount)
        {
            distanceMoved = moveAmount;
            OnPlayerMoved?.Invoke(moveAmount);
        }

        void TogglePause()
        {
            gamePaused = !gamePaused;
            OnPlayerPressedPause?.Invoke(gamePaused);
            if (gamePaused) Time.timeScale = 0;
            else Time.timeScale = 1;
        }

        public void GoBackToMenu()
        {
            SceneManager.Get().LoadSceneAsync("Main Menu");
        }

        public void GoToEndGameScene()
        {
            SceneManager.Get().LoadSceneAsync("End Game");
        }

        public void StartGame()
        {
            Time.timeScale = 1;
        }

        void StopGame()
        {
            Time.timeScale = 0;
        }

    }
}
