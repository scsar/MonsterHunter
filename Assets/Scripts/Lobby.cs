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

        // 2�� Ű�� ������ ����� �ڵ�
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadScene3();
        }

        // 3�� Ű�� ������ ����� �ڵ�
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

        // ������ ��ο��� ���¸� �����մϴ�.
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
