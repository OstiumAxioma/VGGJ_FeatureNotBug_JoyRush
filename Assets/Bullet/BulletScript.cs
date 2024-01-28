using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePosition;
    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    private GameObject player;
    private int playerStatus;
    private Rigidbody2D rb;
    private float bulletTimer = 0f;
    public int bulletTime = 3;
    public float force;
    [SerializeField] private List<Sprite> spriteMap = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        playerStatus = playerScript.status;

        updateRender();

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePosition - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        //normalized so that the bullet speed remains the same regardless whether the mouse cursor is further away from the player or closer

        //If want the bullet to rotate
        Vector3 rotation = transform.position - mousePosition;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        bulletTimer += Time.deltaTime;
    }

    private void updateRender()
    {
        if (playerStatus >= 3)
        {
            spriteRenderer.sprite = spriteMap[2];
        }
        else if (playerStatus == 0) 
        {
            spriteRenderer.sprite = spriteMap[0];
        }
        else
        {
            spriteRenderer.sprite = spriteMap[1];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") ||
            bulletTimer >= bulletTime)
        {
            Destroy(gameObject);
        }
    }
}
