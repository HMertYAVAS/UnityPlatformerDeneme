using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    bool broken = true;
    AudioSource audioSource;
    public AudioClip RobotWalk;
    public AudioClip FixRobot;
    public AudioClip HitEnemy;
    public ParticleSystem smokeEffect;
    private Rigidbody2D rEnemy;
    float timer;
    private float audioTimer=1.0f;
    int direction = 1;
    
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        rEnemy = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource=GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        audioTimer -= Time.deltaTime;

        if (audioTimer < 0 && broken)
        {
            audioSource.PlayOneShot(RobotWalk);
            audioTimer = 1.0f;
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rEnemy.position;
        if (broken)
        {
            
            if (vertical)
            {
                position.y = position.y + Time.deltaTime * speed * direction;
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", direction);
            }
            else
            {
                position.x = position.x + Time.deltaTime * speed * direction;
                animator.SetFloat("Move X", direction);
                animator.SetFloat("Move Y", 0);
            
            }
        
        
    }    
        
        

        rEnemy.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    
    public void Fix(){
        audioSource.PlayOneShot(HitEnemy);
        broken=false;
        
        //rigidbody finish
        rEnemy.simulated=false;

        
        animator.SetTrigger("Fixed");
        audioSource.PlayOneShot(FixRobot);
        smokeEffect.Stop();
        //Bir saniye sonra gameObject destroy
        Destroy (gameObject, 1.0f);

    }
}