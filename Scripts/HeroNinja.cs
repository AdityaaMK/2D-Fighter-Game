using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNinja : MonoBehaviour
{

    Animator animator, enemyAnimator;
    int attackCoolDown = 0; // prevents space bar attack spamming
    float health = 5;
    public GameObject enemy;
    public GameObject hpBar;

    enum direction
    {
        left, right
    }

    direction playerDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerDirection = direction.left;
        enemyAnimator = enemy.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<EyesBlink>().enabled)
        {
            if (Input.GetKeyDown("space") && attackCoolDown < 0)
            {
                animator.SetTrigger("attack");
                attackCoolDown = 100;
            }
            attackCoolDown--;
            if (Input.GetKey("left"))
            {
                if (playerDirection == direction.right)
                {
                    playerDirection = direction.left;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                transform.Translate(Vector2.left / 20, Space.World);
                animator.SetBool("walk", true);
            }
            else if (Input.GetKey("right"))
            {
                if (playerDirection == direction.left)
                {
                    playerDirection = direction.right;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                transform.Translate(Vector2.right / 20, Space.World);
                animator.SetBool("walk", true);
            }
            else if (Input.GetKey("up"))
            {
                transform.Translate(Vector2.up / 20, Space.World);
                animator.SetBool("walk", true);
            }
            else if (Input.GetKey("down"))
            {
                transform.Translate(Vector2.down / 20, Space.World);
                animator.SetBool("walk", true);
            }
            else
            {
                animator.SetBool("walk", false);
            }
        }
    }

    void fall(GameObject child)
    {
        //Debug.Log(child);
        Rigidbody2D childRB = child.GetComponent<Rigidbody2D>();
        if (child.GetComponent<BoxCollider2D>())
            child.GetComponent<BoxCollider2D>().enabled = true;
        if (!childRB)
            childRB = child.AddComponent<Rigidbody2D>();
        var force = 5 * new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        child.transform.parent = null;
        childRB.AddForce(force, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy Weapon" && enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            health--;
            float percent = health / 5;
            if (percent < 0)
                percent = 0;
            hpBar.transform.localScale = new Vector2(percent, hpBar.transform.localScale.y);
            if (health <= 0)
            {
                animator.enabled = false;
                this.GetComponent<EyesBlink>().enabled = false;
                if (this.transform.GetChild(0).transform.childCount > 0)
                {
                    for (int i = this.transform.GetChild(0).gameObject.transform.GetChild(0).transform.GetChild(0).transform.childCount - 1; i != -1; i--)
                    {
                        GameObject child = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                        Destroy(child, 6 + Random.Range(0.5f, 0.5f));
                        fall(child);
                    }
                    for (int i = this.transform.GetChild(0).gameObject.transform.GetChild(0).transform.GetChild(1).transform.childCount - 1; i != -1; i--)
                    {
                        GameObject child = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).gameObject;
                        Destroy(child, 6 + Random.Range(0.5f, 0.5f));
                        fall(child);
                    }
                    for (int i = this.transform.GetChild(0).gameObject.transform.GetChild(0).transform.GetChild(2).transform.childCount - 1; i != -1; i--)
                    {
                        GameObject child = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).gameObject;
                        Destroy(child, 6 + Random.Range(0.5f, 0.5f));
                        fall(child);
                    }
                    for (int i = this.transform.GetChild(0).gameObject.transform.GetChild(0).transform.childCount - 1; i != -1; i--)
                    {
                        GameObject child = this.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
                        Destroy(child, 5 + Random.Range(0.5f, 0.5f));
                        fall(child);
                    }
                }
                for (int i = this.transform.GetChild(0).gameObject.transform.childCount - 1; i != -1; i--)
                {
                    GameObject child = this.transform.GetChild(0).transform.GetChild(0).gameObject;
                    Destroy(child, 4 + Random.Range(0.5f, 0.5f));
                    fall(child);
                }
                Destroy(this.gameObject, 4 + Random.Range(0.5f, 0.5f));
            }
        }
    }
}
