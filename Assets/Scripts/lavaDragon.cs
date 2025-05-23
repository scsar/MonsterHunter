using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class lavaDragon : MonoBehaviour
{
    public GameObject player;
    private Animator animator;
    private float HP;
    private bool isAttack = false; // 공격중인지
    private bool firstAttack = false; // 처음 피격상황
    private bool isGuard;

    public bool isClear = false;
    float damage;
    private float Timer = 0f;
    private float Cooltime = 3f;
    private bool isCooltime = true;

    private bool isCoroutineRunning = false;

    [SerializeField]
    private GameObject breathEffect, earthEffect, meteorEffect,
    summonEffect, waitEffect, wallEffect;

    [SerializeField]
    private GameObject NormalRange, DashRange, QuakeRange, FlameRange;

    [SerializeField]
    private GameObject pillar;

    private bool isGimic = false;
    private bool hp70 = true;
    private bool hp50 = true;
    [SerializeField]
    private Transform move;

    private bool start = false; // 플레이어 추적여부(첫 피격여부)
    public bool die = false;

    private float distance;
    private NavMeshAgent navMesh;
    PlayerMove playerMove;

    // 코루틴 함수 first를 활성화 하기위한 bool
    private bool f1 = true;

    // 몬스터의 행동 패턴을 정해주기 위한 int 변수
    int pattern = 0;

    // Start is called before the first frame update
    void Awake()
    {
        Tutorial.stageclear = false;
        playerMove = FindObjectOfType<PlayerMove>();
        player = GameObject.FindWithTag("Player");
        HP = 500;
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooltime)
        {
            Timer += Time.deltaTime;
            if (Timer >= Cooltime)
            {
                Timer = 0f;
                isCooltime = false;
            }
        }


        //Debug.Log(HP);
        if (player != null)
        {
            // 플레이어가 존재한다면, 플레이어와 몬스터의 현재 위치를 계산
            distance = Vector3.Distance(player.transform.position, this.transform.position);
        }
        if (firstAttack)
        {
            StartCoroutine(first());
        }
        if (distance > 10.0f&& start && !isAttack && !die)
        {
            // 플레이어와 일정거리이상 떨어져 있을떄.
            // 플레이어에게 접근하기위한 코드
            navMesh.isStopped = false;
            isGuard = true;
            animator.SetBool("Run", true);
            // 목표를 플레이어의 위치로 설정
            navMesh.destination = player.transform.position; 
        }
        else if(distance <= 10.0f && start &&!isAttack)
        {
            // 접근하였을경우, 공격을 수행한다.
            navMesh.isStopped = true;
            isGuard = false;
            animator.SetBool("Run", false);
            if (!isGimic)
                StartCoroutine(Attack());

        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // 총알과 충돌했을떄, 최초 피격 bool을 활성화 하고, hp를 감소시킨다.
            firstAttack = true;
            if (isGuard && !isCoroutineRunning)
            {
                start = false;
                navMesh.isStopped = true;
                animator.SetBool("Guard", true);
                animator.SetTrigger("GuardStart");
                StartCoroutine(GuardWait());
            }
            else if (!isGuard && !isGimic)
            {
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
            }
            Destroy(collision.gameObject);
            if (HP/50 <= 7 && hp70)
            {
                hp70 = false;
                isGimic = true;
                StartCoroutine(FirstPattern());
            }
            /*else if (HP/50  <= 5 && hp50)
            {
                hp50 = false;
                isGimic = true;
                StartCoroutine(SecondsPattern(move));
            }*/
            if (HP <= 0) {
                Debug.Log("die");
                die = true;
                animator.SetTrigger("Die");
                //Tutorial.stageclear = true;
                Round3txt.stage3clear = true;
            }
        }
        if (isAttack)
        {
            // 공격중 플레이어와 부딪치는 경우 플레이어의 hp를 감소시킨다.
            if (collision.gameObject.CompareTag("Player"))
            {
                playerMove.PlayerDamege(1);

            }
        }
    }

    IEnumerator first()
    {
        if (f1)
        {
            // 최초 조우조건을 만족하기위한 코루틴 함수
            f1 = false;
            animator.SetTrigger("FirstHit");
            yield return new WaitForSeconds(2.5f);
            start = true;
        }
    }

    IEnumerator Attack()
    {
        // 공격시 이동과 관련된 bool변수를 false로 돌리고, 공격관련 변수를 true로 돌린다. 
        start = false;
        isAttack = true;

        if (isCooltime)
        {
            pattern = 1;
        }
        else
        {
            pattern = Random.Range(2, 6);
            isCooltime = true;
        }

        if (pattern == 1)
        {
            // bite
            animator.SetBool("NormalAttack",true);
            NormalRange.SetActive(true);
            yield return new WaitForSeconds(1);
            animator.SetBool("NormalAttack", false);
            NormalRange.SetActive(false);
            isAttack = false;
        }
        
        if (pattern == 2)
        {
            //Dash
            animator.SetBool("DashAttack", true);
            DashRange.SetActive(true);
            yield return new WaitForSeconds(4f);
            DashRange.SetActive(false);
            animator.SetBool("DashAttack", false);
            isAttack = false;
        }

        if (pattern == 3)
        {
            // breath
            animator.SetBool("FlameAttack", true);
            Transform breathPosition = transform.GetChild(0).transform;
            FlameRange.SetActive(true);
            Vector3 temp = breathPosition.transform.position;
            yield return new WaitForSeconds(0.6f);
            GameObject breath = Instantiate(breathEffect);
            breath.transform.position = breathPosition.position;
            breath.transform.parent = breathPosition;
            breath.transform.rotation = breathPosition.rotation;
            for (int i = 0; i < 10; i++)
            {
                breath.transform.Translate(-0.15f, 0, 0);
                yield return new WaitForSeconds(0.2f);
            }
            FlameRange.SetActive(false);
            animator.SetBool("FlameAttack", false);
            Destroy(breath);
            breathPosition.position = temp;
            isAttack = false;
        }
        
        if (pattern == 4)
        {
            // earth quake
            animator.SetBool("NormalAttack", true);
            yield return new WaitForSeconds(1);
            GameObject Quake = Instantiate(earthEffect);
            QuakeRange.SetActive(true);
            Quake.transform.position = this.transform.position;
            Quake.transform.rotation = this.transform.rotation;
            animator.SetBool("NormalAttack", false);
            QuakeRange.SetActive(false);
            Destroy(Quake, 2f);
            isAttack = false;
        }

        if (pattern == 5)
        {
            // Meteor
            animator.SetBool("Fly", true);
            animator.SetBool("StayFly", false);
            yield return new WaitForSeconds(2);
            GameObject Meteor = Instantiate(meteorEffect);
            Meteor.transform.position = this.transform.position;
            Meteor.transform.rotation = this.transform.rotation;
            yield return new WaitForSeconds(4);
            animator.SetBool("Fly", false);
            yield return new WaitForSeconds(3);
            Destroy(Meteor);
            isAttack = false;
        }

        if (!isGimic)
            start = true;
    }

    
    IEnumerator GuardWait()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(0.5f);
        start = true;
        navMesh.isStopped = false;
        isGuard = false;
        animator.SetBool("Guard", false);
        isCoroutineRunning = false;
    }

    IEnumerator FirstPattern()
    {
        yield return new WaitForSeconds(0.5f);
        start = false;
        navMesh.isStopped = true;
        Vector3[] directions = new Vector3[3];
        List<GameObject> pillars = new();

        directions[0] = transform.forward * 5f;           
        directions[1] = -transform.forward * 5f;          
        directions[2] = transform.right * 5f;        
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 spawnPosition = transform.position + directions[i] * 2.5f;
            GameObject born = Instantiate(summonEffect, spawnPosition, Quaternion.identity);
            Destroy(born, 2.0f );
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < directions.Length - 1; i++)
        {
            Vector3 spawnPosition = transform.position + directions[i] * 2.5f;
            GameObject wrong = Instantiate(pillar, spawnPosition, Quaternion.identity);
            pillars.Add(wrong);
            wrong.GetComponentInChildren<Animator>().SetBool("isCorrect", false);
        }
        GameObject cor = Instantiate(pillar, transform.position + directions[2] * 2.5f, Quaternion.identity);
        cor.GetComponentInChildren<Animator>().SetBool("isCorrect", true);
        pillars.Add(cor);
        GameObject wait = Instantiate(waitEffect, this.transform.position, Quaternion.identity);
        yield return new WaitUntil ( () => isClear == true);

        for (int i = 0; i < pillars.Count; i++)
        {
            Destroy(pillars[i]);
        }

        Destroy(wait);
        isGimic = false;
        start = true;
    }


    IEnumerator SecondsPattern(Transform move)
    {
        start = false;
        navMesh.isStopped = false;

        animator.SetBool("Fly", true);
        animator.SetBool("StayFly", true);
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 30; i ++)
        {
            navMesh.destination = move.position;
            yield return new WaitForSeconds(0.3f);     
        }
        animator.SetBool("StayFly", false);
        animator.SetBool("Fly", false);
        yield return new WaitForSeconds(2);

        GameObject Wall = Instantiate(wallEffect, move.GetChild(0).position, Quaternion.identity);
        GameObject Wall2 = Instantiate(wallEffect, move.GetChild(1).position, Quaternion.identity);

        yield return new WaitForSeconds(2);

        navMesh.destination = player.transform.position;
        Cooltime /= 2;
        start = true;
        // TODO
        // 패턴 쿨타임 감소

    }

}
