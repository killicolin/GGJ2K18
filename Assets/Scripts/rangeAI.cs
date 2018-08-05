using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeAI : AIController
{
    private float timeSpot;
    private float timeCreation;
    override protected void attacking(Vector3 dist)
    {
        Vector3 playerPos = GameController.Instance.player.transform.position;
        float distance = (playerPos - transform.position).magnitude;


        if (distance > GameController.Instance.ennemiDistanceAttackRange)
        {
            rb2D.velocity = dist * GameController.Instance.ennemi2Speed;
            isMoving = true;
            anim.SetBool("isMoving", isMoving);
        }
        else if (distance < GameController.Instance.ennemiDistanceAttackRange && timeCreation + 1f <= Time.fixedTime && timeSpot != 0f)
        {
            if (timeSpot + 0.5f < Time.fixedTime){ 
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
                timeCreation = Time.fixedTime + GameController.Instance.bulletFireCooldown;
                Shoot(dist);
            }
            else
            {
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
                rb2D.velocity = Vector2.zero;
            }
        }
        else
        {
            isMoving = false;
            anim.SetBool("isMoving", isMoving);
            rb2D.velocity = Vector2.zero;
            timeSpot = Time.fixedTime;
        }

    }

    void Start()
    {
        base.Start();
        timeSpot=0f;
        timeCreation= Time.fixedTime;
        anim.runtimeAnimatorController = Resources.Load("GunGuardController") as RuntimeAnimatorController;
    }

     public void playSound(){
        System.Random randomDirection = new System.Random();
        int index = randomDirection.Next(0, GameController.Instance.PistolSoundList.Length);
        audioSource.clip = GameController.Instance.PistolSoundList[index];
        audioSource.Play();
    }

    void Shoot(Vector3 targetDirection)
    {
        playSound();
        GameObject bullet = Instantiate(GameController.Instance.bulletProjectile);
        Vector2 projectileDirection = (Quaternion.Euler(new Vector3(0, 0, directionAngle)) * Vector2.left).normalized;
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y, -.1f);
        bullet.transform.Rotate(new Vector3(0, 0, directionAngle));
        bullet.GetComponent<Rigidbody2D>().AddForce(projectileDirection * 500 * GameController.Instance.bulletSpeed);
        anim.SetBool("isMoving", false);
    }

}
