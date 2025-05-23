using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Round3txt : MonoBehaviour
{
    public GameObject UIImage;
    public Text displayText;  // UI Text ������Ʈ
    public GameObject UISpacebar;
    public GameObject UICh;
    public GameObject dragon;
    private List<string> messages;
    int currentIndex = 0;
    private PlayerMove playerMove;
    public static bool stage3clear;

    //public static bool dieeee;
    // Start is called before the first frame update
    void Start()
    {
        stage3clear = false;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<PlayerMove>();
        }
        messages = new List<string> { "�̰��� �������� ���� ��� ���ΰ���!",
                                        "���� �츮�� ���� ���� �� �ִ�",
                                        "������ �ӿ��� ��� ������ ���� ��ã�ƹ�",
                                        "���� �ָ��� ���� ������ ��� ���ƹ�������",
                                        "���� �� ��ġ�ϱ� �ٶ���!",
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
        if (playerMove.diePlayer)
        {
            diePlayer();
        }else if (stage3clear) 
        {
            UICh.SetActive(true);
            UIImage.SetActive(true);
            displayText.text = "���� ����Ѱ�!!!!!!! \n 3�� �� �κ�� �̵��˴ϴ�.";
            UISpacebar.SetActive(true);
            StartCoroutine(clearc());
        }

    }

    void ChangeText()
    {
        if (currentIndex < 4)
        {
            displayText.text = messages[++currentIndex];
        }
        else
        {
            delete();
            dragon.SetActive(true);
        }

    }
    void delete()
    {
        UICh.SetActive(false);
        UIImage.SetActive(false);
        displayText.text = "";
        UISpacebar.SetActive(false);
    }
    public void diePlayer()
    {
        UICh.SetActive(true);
        UIImage.SetActive(true);
        displayText.text = "�ƽ��� �׾����ϴ�~ \n 3�� �� �κ�� �̵��˴ϴ�.";
        UISpacebar.SetActive(true);
        StartCoroutine(clearc());

    }

    public IEnumerator clearc()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

}
