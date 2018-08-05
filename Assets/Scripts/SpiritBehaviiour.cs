using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBehaviiour : MonoBehaviour {
    public float lifeTime;
    public float lerpSpeed;
    private float timeCreation;
    public Vector2 direction { get; set; }
    public GameObject oldBody { get; set; }
    // Use this for initialization
    void Start () {
        timeCreation = Time.fixedTime;
	}

    public IEnumerator backSpirit()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        float startTime = Time.time;
        //GameController.Instance.trackingCamera = false;
        //dehueu mais tant pis
        if (oldBody != null) {
            float journeyLength = Vector3.Distance(Camera.main.transform.position, oldBody.transform.position);
            GameController.Instance.player = oldBody;
            Vector3 FinalPosition = new Vector3(oldBody.transform.position.x, oldBody.transform.position.y, -1f);
            while (Camera.main.transform.position != FinalPosition)
            {
                float distCovered = (Time.time - startTime) * lerpSpeed;
                float fracJourney = distCovered / journeyLength;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, FinalPosition, fracJourney);
                yield return null;
            }
            spiritToAI(oldBody);
        }
        else{
            GameController.Instance.tesMORT();
        }
        //on rend l'objet invisible
    }

    // Update is called once per frame
    void Update () {
        if (Time.fixedTime > timeCreation+lifeTime)
        {
            StartCoroutine(backSpirit());
        }
	}

    private bool spiritToAI(GameObject newBody)
    {
        PNJBehaviour pnj = newBody.GetComponent<PNJBehaviour>();
        bool isSpirit= pnj != null;
        if (pnj != null)
        {
            Destroy(this.gameObject);
            Destroy(pnj.controller);
            PlayerController player = newBody.AddComponent<PlayerController>() as PlayerController;
            pnj.controller = player;
            if (newBody != oldBody)
                GameController.Instance.changeBody(newBody);
            else
                GameController.Instance.player = newBody;
            pnj.isPlayer = true;
        }
        return isSpirit;
    }


    void OnCollisionEnter2D(Collision2D collision){




        if (collision.collider.CompareTag("Bullet"))
        {
            GameController.Instance.tesMORT();
        }
        else
        {
            bool isWall;
            isWall = !spiritToAI(collision.gameObject);
            if (isWall)
            {
                BoxCollider2D collider = collision.gameObject.GetComponent<BoxCollider2D>();
                Transform wallTransform = collision.gameObject.transform;
                Matrix4x4 model = wallTransform.localToWorldMatrix;
                Vector3 xRepere = ((Vector3)model.GetRow(0)).normalized;
                Vector3 yRepere = ((Vector3)model.GetRow(1)).normalized;
                float width = wallTransform.localScale.x;
                float height = wallTransform.localScale.y;
                Vector2 newDirection = -direction;
                float left, right, top, bottom;
                left = Mathf.Abs(wallTransform.position.x + collider.offset.x - width * collider.size.x / 2 - collision.contacts[0].point.x);
                right = Mathf.Abs(wallTransform.position.x + collider.offset.x + width * collider.size.x / 2 - collision.contacts[0].point.x);
                top = Mathf.Abs(wallTransform.position.y + collider.offset.y + height * collider.size.y / 2 - collision.contacts[0].point.y);
                bottom = Mathf.Abs(wallTransform.position.y + collider.offset.y - height * collider.size.y / 2 - collision.contacts[0].point.y);
                float mini;
                mini = Mathf.Min(Mathf.Min(left, right), Mathf.Min(top, bottom));
                if (mini == left || mini == right)
                {
                    newDirection = Vector2.Reflect(direction, xRepere);
                }
                else if (mini == top || mini == bottom)
                {
                    newDirection = Vector2.Reflect(direction, yRepere);
                }
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                gameObject.GetComponent<Rigidbody2D>().AddForce(newDirection * 500 * GameController.Instance.soulSpeed);
                float rotation = Vector3.SignedAngle(direction, newDirection, new Vector3(0, 0, 1));
                gameObject.transform.Rotate(new Vector3(0, 0, rotation));
                direction = newDirection;
            }
        }
    }
}
