using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Image cutScene1;
    public Image cutScene2;
    public string loadScene;
    [SerializeField] GameObject cutScene1TXT;
    [SerializeField] GameObject cutScene2TXT;
    [SerializeField] GameObject cutScene3TXT;
    [SerializeField] GameObject cutScene4TXT;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        cutScene1.canvasRenderer.SetAlpha(0.0f);
        cutScene2.canvasRenderer.SetAlpha(0.0f);

        FadeIn(cutScene1);
        cutScene1TXT.GetComponent<TypeEffect>().EffectStart();
        yield return new WaitForSeconds(7.5f);
        cutScene1TXT.SetActive(false);
        yield return new WaitForSeconds(3.2f);
        FadeOut(cutScene1);
        FadeIn(cutScene2);
        cutScene2TXT.GetComponent<TypeEffect>().EffectStart();
        yield return new WaitForSeconds(6f);
        cutScene2TXT.SetActive(false);
        cutScene3TXT.GetComponent<TypeEffect>().EffectStart();
        yield return new WaitForSeconds(6.2f);
        cutScene3TXT.SetActive(false);
        cutScene4TXT.GetComponent<TypeEffect>().EffectStart();
        yield return new WaitForSeconds(4.5f);
        cutScene4TXT.SetActive(false);
        FadeOut(cutScene2);
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(loadScene);
    }

    void FadeIn(Image cutScene)
    {
        cutScene.CrossFadeAlpha(1.0f, 1.2f, false);
    }

    void FadeOut(Image cutScene)
    {
        cutScene.CrossFadeAlpha(0.0f, 1.2f, false);

    }
}
