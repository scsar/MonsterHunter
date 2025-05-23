using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diezone : MonoBehaviour
{
    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerMove = playerObject.GetComponent<PlayerMove>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Player 오브젝트에 닿았는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMove.diePlayer = true;
        }
    }
}
