using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Round2txt : MonoBehaviour
{
    public GameObject UIImage;
    public Text displayText;  // UI Text ������Ʈ
    public GameObject UISpacebar;
    public GameObject UICh;
    private List<string> messages;
    int currentIndex = 0;

    public GameObject hydra;
    public GameObject GoLobby;
    static public bool stage2clear= false;

    private PlayerMove playerMove;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<PlayerMove>();
        }
        stage2clear = false;
        messages = new List<string> { "�̰����� ���� ����� ����� ���",
                                        "������ ������ ������ ��ȯ�ؼ� ����� �����Ѵٰ���",
                                        "������ ������ ���� �����!",
                                        "�׷� ����� ����!",
        };
        displayText.text = messages[currentIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeText();
        }
        if (stage2clear)
        {
            UICh.SetActive(true);
            UIImage.SetActive(true);
            displayText.text = "���� ����Ѱ�!!\n3�� �Ŀ� �κ� �̵��˴ϴ�.";
            UISpacebar.SetActive(true);
            GoLobby.SetActive(true);
            StartCoroutine(clearc());
        }
        if (playerMove.diePlayer)
        {
            UICh.SetActive(true);
            UIImage.SetActive(true);
            displayText.text = "�ƽ��� �׾��׿�~~\n3�� �Ŀ� �κ� �̵��˴ϴ�.";
            UISpacebar.SetActive(true);
            GoLobby.SetActive(true);
            StartCoroutine(clearc());
        }
    }
    void ChangeText()
    {
        if (currentIndex < 3)
        {
            displayText.text = messages[++currentIndex];
        }
        else
        {
            hydra.SetActive(true);
            delete();
        }
    }
        void delete()
    {
        UICh.SetActive(false);
        UIImage.SetActive(false);
        displayText.text = "";
        UISpacebar.SetActive(false);
    }
    public IEnumerator clearc()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
}
