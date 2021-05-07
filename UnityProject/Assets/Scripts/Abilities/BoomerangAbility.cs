using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAbility : MonoBehaviour
{

    Vector2 direction = Vector2.left;
    float speed = 1f;
    int damage = 0;
    bool oppositeDirection;
    float maxSpeed = 3f;

    float timeToSlowDown;


    public void Setup(Vector2 direction, float speed, int damage, float timeToSlowDown)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.timeToSlowDown = timeToSlowDown;
    }


  
    // Update is called once per frame
    void Update()
    {
        if (speed <= 0)
        {
            oppositeDirection = true;
            this.direction = Vector2.right;
        }

        transform.Translate(direction * Time.deltaTime * speed);

        if (!oppositeDirection)
        {     

            speed -= 0.5f * Time.deltaTime;
        }
        if(oppositeDirection)
        {
            Mathf.Min(speed += 0.5f * Time.deltaTime, maxSpeed);
                
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<IDamagable>() != null)
        {
            collision.collider.GetComponent<IDamagable>().TakeDamage(damage);
        }
    }
}
