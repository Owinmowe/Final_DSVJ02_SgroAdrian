namespace MarsArena
{

    using TMPro;
    using UnityEngine;

    public class UIEndGame : MonoBehaviour
    {

        [Header("Arena Data")]
        [SerializeField] TextMeshProUGUI survivedText = null;
        [SerializeField] TextMeshProUGUI pointsText = null;
        [SerializeField] TextMeshProUGUI pylonsText = null;
        [SerializeField] TextMeshProUGUI distanceText = null;

        ArenaData currentSessionData = new ArenaData();

        void Start()
        {
            currentSessionData = SceneManager.Get().GetLastSessionArenaData();

            survivedText.text = currentSessionData.survived ? "Survived: Yes" : "Survived: No";
            pointsText.text = "Points: " + currentSessionData.points.ToString();
            pylonsText.text = "Pylons Destroyed: " + currentSessionData.pylonsDestroyed.ToString();
            distanceText.text = "Distance Moved: " + currentSessionData.distanceMoved.ToString("F1") + " Meters";
        }

        public void BackToMenu()
        {
            SceneManager.Get().LoadSceneAsync("Main Menu");
        }

        public void PlayAgain()
        {
            SceneManager.Get().LoadSceneAsync("Game Scene");
        }
    }
}
