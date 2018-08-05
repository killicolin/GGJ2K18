using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : CharController
{
    protected Vector3 view;
    private float chasing;
    virtual protected void attacking(Vector3 dist)
    {
      
    }

    public void playSound()
    {

    }
    private void updateOrientationPlayer()
    {
       Vector3 playerPosition = GameController.Instance.player.gameObject.transform.position;
        Vector3 dist = (gameObject.transform.position - playerPosition).normalized;
        float rotation = Mathf.Acos(dist.x) * Mathf.Rad2Deg * Mathf.Sign(dist.y);
        float distanceView = GameController.Instance.ennemiRangeView;
        RaycastHit2D[] hits = Physics2D.RaycastAll(gameObject.transform.position - 0.5f * dist, -dist,distanceView );

        if(hits.Length == 0)
        {
            return;
        }

        if (gameObject.CompareTag("RANGE"))
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.layer == 9 /*LowBlock*/ )
                {
                    continue;
                }
                else if (hit.collider.gameObject == GameController.Instance.player)
                {
                    directionAngle = rotation;
                    attacking(-dist);
                    break;
                }
                else
                {
                    isMoving = false;
                    rb2D.velocity = new Vector2();
                    break;
                }
            }
        }
        else if (gameObject.CompareTag("CAC"))
        {
            if (hits[0].collider != null)
            {
                if (hits[0].collider.gameObject == GameController.Instance.player)
                {
                    directionAngle = rotation;
                    attacking(-dist);
                }
                else
                {
                    isMoving = false;
                    rb2D.velocity = new Vector2();
                }
            }
            else
            {
                isMoving = false;
                rb2D.velocity = new Vector2();
            }
        }

      anim.SetBool("isMoving", isMoving);
    }
    // Use this for initialization
    protected void Start () {
        base.Start();
        view = new Vector3(0, 1, 0);
        directionAngle = Vector3.SignedAngle(new Vector3(0, 1, 0), view, new Vector3(0, 0, 1));
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        updateOrientationPlayer();
    }
}
