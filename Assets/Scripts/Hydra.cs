using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Hydra : MonoBehaviour
{
    private Animator animator;
    private float HP;
    public GameObject player;
    private NavMeshAgent navMesh;
    private bool isAttack = false;
    private bool firstAttack = false;
    private bool start = false;
    public bool die = false;
    private float distance;
    public GameObject miniMonster;
    public GameObject bornEffect;
    public GameObject groundEffect;
    public GameObject groundEffect2;
    public GameObject groundDamagerounge;
    public GameObject fireDamagerounge;
    public GameObject fireEffect;
    public GameObject attackDamagerounge;

    public GameObject levelUPEffect;
    public GameObject levleUPTree;
    public GameObject fail;

    public Transform fireFos;
    PlayerMove playerMove;
    public bool f2;
    public bool f3;
    public bool powerUP;
    float damage;

    // Start is called before the first frame update
    void Start()
    {
        Tutorial.stageclear = false;
        f2 = true;
        f3 = true;
        powerUP = false;
        playerMove = FindObjectOfType<PlayerMove>();
        player = GameObject.Find("Player");
        HP = 200;
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            distance = Vector3.Distance(player.transform.position, this.transform.position);
        }
        if (firstAttack)
        {
            StartCoroutine(first());
        }
        if (distance > 6.0f&& start && !isAttack && !die)
        {
            navMesh.isStopped = false;
            //Debug.Log("플레이어 추적중");
            animator.SetBool("Run", true);
            navMesh.destination = player.transform.position;    // 플레이 추적
        }
        else if(distance <= 6.0f && start &&!isAttack)
        {
            //Debug.Log("도착");
            navMesh.isStopped = true;
            animator.SetBool("Run", false);
            StartCoroutine("Attack");


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
            Debug.Log(HP);
            
            if (HP <= 0) {
                die = true;
                animator.SetBool("DIe",true);
                Round2txt.stage2clear = true;
                StartCoroutine("back");
            }
        }
        if (isAttack)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerMove.PlayerDamege(1);

            }

        }
    }
    IEnumerator back()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(1);
    }
        bool f1= true;  
    IEnumerator first()
    {
        if (f1)
        {
            f1 = false;
            animator.SetBool("FirstHit", true);
            yield return new WaitForSeconds(2.5f);
            //Debug.Log("처음 소리치기");
            start = true;
        }
    }



    int pattern;
    IEnumerator Attack()
    {

        int pattern = Random.Range(1, 4);

      /*  if (powerUP)
        {
            pattern = 2;
        }
        else if(pattern < 3)
        {
            pattern++;
        }
        else
        {
            pattern = 1;
        }*/
        
        start = false;
        isAttack = true;

        if (HP < 150 && f2)
        {
            f2 = false;
            Debug.Log("각성 중");
            animator.SetBool("LevelUp", true);
            GameObject up = Instantiate(levelUPEffect, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(0.5f);
            GameObject up2 = Instantiate(levelUPEffect, this.transform.position, this.transform.rotation);



            Vector3 direction = -transform.forward * 5f;     


                Vector3 spawnPosition = transform.position + direction * 3f;
                GameObject born = Instantiate(bornEffect, spawnPosition, Quaternion.identity);
                Destroy(born, 2.0f);

                yield return new WaitForSeconds(1.5f);
                GameObject tree = Instantiate(levleUPTree, spawnPosition, Quaternion.identity);




            yield return new WaitForSeconds(15f);

            if (TreePatern.HPPlus)
            {
                f2 = true;
                HP = 200;
                Instantiate(fail, spawnPosition, Quaternion.identity);
                //Debug.Log("실패");
            }
            else
            {
                Debug.Log("성공");
                HP = 140;
                powerUP = true;
            }
            Destroy(tree);
            animator.SetBool("LevelUp", false);
            Destroy(up); Destroy(up2);

            isAttack = false;
        }

        if (HP < 50 && f3)
        {
            f3 = false;
            Debug.Log("각성 중2");
            animator.SetBool("LevelUp", true);
            GameObject up = Instantiate(levelUPEffect, this.transform.position, this.transform.rotation);
            yield return new WaitForSeconds(0.5f);
            GameObject up2 = Instantiate(levelUPEffect, this.transform.position, this.transform.rotation);



            Vector3 direction = -transform.forward * 5f;
            Vector3 direction2 = transform.forward * 5f;

            Vector3 spawnPosition = transform.position + direction * 3f;
            Vector3 spawnPosition2 = transform.position + direction2 * 3f;

            GameObject born = Instantiate(bornEffect, spawnPosition, Quaternion.identity);
            GameObject born2 = Instantiate(bornEffect, spawnPosition2, Quaternion.identity);
            Destroy(born, 2.0f); Destroy(born2, 2.0f);

            yield return new WaitForSeconds(1.5f);
            GameObject tree = Instantiate(levleUPTree, spawnPosition, Quaternion.identity);
            GameObject tree2 = Instantiate(levleUPTree, spawnPosition2, Quaternion.identity);



            yield return new WaitForSeconds(30f);

            if (TreePatern.HPPlus)
            {
                f2 = true;
                HP = 100;
                Instantiate(fail, spawnPosition, Quaternion.identity);
                Debug.Log("실패");
            }
            else
            {
                Debug.Log("성공");
                HP = 40;
            }
            Destroy(tree);
            animator.SetBool("LevelUp", false);
            Destroy(up); Destroy(up2);

            isAttack = false;
        }



        else if (powerUP&& pattern ==2)
        {
            animator.SetBool("spawn", true);
            fireDamagerounge.SetActive(true);
            yield return new WaitForSeconds(1f);

            Vector3 directionToPlayer = player.transform.position - fireFos.transform.position;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);

            GameObject dust = Instantiate(fireEffect, fireFos.position, rotationToPlayer);
            yield return new WaitForSeconds(1f);
            directionToPlayer = player.transform.position - fireFos.transform.position;
            rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            GameObject dust2 = Instantiate(fireEffect, fireFos.position, rotationToPlayer);
            yield return new WaitForSeconds(1f);
            directionToPlayer = player.transform.position - fireFos.transform.position;
            rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            GameObject dust3 = Instantiate(fireEffect, fireFos.position, rotationToPlayer);
            animator.SetBool("spawn", false);
            yield return new WaitForSeconds(2f);
            Destroy(dust); Destroy(dust2); Destroy(dust3);
            fireDamagerounge.SetActive(false);
            isAttack = false;
        }

  


        if(pattern == 1)
        {
            animator.SetBool("spawn", true);
            yield return new WaitForSeconds(3.0f);
            Debug.Log("스폰 공격");


            Vector3[] directions = new Vector3[4];

            directions[0] = transform.forward * 2.5f;           // 앞
            directions[1] = -transform.forward * 2.5f;          // 뒤
            directions[2] = transform.right * 2.5f;             // 오른쪽
            directions[3] = -transform.right * 2.5f;            // 왼쪽

            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 spawnPosition = transform.position + directions[i] * 2.5f;
                GameObject born = Instantiate(bornEffect, spawnPosition, Quaternion.identity);
                Destroy(born, 2.0f );
            }
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 spawnPosition = transform.position + directions[i] * 2.5f;
                Instantiate(miniMonster, spawnPosition, Quaternion.identity);
            }

            animator.SetBool("spawn", false);
            yield return new WaitForSeconds(1.0f);

            isAttack = false;
        }

        if (!powerUP && pattern == 2)
        {
            animator.SetBool("HornAttack", true);
            attackDamagerounge.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            Debug.Log("이동 공격");
            animator.SetBool("HornAttack", false);
            yield return new WaitForSeconds(1.0f);
            attackDamagerounge.SetActive(false);


            isAttack = false;
        }


        if (pattern == 3) {
            Debug.Log("점프 공격");
            animator.SetBool("jump",true);
            yield return new WaitForSeconds(1f);
            GameObject born = Instantiate(groundEffect, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            GameObject born2 = Instantiate(groundEffect, this.transform.position, Quaternion.identity);
            groundDamagerounge.SetActive(true);
            yield return new WaitForSeconds(1f);
            GameObject born3 = Instantiate(groundEffect, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            GameObject born4 = Instantiate(groundEffect2, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            GameObject born5 = Instantiate(groundEffect2, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            groundDamagerounge.SetActive(false);

            animator.SetBool("jump", false);
            isAttack = false;
        }



        Debug.Log("공격 종료");
        
        start = true;
    }

    
}
