using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenSceneCtrl : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject skipButton;
    public Text text;
    public Image panel;
    public bool OpenGame = false;

    void Update()
    {
        if (OpenGame && Input.GetMouseButtonDown(0)) LoadingScene.LoadScene("Battle");
    }

    private IEnumerator Press()
    {
        float howFast = 3.0f;
        while (true)
        {
            if(panel.color.a < 250.0f/255.0f) panel.color += new Color(0, 0, 0, Time.deltaTime * 0.15f);
            text.color -= new Color(0, 0, 0, howFast * Time.deltaTime);
            if (text.color.a <= 0.01f || text.color.a >=1) howFast = -howFast;
            yield return null;
        }
    }

    public void SkipOpenning()
    {
        timeLine.SetActive(false);
        skipButton.SetActive(false);
    }

    public void GameStatable()
    {
        OpenGame = true;
    }
}
