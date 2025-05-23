using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Lobby : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadScene2();
        }

        // 2번 키를 누르면 실행될 코드
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene3();
        }

        // 3번 키를 누르면 실행될 코드
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LoadScene4();
        }

    }

    public void LoadScene()
    {
        StartCoroutine(FadeToBlack(1));

    }
    IEnumerator FadeToBlack(int index)
    {
        fadeImage.gameObject.SetActive(true);
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 완전히 어두워진 상태를 보장합니다.
        color.a = 1f;
        fadeImage.color = color;
        SceneManager.LoadScene(index);
    }

    public void LoadScene2()
    {
        StartCoroutine(FadeToBlack(2));
    }
    public void LoadScene3()
    {
        StartCoroutine(FadeToBlack(3));
    }
    public void LoadScene4()
    {
        StartCoroutine(FadeToBlack(4));
    }


}
