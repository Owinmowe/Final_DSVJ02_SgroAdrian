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

            // Start is called before the first frame update
            void Start()
            {
                mainMenuAnimationComponent.TransitionIn();
                creditsAnimationComponent.TransitionOut();
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }
}