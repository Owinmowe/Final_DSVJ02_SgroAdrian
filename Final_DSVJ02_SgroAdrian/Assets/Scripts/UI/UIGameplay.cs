namespace MarsArena
{

    namespace UI
    {

        using System.Collections;
        using TMPro;
        using UnityEngine;

        public class UIGameplay : MonoBehaviour
        {
            [SerializeField] GameManager gm = null;
            [SerializeField] float pauseTillSceneChange = 5f;
            [Header("Playing HUD")]
            [SerializeField] UIAnimationComponent playingHUDGroup = null;
            [SerializeField] TextMeshProUGUI pointsTextComponent = null;
            [SerializeField] TextMeshProUGUI movementTextComponent = null;
            [SerializeField] TextMeshProUGUI timeTextComponent = null;

            [Header("Pause HUD")]
            [SerializeField] UIAnimationComponent pauseHUDGroup = null;

            [Header("Starting HUD")]
            [SerializeField] UIAnimationComponent countDownText = null;
            [SerializeField] int startingCount = 3;
            TextMeshProUGUI countDownTextComponent; 

            private void Awake()
            {
                gm.OnTimeChanged += UpdateTimeText;
                gm.OnPlayerDestroyed += PlayerDestroyed;
                gm.OnTimeUp += PlayerTimeUp;
                gm.OnPlayerMoved += UpdatePlayerMovementText;
                gm.OnPlayerLifeChanged += UpdatePlayerLifeHUD;
                gm.OnEnemyDestroyed += UpdatePointsText;
                gm.OnPlayerPressedPause += PauseToggle;

                countDownTextComponent = countDownText.GetComponent<TextMeshProUGUI>();
                countDownText.OnTransitionEnd += NextStartingNumber;
            }

            private void Start()
            {
                
            }

            void NextStartingNumber()
            {
                startingCount--;
                if(startingCount > 0)
                {
                    countDownTextComponent.text = startingCount.ToString();
                }
                else
                {
                    countDownTextComponent.text = "GO!";
                    playingHUDGroup.TransitionIn();
                    gm.StartGame();
                }
            }

            void PlayerTimeUp()
            {
                StartCoroutine(SessionEnded(true));
            }

            void PlayerDestroyed()
            {
                StartCoroutine(SessionEnded(false));
            }

            IEnumerator SessionEnded(bool survived)
            {
                yield return new WaitForSecondsRealtime(pauseTillSceneChange);
                gm.GoToEndGameScene();
            }

            void UpdatePlayerMovementText(float amount)
            {
                movementTextComponent.text = "Movement: " + amount.ToString("F1");
            }

            void UpdatePlayerLifeHUD(float armor, float shield)
            {

            }

            void UpdatePointsText(int points)
            {
                pointsTextComponent.text = "Points: " + points;
            }

            void UpdateTimeText(float time)
            {
                timeTextComponent.text = "Time: " + time.ToString("F1");
            }

            void PauseToggle(bool paused)
            {
                if (paused)
                {
                    playingHUDGroup.TransitionOut();
                    pauseHUDGroup.TransitionIn();
                }
                else
                {
                    playingHUDGroup.TransitionIn();
                    pauseHUDGroup.TransitionOut();
                }
            }
        }
    }
}
