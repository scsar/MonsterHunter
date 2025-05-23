using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Tutorial : MonoBehaviour
{
    public GameObject UIImage;
    public Text displayText;  // UI Text 컴포넌트
    public GameObject UISpacebar;
    public GameObject UICh;
    public GameObject soulEator;
    public GameObject soulEatorEffect;
    public GameObject soulEatorEffect2;
    private List<string> messages;
    private int currentIndex = 0;
    static public bool stage1 = true;
    static public bool stage2 = false;
    static public bool stage3 = false;
    static public bool stage4 = false;
    static public int zumsu;

    public GameObject GoLobby;
    static public bool stageclear = false;

    private bool n;
    // Start is called before the first frame update
    void Start()
    {
        n = false;
        stage1 = true;
        stage2 = false;
        stage3 = false;
        zumsu = 0;
        messages = new List<string> { "안녕하세요, 튜토리얼 입니다",
                                        "눈 앞에 보이는 큐브를 맞춰 보세요.",
                                        "잘했어요 R 키를 눌러 재장전 해보세요",
                                        "아주 잘했어요! 재장전할때는 움직일수 없습니다.",
                                        "탭을 눌러 총 모드도 변경할수 있습니다!",
                                        "단발은 1, 점사는 0.8, 연발은 0.5 데미지입니다.",
                                        "이제 본격적으로 몬스터를 사냥해 봐요!",
                                        "몬스터의 공격을 피하며 처치하세요",
                                        "일정 피가 깍이면 추가 패턴이 등장합니다!",
                                        "1",
                                        "2",
                                        "3",
                                        "4",


        };
        displayText.text = messages[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ChangeText();
        }
        if (stage2)
        {
            stage2 = false;
            displayText.text = messages[++currentIndex];
            create();
            //Debug.Log("stage 2");
            n = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && n)
        {
            n = false;
            delete();
            stage3 = true;
            Invoke("create",2f);
            displayText.text = ".....";
        }
        if (stageclear)
        {
            GoLobby.SetActive(true);
            create();
            displayText.text = "이걸 성공하다니 정말 대단한걸!";
        }
    }
    void ChangeText()
    {
        if(stage1){
            //Debug.Log("stage 1");
            displayText.text = messages[++currentIndex];
            stage1 = false;
            
        }
        else if (stage3)
        {
            //Debug.Log("stage 3");
           
            displayText.text = messages[++currentIndex];
            if (currentIndex >= 6)
            {
                stage3 = false;
                StartCoroutine("Spawn");
            }
        }
        else{
            delete();
        }
    }



    IEnumerator Spawn()
    {
        GameObject ef1 = Instantiate(soulEatorEffect, soulEator.transform.position, Quaternion.identity);
        GameObject ef2 = Instantiate(soulEatorEffect2, soulEator.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        soulEator.SetActive(true);

        yield return new WaitForSeconds(3.0f);
        Destroy(ef1);
        Destroy(ef2);
    }
        public void next()
    {
        create();
        displayText.text = messages[++currentIndex];
        Invoke("delete",3f);
    }

    public void nextPower()
    {
        create();
        displayText.text = messages[++currentIndex];
        Invoke("delete", 3f);
    }

    public void nextFinal()
    {
        create();
        displayText.text = "와우 대단한걸~!\n3초 후 로비로 돌아갑니다.";
        StartCoroutine(clearc());
    }
    public IEnumerator clearc()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
    void delete()
    {
        UICh.SetActive(false);
        UIImage.SetActive(false);
        displayText.text = "";
        UISpacebar.SetActive(false);
    }

    void create()
    {
        UICh.SetActive(true);
        UIImage.SetActive(true);
        UISpacebar.SetActive(true);
    }
    
}
