using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator anim;
    public float transitionTime = .25f;

    public static LevelLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName)
    {
        //If we did not start a cutscene
        StartCoroutine(LoadWithFade(sceneName));
    }

    public void LoadScene(int sceneNo)
    {
        StartCoroutine(LoadWithFade(sceneNo));
    }


    IEnumerator LoadWithFade(int levelIndex)
    {
        //Play animation
        if (anim) {
            anim.SetTrigger("Start");
        }

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadWithFade(string levelName)
    {
        //Play animation
        if (anim) {
            anim.SetTrigger("Start");
        }        

        //Wait
        yield return new WaitForSeconds(transitionTime);

        //Load scene
        SceneManager.LoadScene(levelName);
    }

    //Only use these other two if we end up implementing a loadingbar
    public void LoadingBarVer(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            anim.SetBool("Loading", true);
            float progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(operation.progress);
            yield return null;
        }
    }
}
