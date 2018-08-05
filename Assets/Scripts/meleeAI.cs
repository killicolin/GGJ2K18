using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAI : AIController{
    override protected void attacking(Vector3 dist)
    {
        rb2D.velocity = dist * GameController.Instance.ennemi1Speed;
        isMoving = true;
    }

    public void playSound()
    {
        System.Random randomDirection = new System.Random();
        int index = randomDirection.Next(0, GameController.Instance.ClubSoundList.Length);
        audioSource.clip = GameController.Instance.ClubSoundList[index];
        audioSource.Play();
    }

    void Start()
    {
        base.Start();
        anim.runtimeAnimatorController = Resources.Load("MeleeHumanNormal") as RuntimeAnimatorController;
    }
}
