using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
    public float directionAngle { get; set; }
    public bool isMoving { get; set; }
    protected Rigidbody2D rb2D;
    protected Animator anim;
    protected AudioSource audioSource;
    // Use this for initialization
    protected void Start () {
        anim = gameObject.GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    virtual public void playSound()
    {

    }
    // Update is called once per frame
    void Update () {
    }
}
