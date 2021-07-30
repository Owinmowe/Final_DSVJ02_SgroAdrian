namespace MarsArena
{

    namespace UI
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        public class UIMainMenu : MonoBehaviour
        {

            [SerializeField] List<UIAnimationComponent> splashArts = null;
            [Space(10)]
            [SerializeField] UIAnimationComponent mainMenuAnimationComponent = null;
            [SerializeField] UIAnimationComponent creditsAnimationComponent = null;
            [SerializeField] float transitionSpeed = .2f;

            int currentSplashArt = 0;

            // Start is called before the first frame update
            void Start()
            {
                mainMenuAnimationComponent.SetTransitionSpeed(transitionSpeed);
                creditsAnimationComponent.SetTransitionSpeed(transitionSpeed);
                if(splashArts.Count > 0)
                {
                    foreach (var item in splashArts)
                    {
                        item.OnTransitionEnd += NextSplashArt;
                    }
                    splashArts[0].TransitionIn();
                }
                else
                {
                    SceneManager.Get().WhiteScreenUnfade();
                    mainMenuAnimationComponent.TransitionIn();
                }
            }


            void NextSplashArt()
            {
                currentSplashArt++;
                if (currentSplashArt < splashArts.Count)
                {
                    splashArts[currentSplashArt].TransitionIn();
                }
                else
                {
                    SceneManager.Get().WhiteScreenUnfade();
                    mainMenuAnimationComponent.TransitionIn();
                }
            }

            public void OpenCredits()
            {
                mainMenuAnimationComponent.TransitionOut();
                creditsAnimationComponent.TransitionIn();
            }

            public void BackToTheMenu()
            {
                mainMenuAnimationComponent.TransitionIn();
                creditsAnimationComponent.TransitionOut();
            }

            public void PlayGame()
            {
                mainMenuAnimationComponent.TransitionOut();
                SceneManager.Get().LoadSceneAsync("Game Scene");
            }

            public void ExitGame()
            {
                Application.Quit();
            }

        }
    }
}