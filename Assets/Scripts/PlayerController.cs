using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharController
{
    private GameObject projectileSprite;
    public PNJBehaviour pnj { get; set; }
    private void updateOrientationPlayer()
    {
        Vector3 positon = Input.mousePosition;
        int w = Camera.main.pixelWidth;
        int h = Camera.main.pixelHeight;
        Vector3 orientation = Vector3.Normalize(new Vector3(-positon.x + w / 2f, 0, -positon.y + h / 2f));
        directionAngle = Mathf.Acos(orientation.x) * Mathf.Rad2Deg * Mathf.Sign(orientation.z);
    }

    public void playerToAI(GameObject mindProjectile,GameObject oldBody)
    {
        GameController.Instance.player = mindProjectile;
        Destroy(pnj.controller);
        AIController aiController = gameObject.AddComponent<BasicAI>() as BasicAI;
        if (oldBody.CompareTag("CAC"))
            aiController = gameObject.AddComponent<meleeAI>() as meleeAI;
        else if (oldBody.CompareTag("RANGE"))
            aiController = gameObject.AddComponent<rangeAI>() as rangeAI;
        else if (oldBody.CompareTag("SUPER_RANGE"))
            aiController = gameObject.AddComponent<superRangeAI>() as superRangeAI;
        pnj.controller = aiController;
        rb2D.velocity = new Vector2(0f, 0f);
    }

    public void shootSpirit()
    {
        playSound();
        PNJBehaviour pnj = gameObject.GetComponent<PNJBehaviour>();
        pnj.isPlayer = false;
        GameObject mindProjectile = Instantiate(projectileSprite);
        mindProjectile.GetComponent<SpiritBehaviiour>().oldBody = gameObject;
        playerToAI(mindProjectile, gameObject);
        Vector2 projectileDirection = (Quaternion.Euler(new Vector3(0, 0, directionAngle)) * Vector2.left).normalized;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), mindProjectile.GetComponent<Collider2D>());
        mindProjectile.transform.position = transform.position;
        mindProjectile.transform.Rotate(new Vector3(0, 0, directionAngle));
        mindProjectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * 500 * GameController.Instance.soulSpeed);
        mindProjectile.GetComponent<SpiritBehaviiour>().direction = projectileDirection;
        anim.SetBool("isMoving", false);
    }

    public void move()
    {
        isMoving = false;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            rb2D.velocity = moveDirection * GameController.Instance.playerSpeed;
            isMoving = true;
        }
        else
        {
            rb2D.velocity = new Vector2();
        }
        anim.SetBool("isMoving", isMoving);
    }

    private void playSound()
    {
        System.Random randomDirection = new System.Random();
        int index = randomDirection.Next(0, GameController.Instance.SoulShotSoundList.Length);
        audioSource.clip = GameController.Instance.SoulShotSoundList[index];
        audioSource.Play();
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
        projectileSprite = GameController.Instance.spiritProjectile;
        pnj = GetComponent<PNJBehaviour>();
        if (gameObject.CompareTag("CAC"))
            anim.runtimeAnimatorController = Resources.Load("possMeleeAnimator") as RuntimeAnimatorController;
        else if (gameObject.CompareTag("RANGE"))
            anim.runtimeAnimatorController = Resources.Load("PossGunner") as RuntimeAnimatorController;
        else if (gameObject.CompareTag("SUPER_RANGE"))
            anim.runtimeAnimatorController = Resources.Load("MeleeHumanNormal") as RuntimeAnimatorController;
        else
            anim.runtimeAnimatorController = Resources.Load("basicPosses") as RuntimeAnimatorController;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.alive)
        {
            updateOrientationPlayer();
            move();
            if (Input.GetMouseButtonDown(0))
            {
                shootSpirit();
            }
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);
            rb2D.velocity = new Vector2();
        }

    }

}
