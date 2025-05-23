using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Round3txt : MonoBehaviour
{
    public GameObject UIImage;
    public Text displayText;  // UI Text 컴포넌트
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
        messages = new List<string> { "이곳이 전설속의 용이 사는 곳인가바!",
                                        "용은 우리의 눈을 속일 수 있대",
                                        "거짓들 속에서 살아 숨쉬는 것을 잘찾아바",
                                        "또한 멀리서 때는 공격은 모두 막아버리나바",
                                        "용을 꼭 퇴치하길 바랄게!",
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
            displayText.text = "정말 대단한걸!!!!!!! \n 3초 후 로비로 이동됩니다.";
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
        displayText.text = "아쉽게 죽었습니다~ \n 3초 후 로비로 이동됩니다.";
        UISpacebar.SetActive(true);
        StartCoroutine(clearc());

    }

    public IEnumerator clearc()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }

}
