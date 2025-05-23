using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject shotEffect;
    private float bulletSpedd = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject shot = Instantiate(shotEffect, this.transform.position, this.transform.rotation);
        Destroy(shot,1.0f);
        Destroy(gameObject, 3.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * bulletSpedd * Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            GameObject hit = Instantiate(hitEffect, collision.transform.position, collision.transform.rotation);
            Destroy(hit, 1.0f);
        }
        if (collision.gameObject.CompareTag("target"))
        {
            GameObject hit = Instantiate(hitEffect, collision.transform.position, collision.transform.rotation);
            Tutorial.zumsu++;
            Destroy(hit, 1.0f);
            if (Tutorial.zumsu >= 3){
                Tutorial.stage2 = true;
            }
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

    }
}
