using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class UIManager : MonoBehaviour
{
    #region public field
    public GameObject winhammer, hammer;
    public GameObject Tap_to_throw;
    public GameObject NextWaveEnemy;
    public GameObject ShowWintext;
    public GameObject Retry_button;
    public Animator Player_anim;
    //particle
    public GameObject WincoinParticle;
    public GameObject WinParticle;
    //UI 
    public Text Coinpointtext;
    public TextMeshProUGUI coincount;
    public Image levelprogressbar;
    public Image Loadnextlevel;
    public Text CurrentLevelText, NextLevelText;
    public Image startload;
    public Text LoadingText;
    //int ,float ,bool,vector etc
    public int winning;
    public float Deadpoint;
    public bool gameover;
    public bool coinincreamenting = true;
    #endregion

    #region private field
    int soundplayturn;
    int coinValue, coinvalueforSave;
    int currentLvl = 1, NextLvl = 2;
    bool moveham;
    bool loadlevel = true;

    #endregion

    // script Reference;
    testPlayer testplayer;

    #region monobehaviour callback 
    // Start is called before the first frame update
    private void Awake()
    {
    
        
        // Saving coin value
        coinvalueforSave = PlayerPrefs.GetInt("Coin");
        Coinpointtext.text = coinvalueforSave.ToString();
        Loadnextlevel.gameObject.SetActive(false);  //loading sceen;
        int mainlevel = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("Level", mainlevel);
    }
    void Start()
    {
        //Levelshowing text setup
        if (PlayerPrefs.GetInt("CurrentLevel") >= 1)
        {
            CurrentLevelText.text = PlayerPrefs.GetInt("CurrentLevel").ToString();
            currentLvl = PlayerPrefs.GetInt("CurrentLevel");
        }
        else
        {
            CurrentLevelText.text = 1.ToString();
        }
        if (PlayerPrefs.GetInt("NextLevel") >= 2)
        {
            NextLevelText.text = PlayerPrefs.GetInt("NextLevel").ToString();
            NextLvl = PlayerPrefs.GetInt("NextLevel");
        }
        else
        {
            NextLevelText.text = 2.ToString();
        }
      
       
       
        ShowWintext.SetActive(false);
        testplayer = GameObject.Find("man-casual").GetComponent<testPlayer>();
        coincount.gameObject.SetActive(false);
        StartCoroutine(startLoading());
    }
    
    // Update is called once per frame
    void Update()
    {
        checkalive();
       
        if (moveham)
        {
           
            winhammer.transform.DOMove(new Vector3(4.08f, 4.7f, 1.01f), 1f);
           
            StartCoroutine(RotateHammer());
        }
     
    }
    #endregion
    #region private method
    private void checkalive()
    {
        //levelprogress bar
        levelprogressbar.fillAmount = Deadpoint / 46;

        //waves of enemy Spawning
        if (Deadpoint >= 18)
        {
           // WaveText.SetActive(true);
            StartCoroutine(nextWaveCountDown());
          //  WaveText.transform.Translate(Vector3.up * 1);
        }
        if (Deadpoint ==winning)
        {
            ShowWintext.SetActive(true);
            WinParticle.SetActive(true);
            winhammer.SetActive(true);
           
            winhammer.transform.position = testplayer.HammerhitPoint;  //tracing back hammer form last pos to win pos
          
            testplayer.whenwin = true;
           // hammer.SetActive(false);
            moveham = true;
            if (soundplayturn < 2)
            {
                GameObject.Find("MusicManager").GetComponent<SoundManager>().Playsound.volume = 1f;
                GameObject.Find("MusicManager").GetComponent<SoundManager>().Sound(0);

            }
            soundplayturn++;
        }
    }
   IEnumerator nextWaveCountDown()
    {
        yield return new WaitForSeconds(.3f);
             NextWaveEnemy.SetActive(true);
      //  Destroy(WaveText);

    }
    IEnumerator RotateHammer()
    {
        yield return new WaitForSeconds(1f); 
      
        winhammer.transform.Rotate(Vector3.forward, 90f * Time.deltaTime);
        yield return new WaitForSeconds(.5f);
       
        WincoinParticle.SetActive(true);
       
        WincoinParticle.transform.position = new Vector3(4.08f, 6.3f, 6.3f);
        coincount.gameObject.SetActive(true);
        //increamenting coin for two second
        while (coinincreamenting)
        {

            coinValue++;
            coincount.text = coinValue+"+".ToString();
            if (coinValue >= 25)
            {
                coinvalueforSave += coinValue;
                PlayerPrefs.SetInt("Coin", coinvalueforSave);
                coinincreamenting = false;
            }
            Coinpointtext.text = coinvalueforSave.ToString();
            yield return new WaitForSeconds(4f);
        }
      
      

    }
    IEnumerator LoadNextLevel()
    {
       
       
        yield return new WaitForSeconds(.01f);
        
        currentLvl++;
        NextLvl++;
        PlayerPrefs.SetInt("CurrentLevel", currentLvl);
        PlayerPrefs.SetInt("NextLevel", NextLvl);
        if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    IEnumerator startLoading()
    {
       
        yield return new WaitForSeconds(4f);
        LoadingText.gameObject.SetActive(false);
       
        startload.gameObject.SetActive(false);

    }
    #endregion

    #region public method
    public void Dead(bool dead)
    {
        if (dead&&!testplayer.whenwin)
        {
            Player_anim.SetBool("Dead", true);
            Retry_button.SetActive(true);
            gameover = true;
        }
    }
    public void TapToThrow()
    {
        Tap_to_throw.SetActive(false);
    }
    public void NextLevel()
    {
        Loadnextlevel.gameObject.SetActive(true);
        StartCoroutine(LoadNextLevel());
    }
   public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
