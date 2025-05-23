using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class SoulEator : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    private NavMeshAgent navMesh;
    private float distance;
    private bool f1=true;
    private bool start = false;
    public bool die = false;
    private bool firstAttack = false;
    private bool isAttack = false;
    private float HP = 100;
    float damage;
    bool f2;

    private float dis;
    private bool powerUp;
    PlayerMove playerMove;
    Tutorial tutorial;

    public GameObject DamageRounge1;
    public GameObject DamageRounge2;

    public GameObject firePosEffect;
    public GameObject fireBall;

    public Transform firePos;

    // Start is called before the first frame update
    void Start()
    {
        f2 = true;
        dis = 6.0f;
        powerUp = false;
        tutorial = FindObjectOfType<Tutorial>();
        player = GameObject.Find("Player(Stage1)");
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        playerMove = FindObjectOfType<PlayerMove>();

        if (navMesh == null)
        {
            Debug.LogError("NavMeshAgent 컴포넌트가 " + gameObject.name + "에 없습니다.");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distance = Vector3.Distance(player.transform.position, this.transform.position);
        }
        else
        {
            Debug.Log("못찾음");
        }

        if (firstAttack)
        {
            StartCoroutine(first());
        }

        if (distance > dis && start && !isAttack && !die)
        {
                navMesh.isStopped = false;
                Debug.Log("플레이어 추적중");
            if (powerUp)
            {
                animator.SetBool("FlyRun", true);
            }
            else
            {
                animator.SetBool("Run", true);
            }
                navMesh.destination = player.transform.position;    // 플레이 추적
            //Debug.Log(distance);

        }
        else if (distance <= dis && start && !isAttack && !die)
            {
                Debug.Log("도착");
                navMesh.isStopped = true;
            if (powerUp)
            {
                animator.SetBool("FlyRun", false);
            }
            else
            {
                animator.SetBool("Run", false);
            }
            StartCoroutine("Attack");
            //Debug.Log(distance);
            }
            

    }
   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            firstAttack = true;
            Destroy(collision.gameObject);
            switch (playerMove.currentMode)
            {
                case PlayerMove.FireMode.Single:
                    damage = 1;
                    break;
                case PlayerMove.FireMode.Burst:
                    damage = 0.8f;
                    break;
                case PlayerMove.FireMode.Auto:
                    damage = 0.5f;
                    break;
            }
            HP -= damage;
            Debug.Log(damage);
            if (HP < 50 && f2)
            {
                f2 = false;
                StartCoroutine("Flying");

            }
            if (HP <= 0)
            {
                tutorial.nextFinal();
                die = true;
                animator.SetBool("Die", true);
            }
        }
            if (isAttack)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    //Debug.Log("대미지 1");
                    playerMove.PlayerDamege(1);
                }
            }
    }

    IEnumerator Flying()
    {
        Debug.Log("날아오르기");
        animator.SetBool("BasicAttack", false);
        animator.SetBool("TailAttack", false);
        dis = 12f;
        animator.SetBool("fly", true);
        tutorial.nextPower();
        yield return new WaitForSeconds(3f);
        powerUp = true;
        navMesh.speed = 10f;
    }

        IEnumerator first()
    {
        if (f1)
        {
            f1 = false;
            animator.SetBool("First", true);
            yield return new WaitForSeconds(4f);
            //Debug.Log("처음 소리치기");
            tutorial.next();
            yield return new WaitForSeconds(2.0f);
            
            start = true;
        }
    }


    IEnumerator Attack()
    {
        isAttack = true;
        int pattern = Random.Range(1, 3);

        if (powerUp) {
            Vector3 directionToPlayer = player.transform.position - firePos.position;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);

            animator.SetBool("FireAttack", true);
            yield return new WaitForSeconds(0.8f);
            Instantiate(firePosEffect, firePos.position, firePos.rotation);
            Instantiate(fireBall, firePos.position, rotationToPlayer);
            yield return new WaitForSeconds(1.0f);

            directionToPlayer = player.transform.position - firePos.position;
            rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            Instantiate(firePosEffect, firePos.position, firePos.rotation);
            Instantiate(fireBall, firePos.position, rotationToPlayer);

            yield return new WaitForSeconds(1.0f);
            directionToPlayer = player.transform.position - firePos.position;
            rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            Instantiate(firePosEffect, firePos.position, firePos.rotation);
            Instantiate(fireBall, firePos.position, rotationToPlayer);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("FireAttack", false);
        }
        else if (pattern == 1)
        {

            animator.SetBool("BasicAttack", true);
            DamageRounge1.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            animator.SetBool("BasicAttack", false);
            DamageRounge1.SetActive(false);


        }else if (pattern == 2)
        {
            animator.SetBool("TailAttack", true);
            DamageRounge2.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            animator.SetBool("TailAttack", false);
            DamageRounge2.SetActive(false);
        }
        isAttack = false;
    }
}