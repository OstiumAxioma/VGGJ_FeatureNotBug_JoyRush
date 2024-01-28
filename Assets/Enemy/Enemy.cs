using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private float speed;

    private SpriteRenderer spriteRenderer;

    // The cooldown time between each attack
    private float attackedCD = 1f;
    private float attackedTimer = 0f;
    private bool isAttacked = false;

    // The time takes drop a level of happiness
    private float statusDegradeCD;
    private float statusDegradeTimer = 0f;

    [SerializeField] private List<Sprite> spriteMap = new();

    public List<AudioClip> audioClips;
    private AudioSource audioSource;
    
    public int status = 0;

    // Start is called before the first frame update
    void Start()
    {
        status = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        StatusDegrade();
        UpdateAttackedTimer();
        WalkToPlayer();
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
    }

    public void SetSpeed(float speed) { this.speed = speed; }

    public void SetAttackedCD(float cd) { this.attackedCD = cd;}

    public void SetStatusDegradeCD(float cd) { this.statusDegradeCD = cd;}
    
    public int GetScore()
    {
        return Mathf.Max(status, 0); 
    }
    
    private void UpdateSprite()
    {
        spriteRenderer.sprite = spriteMap[status];
    }

    private void StatusDegrade(){

        if (isAttacked)
        {
            statusDegradeTimer = 0f;
            return;
        }

        if (statusDegradeTimer >= statusDegradeCD)
        {
            if (status > 4 && status > 0)
            {
                --status;
            }
            UpdateSprite();
            statusDegradeTimer = 0f;
        }
        statusDegradeTimer += Time.deltaTime;
    }

    private void UpdateAttackedTimer()
    {
        if (isAttacked)
        {
            attackedTimer += Time.deltaTime;
            if (attackedTimer >= attackedCD)
            {
                isAttacked = false;
                attackedTimer = 0f;
            }
        }
    }

    private void WalkToPlayer()
    {
        // Check if player is assigned
        if (player != null && status < 5)
        {
            // Move towards the player
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.position += speed * Time.deltaTime * direction;
        }
    }

    void PlayAudio(int clipIndex)
    {
        if (audioSource != null && audioClips != null && clipIndex < audioClips.Count)
        {
            // Set the clip and play
            audioSource.clip = audioClips[clipIndex];
            audioSource.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            if (!isAttacked) {
                if (status < spriteMap.Count)
                {
                    ++status;
                }
                UpdateSprite();
                isAttacked = true;
                if (status < 4) PlayAudio(0);
                else if (status == 4) PlayAudio(1);
                else PlayAudio(2);
                //Debug.Log("audio played"); 
                //Debug.Log("Bullet collision detected, status: " + status.ToString());
            }
        }
    }
}
