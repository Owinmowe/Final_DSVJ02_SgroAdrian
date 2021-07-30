namespace MarsArena
{

    namespace UI
    {
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;

        public class UIMainMenu : MonoBehaviour
        {

            [SerializeField] UIAnimationComponent mainMenuAnimationComponent = null;
            [SerializeField] UIAnimationComponent creditsAnimationComponent = null;
            [SerializeField] float transitionSpeed = .2f;

            // Start is called before the first frame update
            void Start()
            {
                mainMenuAnimationComponent.SetTransitionSpeed(transitionSpeed);
                creditsAnimationComponent.SetTransitionSpeed(transitionSpeed);
                mainMenuAnimationComponent.TransitionIn();
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
            }

            public void ExitGame()
            {
                Application.Quit();
            }

        }
    }
}