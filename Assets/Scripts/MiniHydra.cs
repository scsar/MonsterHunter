using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class MiniHydra : MonoBehaviour
{

    private Animator animator;
    private NavMeshAgent navMesh;
    private GameObject player;
    private bool isAttack = false;
    private bool die = false;
    int HP = 5;
    float distance;
    PlayerMove playerMove;

    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) { 
            distance = Vector3.Distance(player.transform.position, this.transform.position);
        }
        if (distance > 4.0f && !isAttack && !die)
        {
            navMesh.isStopped = false;
            //Debug.Log("�÷��̾� ������");
            navMesh.destination = player.transform.position;    // �÷��� ����
        }
        else if (distance <= 4.0f && !isAttack)
        {
            //Debug.Log("����");
            navMesh.isStopped = true;
            StartCoroutine("Attack");
        }

    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            HP--;
            if (HP == 0)
            {
                die = true;
                animator.SetBool("Die", true);
                Destroy(this.gameObject,5f);
            }
        }
        if (other.CompareTag("Player") && isAttack)
        {
            StartCoroutine("Demage");
        }
    }
   
    IEnumerator Demage()
    {
        Debug.Log("���� ����");
        playerMove.PlayerDamege(1);
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator Attack()
    {
        isAttack = true;
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(2.0f);
        //Debug.Log("�̵� ����");
        animator.SetBool("Attack", false);
        isAttack = false;
    }
}
