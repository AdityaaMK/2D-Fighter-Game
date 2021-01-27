using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour
{

    Animator animator, heroAnimator;
    float health = 5;
    public GameObject hero;
    public GameObject hpBar;
    bool alive = true;
    float timer = 0;
    int countOne, countTwo, countThree;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        heroAnimator = hero.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            animator.SetTrigger("walk");
            timer += Time.deltaTime;
            if (countOne < 30)
            {
                if (timer > 0.05)
                {
                    transform.Translate(new Vector2(1, 1) / 8);
                    timer = 0;
                    countOne++;
                }
            }
            else if (countTwo < 30)
            {
                animator.ResetTrigger("attack");
                if (timer > 0.05)
                {
                    transform.Translate(new Vector2(-1, -1) / 8);
                    timer = 0;
                    countTwo++;
                }
            }
            else if (countThree < 1)
            {
                animator.SetTrigger("attack");
                countThree++;
            }
            else
            {
                countOne = 0;
                countTwo = 0;
                countThree = 0;
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
        if (collision.gameObject.tag == "Hero Weapon" && heroAnimator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            health--;
            float percent = health / 5;
            if (percent < 0)
                percent = 0;
            hpBar.transform.localScale = new Vector2(percent, hpBar.transform.localScale.y);
            if(health <= 0)
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
