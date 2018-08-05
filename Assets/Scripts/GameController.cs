using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    protected AudioSource audioSource;
    public GameObject player{ get; set; }
    public bool alive = true;
    public float cameraRange;
    private static GameController instance;
    public GameObject spiritProjectile;
    public GameObject bulletProjectile;
    public float lifeTimeBody;
    public float bodyExpireDate{ get; set; }
    public float ennemiRangeView;
    public float ennemiDistanceAttackRange;
    public float soulSpeed;
    public float bulletSpeed;
    public float bulletFireCooldown;
    public float playerSpeed;
    public float ennemi1Speed;
    public float ennemi2Speed;
    public AudioClip[] DeathSoundList;
    public AudioClip[] PistolSoundList;
    public AudioClip[] ClubSoundList;
    public AudioClip[] SoulShotSoundList;
    public AudioClip[] TrackList;
    public AudioClip ElevatorOpen;
    public AudioClip ElevatorUp;
    public AudioClip ElevatorClose;
    public bool canBeKilled = true;
    //Horloge
    public Text minutes;
    public Text secondes;
    public Text separator;
    float startTime;
    float minf;
    float secf;

    int min;
    int sec;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameController)) as GameController;
                //instance = new GameObject("game").AddComponent<GameController>();
            return instance;
        }
    }

    public void changeBody(GameObject newPlayer){
        bodyExpireDate = Time.fixedTime;
        player = newPlayer;
    }

    // Use this for initialization
    void Start(){
        PlayerController playerController = (PlayerController)FindObjectOfType(typeof(PlayerController));
        player = playerController.gameObject;
        bodyExpireDate = Time.fixedTime;
        Camera.main.orthographicSize = cameraRange;
        audioSource = GetComponent<AudioSource>();
        Physics2D.IgnoreLayerCollision(12, 12);//ignore bullet collision with eachother
        Physics2D.IgnoreLayerCollision(8, 9);//lowBlock soul shoot
        Physics2D.IgnoreLayerCollision(12, 9);//lowBlock shoot

        startTime = Time.time;
    }

    void Awake()
    {
        if (instance != null && instance != this){
            DestroyImmediate(gameObject);
        }
        else{
            instance = this;
        }
    }
    private void updateMainCamera()
    {

            Camera.main.transform.position = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y,-1);
    }
/*
    public IEnumerator resetLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/

    public void tesMORT(){
        if (alive && canBeKilled)
        {
            audioSource.clip = playSoundMort();
            audioSource.Play();
            alive = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void soundOpenElevator()
    {
        audioSource.clip = ElevatorOpen;
        audioSource.Play();
    }


    public void soundCloseElevator()
    {
        audioSource.clip = ElevatorClose;
        audioSource.Play();
    }

    public void soundGoUpElevator()
    {
        audioSource.clip = ElevatorUp;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update () {
        updateMainCamera();
        if (bodyExpireDate + lifeTimeBody < Time.fixedTime)
        {
            tesMORT();
        }
        //horloge
        if (canBeKilled)
        {
            minf = (Time.time - startTime) % 60;
            secf = (int)(Time.timeSinceLevelLoad * 1000f) % 1000; ;

            min = (int)minf;
            sec = (int)secf;

            minutes.text = min.ToString("00");
            secondes.text = sec.ToString("00");
        }
        else
        {
            minutes.color = new Color(1.0f, 0.0f, 0.0f);
            secondes.color = new Color(1.0f, 0.0f, 0.0f);
            separator.color = new Color(1.0f, 0.0f, 0.0f);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public AudioClip playSoundMort()
    {
        System.Random randomDirection = new System.Random();
        int index = randomDirection.Next(0, GameController.Instance.DeathSoundList.Length);
        return GameController.Instance.DeathSoundList[index];
    }
}
