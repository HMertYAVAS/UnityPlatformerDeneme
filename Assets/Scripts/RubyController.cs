using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public static bool showCursor;
    
    public float speed = 3.0f;
    
    private float timer;
    public int maxHealth = 5;
    public float timeInvincible=2.0f;
    
    public int health { get { return currentHealth; }}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;
    
    public GameObject projectilePrefab;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    Animator animator;
    // Start is called before the first frame update
    Vector2 lookDirection = new Vector2(1,0);
    AudioSource audioSource;
    public AudioClip PlayerWalking;
    public AudioClip PlayerHit;
    void Start()
    {
        #region This will make Unity render 10 frames per second.
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;
        #endregion

        
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Cursor.visible = false;
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move =new Vector2(horizontal,vertical);

        if (!Mathf.Approximately(move.x,0.0f) || !Mathf.Approximately(move.y,0.0f))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
        }
        

        animator.SetFloat("Look X",lookDirection.x);
        animator.SetFloat("Look Y",lookDirection.y);
        animator.SetFloat("Speed",move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible =false;
            }
            
        }

        if (timer<0)
        {
            timer=0.75f;
        if(!Mathf.Approximately(move.x,0.0f) || !Mathf.Approximately(move.y,0.0f)){
            PlaySound(PlayerWalking);
        }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NPC character = hit.collider.GetComponent<NPC>();
            if(character != null)
            {
                character.DisplayDialog();
            }
        }
        }

        


    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            return;

            isInvincible =true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(PlayerHit);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if ( currentHealth == 0)
        {
            Destroy(gameObject);
        }
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        //if (lookDirection.y==-1)
        //{
        //GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.down * 0.2f, Quaternion.identity);

        //Projectile projectile = projectileObject.GetComponent<Projectile>();
        //projectile.Launch(lookDirection, 300);
        //}
        //else
        //{
            
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.4f, Quaternion.identity);
        
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 400);
    
        //}
        
        animator.SetTrigger("Launch");
    }
    public void PlaySound(AudioClip clip)
{
    audioSource.PlayOneShot(clip);
}

}
