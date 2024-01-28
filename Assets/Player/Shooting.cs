using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePosition;
    public GameObject bullet;
    public Transform bulletTransform;
    private Transform player;
    public bool canFire;
    private float timer;
    private float timeBetweenFiring = 0.3f;
 
    public List<AudioClip> audioClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        canFire = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement parentScript = player.GetComponent<PlayerMovement>();
        int playerStatus = parentScript.status;
        UpdateTimeBetweenFire(playerStatus);

        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePosition - transform.position;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        if (!canFire) //Prevent continuous fire for a timer's period of time
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring )
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire) //If click with left mouse
        {
            canFire = false;
            Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            int indx = Random.Range(0, audioClips.Count);
            //Debug.Log("audioClip size: " + audioClips.Count.ToString());
            //Debug.Log("random number: " + indx);
            PlayAudio(indx);
        }
    }

    void UpdateTimeBetweenFire(int playerStatus)
    {
        if (playerStatus >= 3)
        {
            timeBetweenFiring = 0.1f;
        }
        else if (playerStatus > 0) 
        {
            timeBetweenFiring = 0.3f;
        }
        else
        {
            timeBetweenFiring = 0.5f;
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
}
