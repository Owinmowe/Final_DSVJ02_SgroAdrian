namespace MarsArena
{
    namespace UI
    {

        using System.Collections;
        using UnityEngine;
        using UnityEngine.UI;
        using TMPro;

        public class UILoadingScreen : MonoBehaviour
        {
            [SerializeField] Image whiteScreen = null;
            [SerializeField] TextMeshProUGUI textComponent = null;

            bool canFadeOut = true;

            public void FadeWithWhiteScreen()
            {
                StopAllCoroutines();
                textComponent.text = "";
                StartCoroutine(whiteScreenFade());
            }

            public void FadeWithWhiteScreen(string text)
            {
                StopAllCoroutines();
                textComponent.text = text;
                StartCoroutine(whiteScreenFade());
            }

            IEnumerator whiteScreenFade()
            {
                while (whiteScreen.color.a + Time.unscaledDeltaTime < 1)
                {
                    whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, whiteScreen.color.a + Time.unscaledDeltaTime);
                    textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + Time.unscaledDeltaTime);
                    yield return null;
                }
                whiteScreen.color = Color.white;
                textComponent.color = Color.white;
                while (!canFadeOut)
                {
                    yield return null;
                }
                while (whiteScreen.color.a - Time.unscaledDeltaTime > 0)
                {
                    whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, whiteScreen.color.a - Time.unscaledDeltaTime);
                    textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, whiteScreen.color.a - Time.unscaledDeltaTime);
                    yield return null;
                }
                whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0);
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
            }

            public void LockFade()
            {
                canFadeOut = false;
            }

            public void UnlockFade()
            {
                canFadeOut = true;
            }

            public void WhiteScreenUnfade()
            {
                StartCoroutine(WhiteScreenUnfadeCoroutine());
            }

            IEnumerator WhiteScreenUnfadeCoroutine()
            {
                while (whiteScreen.color.a - Time.unscaledDeltaTime > 0)
                {
                    whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, whiteScreen.color.a - Time.unscaledDeltaTime);
                    textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, whiteScreen.color.a - Time.unscaledDeltaTime);
                    yield return null;
                }
                whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0);
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
            }
        }
    }
}
