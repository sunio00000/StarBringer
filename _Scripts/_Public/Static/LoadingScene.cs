using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;
    public Image progressBar;
    public Text loadText;
    private readonly string[] text =
    {
        "게임 만드는게 생각보다 더 힘든걸",
        "아무거나 누르다가 버그라도 나면...",
        "뭘 만들어야 재밌을까",
        "천장을 봐, 거북목 되겠어.",
        "어떤 일도 쉽게 이뤄지는 것은 없어",
        "가끔 나도 내가 무슨 말을 하는지 몰라.",
        "오각형의 한 내각은 108도야 ",
        "뭐 하면서 먹고 살아야 하나~",
    };
    private string nextSceneName;

    void Start()
    {
        progressBar.fillAmount = 0.0f;
        loadText.text = text[Random.Range(0, text.Length)];
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Load");
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.3f);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if(op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                    op.allowSceneActivation = true;
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if(progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
