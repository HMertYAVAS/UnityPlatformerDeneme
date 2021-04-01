using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private Rigidbody2D rBullet;
    // Start is called before the first frame update
    void Awake()
    {
        rBullet = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction,float force)
    {
        rBullet.AddForce(direction*force);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if(e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
    }
}
