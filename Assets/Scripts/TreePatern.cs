using UnityEngine;

public class TreePatern : MonoBehaviour
{

    public static bool HPPlus;
    public GameObject levelUpEffect;
    public GameObject clear;
    public GameObject attackEffect;
    public int TreeHP;
    private bool one;
    GameObject t1;
    // Start is called before the first frame update
    void Start()
    {
        one = true;
        TreeHP = 30;
        t1 = Instantiate(levelUpEffect, this.transform.position, this.transform.rotation);
        HPPlus = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(attackEffect, this.transform.position, this.transform.rotation);
            TreeHP--;
            Debug.Log("³ª¹« ÇÇ : " + TreeHP);
            Destroy(collision.gameObject);  
            if(TreeHP <=0 && one)
            {
                one = false;
                Instantiate(clear, this.transform.position, this.transform.rotation);
                HPPlus = false;
                Destroy(t1);

            }
        }
    }

}
