using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterFireBall : MonoBehaviour
{
    private float bulletSpedd = 30.0f;
    public GameObject boombEffect;
    PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = FindObjectOfType<PlayerMove>();
        Destroy(this.gameObject, 5f);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpedd * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            
            Instantiate(boombEffect, this.transform);
            Debug.Log("공격 맞음");
            playerMove.PlayerDamege(1);
        }

        //Destroy(this.gameObject);
    }

}
