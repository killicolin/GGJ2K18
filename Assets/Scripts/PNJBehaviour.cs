using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PNJBehaviour : MonoBehaviour {
    public bool isPlayer { get; set; }
    public CharController controller { get; set; }
    private Animator anim;
    protected AudioSource audioSource;
    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
        controller = GetComponent<CharController>();
        audioSource = GetComponent<AudioSource>();
        if (controller is PlayerController)
            isPlayer = true;
        else
            isPlayer = false;
    }
	
    private void choiceSprite(){
        float limitvalue = 180f / 8f;
        float direction=0f;
        if (controller.directionAngle < 0 & controller.directionAngle > -limitvalue || controller.directionAngle >= 0 & controller.directionAngle <= limitvalue){
            direction = 1f;
        }
        else if (controller.directionAngle > -limitvalue * 3 & controller.directionAngle <= -limitvalue){
            direction = 2f;
        }
        else if (controller.directionAngle > -limitvalue * 5 & controller.directionAngle <= -limitvalue * 3) {
            direction = 3f;
        }
        else if (controller.directionAngle > -limitvalue * 7 & controller.directionAngle <= -limitvalue * 5){
            direction =4f;
        }
        else if (controller.directionAngle > limitvalue * 7 || controller.directionAngle <= -limitvalue * 7){
            direction = 5f;
        }
        else if (controller.directionAngle > limitvalue * 5 & controller.directionAngle <= limitvalue * 7){
            direction = 6f;
        }
        else if (controller.directionAngle > limitvalue * 3 & controller.directionAngle <= limitvalue * 5){
            direction = 7f;
        }
        else{
            direction = 8f;
        }
        anim.SetFloat("direction", direction);
    }

	// Update is called once per frame
	void Update () {
        //valeur de limite
        choiceSprite();
    }

    public IEnumerator deathBody()
    {
        
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isDead", false);
        yield return new WaitForSeconds(0.5f);
        if (gameObject == GameController.Instance.player)
            GameController.Instance.tesMORT();
        Destroy(gameObject);

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.collider.gameObject;
        bool colliderKill = go.CompareTag("CAC") || go.CompareTag("RANGE") ;

        if (isPlayer &&( colliderKill || go.CompareTag("Bullet")))
        {
            if (isPlayer && colliderKill)
            {
                if(go.CompareTag("CAC"))
                    go.GetComponent<meleeAI>().playSound();
                else
                    go.GetComponent<rangeAI>().playSound();
            }
            GameController.Instance.tesMORT();
        }
        else if (go.CompareTag("Bullet"))
        {
            audioSource.clip = GameController.Instance.playSoundMort();
            audioSource.Play();
            anim.SetBool("isDead", true);
            StartCoroutine(deathBody());

            //Destroy(gameObject);
        }
    }
}
