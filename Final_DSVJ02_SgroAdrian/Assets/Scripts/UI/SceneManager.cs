namespace MarsArena
{

    using System.Collections;
    using UnityEngine;

    public class SceneManager : MonoBehaviourSingleton<SceneManager>
    {

        [SerializeField] float minTimeToLoadScene = 1f;
        [SerializeField] UI.UILoadingScreen uI_LoadingScreen = null;

        ArenaData lastSessionData = new ArenaData();

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(AsynchronousLoadWithFake(sceneName));
        }

        IEnumerator AsynchronousLoadWithFake(string scene)
        {
            float loadingProgress = 0;
            float timeLoading = 0;

            yield return null;

            AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
            uI_LoadingScreen.FadeWithWhiteScreen();
            uI_LoadingScreen.LockFade();
            ao.allowSceneActivation = false;

            while (!ao.isDone)
            {
                timeLoading += Time.unscaledDeltaTime;
                loadingProgress = ao.progress + 0.1f;
                loadingProgress = loadingProgress * timeLoading / minTimeToLoadScene;

                // Se completo la carga
                if (loadingProgress >= 1)
                {
                    ao.allowSceneActivation = true;
                    uI_LoadingScreen.UnlockFade();
                }
                yield return null;
            }

        }

        public void SetLastSessionArenaData(bool survived, int points, int pylons, float distance)
        {
            lastSessionData.survived = survived;
            lastSessionData.points = points;
            lastSessionData.pylonsDestroyed = pylons;
            lastSessionData.distanceMoved = distance;
        }

        public ArenaData GetLastSessionArenaData()
        {
            return lastSessionData;
        }

        public void WhiteScreenUnfade()
        {
            uI_LoadingScreen.WhiteScreenUnfade();
        }

        public void FakeLoad(float time)
        {
            StartCoroutine(FakeLoadingWithWhiteScreen(time));
        }

        public void FakeLoad(float time, string text)
        {
            StartCoroutine(FakeLoadingWithWhiteScreen(time, text));
        }

        IEnumerator FakeLoadingWithWhiteScreen(float time)
        {
            uI_LoadingScreen.FadeWithWhiteScreen();
            uI_LoadingScreen.LockFade();
            yield return new WaitForSecondsRealtime(time);
            uI_LoadingScreen.UnlockFade();
        }

        IEnumerator FakeLoadingWithWhiteScreen(float time, string text)
        {
            uI_LoadingScreen.FadeWithWhiteScreen(text);
            uI_LoadingScreen.LockFade();
            yield return new WaitForSecondsRealtime(time);
            uI_LoadingScreen.UnlockFade();
        }
    }
}
