using UnityEngine;

public class Disk : MonoBehaviour
{
    public GameObject BrokenDisk;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, -0.7f, 0);
    }

    public void Destruction()
    {
        Destroy(Instantiate(BrokenDisk, transform.position, transform.rotation),1f);
        
        Destroy(gameObject);

        gm.SpawnDisk();
        gm.ComboUp();
    }
}
