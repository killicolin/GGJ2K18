using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingHUDScript : MonoBehaviour {
    public AnimationClip animDying;
    private Animator animator;
    
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        float timeMultiplier = animDying.length / GameController.Instance.lifeTimeBody;
        animator.SetFloat("time", timeMultiplier);
        animator.SetBool("IsDying", true);

	}
	
    void Update()
    {
        if(Time.fixedTime - GameController.Instance.bodyExpireDate <0.1f)
        {
            animator.SetBool("IsDying", false);
        }
        else
        {
            animator.SetBool("IsDying", true);
        }
    }

	void resetAnimTime()
    {

    }
}
