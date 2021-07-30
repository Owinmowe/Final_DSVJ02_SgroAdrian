namespace MarsArena
{

    namespace UI
    {
        using System;
        using UnityEngine;

        [RequireComponent(typeof(Animator))]
        public class UIAnimationComponent : MonoBehaviour
        {
            public Action OnTransitionEnd;

            Animator anim;

            private void Awake()
            {
                anim = GetComponent<Animator>();
                anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            }

            void TransitionEnd()
            {
                OnTransitionEnd?.Invoke();
            }

            public void TransitionIn()
            {
                anim.SetTrigger("Transition In");
            }

            public void TransitionOut()
            {
                anim.SetTrigger("Transition Out");
            }

            public void SetTransitionSpeed(float speed)
            {
                anim.SetFloat("Transition Speed", speed);
            }

        }
    }
}