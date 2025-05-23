using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamegeGround : MonoBehaviour
{
    bool isAttack = false;
    PlayerMove playerMove;
    [SerializeField]
    private int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isAttack)
        {
            StartCoroutine("Attack");
        }
    }
    
    
    IEnumerator Attack()
    {
        playerMove = FindObjectOfType<PlayerMove>();

        isAttack = true;
        yield return new WaitForSeconds(1.0f);
        //Debug.Log("플레이어 HP 감소");
        if (playerMove != null)
        {
            playerMove.PlayerDamege(damage);
        }

        isAttack = false;
       
    }
}
