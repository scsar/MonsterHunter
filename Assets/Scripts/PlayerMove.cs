using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float walkspeed = 5f;    //�ȴ� ���ǵ�
    public float runspeed = 10f;    //�޸��� ���ǵ�
    public float finalSpeed;
    public GameObject bullet;       //�Ѿ� ������
    public Transform firePos;       //�Ѿ� �߻� ��ġ

    public Camera _camera;
    private CharacterController controller; //�÷��̾�
    private Animator animator;

    public bool toggleCameraRotation;   //�ѷ�����

    public int bulletcount = 30;    // �� �Ѿ� ��
    public int currentbullet = 30;  // ���� �Ѿ� ��
    public TextMeshProUGUI textBullet;  // �Ѿ� �� �ؽ�Ʈ
    public TextMeshProUGUI textBulletMode;  // �Ѿ� ��� �ؽ�Ʈ

    public bool reloading = false;      // ���ε� ��
    public bool run;                    // �޸��� �ִ���
    public float smothness = 10f;

    public bool diePlayer;

    private int HP = 50;

    public Slider HpBar;

    Vector3 aimVec;

    public enum FireMode
    {
        Single,  // �ܹ�
        Burst,   // ����
        Auto     // ����
    }

    public FireMode currentMode = FireMode.Single;
    void Start()
    {
        diePlayer = false;
        HpBar.maxValue  = HP;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        textBullet.text = bulletcount + "/" + currentbullet;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))    //�߻�
        {
            if(!reloading)
                StartCoroutine("Fire");

        }


        if (Input.GetKey(KeyCode.LeftAlt))    // �ѷ�����
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation=false;
        }


        if (Input.GetKey(KeyCode.R))    // ������
        {
            if (!reloading)
            {
                animator.SetBool("Reload", true);
                StartCoroutine("Reload");
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))  //�� ��� ����
        {
            SwitchFireMode();
        }

        if ((Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D)) && !reloading &&!diePlayer)  // �̵�
        {
            animator.SetBool("Front", true);
            InputMovemnet();
        }else if (Input.GetKey(KeyCode.S) && !reloading)
        {
            animator.SetBool("Back", true);
            InputMovemnet();
        }
        else
        {
            animator.SetBool("Front", false);
            animator.SetBool("Back", false);
        }

        HpBar.value = HP;

    }

    private void LateUpdate()
    {
        if(toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1,0,1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime*smothness);
        }
    }

    private bool isRolling = false;
    Vector3 moveDirection;
    void InputMovemnet()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        

        finalSpeed = (run) ? runspeed : walkspeed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (!isRolling)
            {
                //Debug.Log("����");
                StartCoroutine("Roll");
            }
        }

        controller.Move(moveDirection.normalized*finalSpeed * Time.deltaTime);

    }


    IEnumerator Roll()
    {
        isRolling = true;
        controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime * 4f);
        yield return new WaitForSeconds(1f);
        isRolling = false;

    }
    void SwitchFireMode()
    {
        currentMode = (FireMode)(((int)currentMode + 1) % System.Enum.GetValues(typeof(FireMode)).Length);
        textBulletMode.text = "Mode : " + currentMode;
    }

    IEnumerator Reload()        //������
    {
        //Debug.Log("������");
        reloading = true;
        yield return new WaitForSeconds(2f);
        currentbullet = 30;
        animator.SetBool("Reload", false);
        reloading = false;
        textBullet.text = currentbullet + "/" + bulletcount;
        //Debug.Log("������ �Ϸ�");
    }

    public void PlayerDamege(int power)
    {
        //Debug.Log(power + " ��ŭ ����� ����");
        HP -= power;
        if (HP < 0)
        {
            diePlayer = true;
            //Debug.Log("���");
            animator.SetBool("Die", true);
            
        }
    }


    IEnumerator Fire()
    {
        aimVec = (Camera.main.transform.position - firePos.transform.position) + (Camera.main.transform.forward * 50f);
        firePos.transform.rotation = Quaternion.LookRotation(aimVec);
        if(--currentbullet >= 0)
        {
            switch (currentMode)
            {
                case FireMode.Single:
                    Instantiate(bullet, firePos.position, firePos.rotation);
                    yield return new WaitForSeconds(0.3f);
                    break;
                case FireMode.Burst:
                    Instantiate(bullet, firePos.position, firePos.rotation);
                    yield return new WaitForSeconds(0.1f);
                    Instantiate(bullet, firePos.position, firePos.rotation);
                    yield return new WaitForSeconds(0.1f);
                    Instantiate(bullet, firePos.position, firePos.rotation); 
                    break;
                case FireMode.Auto:
                    while (--currentbullet >= 1)
                    {
                        Instantiate(bullet, firePos.position, firePos.rotation);
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
            }
            textBullet.text = currentbullet + "/" + bulletcount;
        }
        else
        {
            textBullet.text = ++currentbullet + "/" + bulletcount;
        }

        
    }

}



