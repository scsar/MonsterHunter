using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField]
    private GameObject correctEffect, wrongEffect;

    private GameObject lava;

    void Awake()
    {
        lava = GameObject.FindWithTag("Monster");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if(transform.GetChild(0).GetComponent<Animator>().GetBool("isCorrect"))
            {
                //Debug.Log("파훼성공");
                GameObject cor = Instantiate(correctEffect, this.transform.position, Quaternion.identity);
                Destroy(cor, 1f);
                lava.GetComponent<lavaDragon>().isClear = true;
            }
            else
            {
                //Debug.Log("실패");
                transform.GetChild(1).gameObject.SetActive(true);
                GameObject wrong = Instantiate(wrongEffect, this.transform.position, Quaternion.identity);
                Destroy(wrong, 1f);
            }
            Destroy(gameObject, 1f);
        }
    }
}
