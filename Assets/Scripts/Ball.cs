using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private bool RashMode = false;
    private bool comboCont = false;
    private int hp = 3;
    private GameManager gm;

    public bool Play = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!Play)
        {
            return;
        }
        if(hp <= 0)
        {
            gm.GameOver();
            Destroy(gameObject);
        }
        if(Input.touchCount>0 || Input.GetMouseButton(0))
        {
            RashMode = true;
            rb.velocity = new Vector3(0, -12, 0);
        }
    }

    public int GetHp()
    {
        return hp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (RashMode)
        {
            if (other.CompareTag("Glass"))
            {
                other.transform.GetComponentInParent<Disk>().Destruction();
                comboCont = true;
                rb.velocity = new Vector3(0, 0, 0);
            }
            else
            {
                rb.velocity = new Vector3(0, 6, 0);
                if(!gm.ComboDmg() || !comboCont)
                {
                    hp -= 1;
                    gm.UpdateHp();
                    gm.ResetCombo();
                }

                RashMode = false;
                comboCont = false;
            }
        }
        else
            rb.velocity = new Vector3(0,6,0);
    }
}
