using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public int status;
    [SerializeField] private List<Sprite> spriteMap = new List<Sprite>();
    //[SerializeField] private Vector3 rotationSpeed = new();
    /*
    public float enemyDetectionDistance = 5f;
    private float status = 0f;
    private float statusChangeTimer = 0f;
    public float statusChangeUnit = 0.1f;
    
    private float statusChangeCD;
    public float statusChangeSlowLimit = 0.5f;
    public float statusChangeFastLimit = 0.1f;
    public int MaxStatusGap = 10;
    */

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        status = 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs(); 
        
        //transform.Rotate(rotationSpeed * Time.deltaTime);

        /*
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyDetectionDistance);
        if (colliders.Any(collider => collider.CompareTag("Enemy"))) //If at least one enemy is detected
        {
            int enemyStatusSum = 0;
            int numOfEnemy = 0;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        enemyStatusSum += enemy.status;
                        numOfEnemy++;
                    }
                }
            }
            //Debug.Log("Number of enemies detected: " + numOfEnemy);
            //Debug.Log("Sum of detected enemies' status: " + enemyStatusSum);
            AffectPlayerStatus(enemyStatusSum);

            if(status >= 1.5) 
            {
                spriteRenderer.sprite = g2;
            }
            else if(status >= 0.5 && status < 1.5)
            {
                spriteRenderer.sprite = g1;
            }
            else if(status > -0.5 && status < 0.5)
            {
                spriteRenderer.sprite = g0;
            }
            else if(status <= -0.5 && status > -1.5)
            {
                spriteRenderer.sprite = n1;
            }
            else if(status <= -1.5 && status >= -2)
            {
                spriteRenderer.sprite= n2;
            }
        }
        */
        
    }
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized; //Todo later
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                //Debug.Log("Enemy: " + enemyScript.status + ", Player: " + this.status);
                if(enemyScript.status < 5 && this.status > 0)
                {
                    this.status--;
                }
                else if (enemyScript.status >= 5 && this.status < 4)
                {
                    this.status++;
                }
            }
        }
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = spriteMap[status];
    }

    /*
    private void AffectPlayerStatus(int enemyStatusSum){
        if (enemyStatusSum != status)
        {
            float statusGap = Math.Abs(enemyStatusSum - status);
            if(statusGap > MaxStatusGap)
            {
                statusGap = MaxStatusGap;
            }

            UpdateStatusChangeCD(statusGap); //Control the status change rate by CD. Change Unit stays the same.
            
            if (statusChangeTimer >= statusChangeCD)
            {
                if (status > -2 && status > enemyStatusSum)
                {
                    status -= statusChangeUnit;
                }
                else if (status < 2 && status < enemyStatusSum)
                {
                    //Debug.Log("++++ now enemyStatusSum: " + enemyStatusSum);
                    status += statusChangeUnit;
                }

                playerStatusText.text = status.ToString();
                //Debug.Log("----Current Player status: " + status);

                statusChangeTimer = 0f;
            }
        }
        statusChangeTimer += Time.deltaTime;
    }

    private void UpdateStatusChangeCD(float statusGap)
    {
        statusChangeCD = statusChangeFastLimit +
                        (statusChangeSlowLimit - statusChangeFastLimit) * (1 - statusGap / MaxStatusGap); //The bigger the statusGap, the smaller the statusChangeCD 

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, enemyDetectionDistance);
    }
    */
}
