using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed = 0f;
    protected int damage = 0;
    protected Vector2 direction = Vector2.left;

    protected void Move(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }
    /// <summary>
    /// Uses the variable 'direction'
    /// </summary>
    protected void Move()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    public virtual void Setup(Vector2 direction, float speed, int damage)
    {

    }
    /// <summary>
    /// Setup for Boomerang
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="speed"></param>
    /// <param name="damage"></param>
    /// <param name="timeToReturn"></param>
    /// <param name="turnOnlyOnce"></param>
    public virtual void Setup(Vector2 direction, float speed, int damage, float timeToReturn, bool turnOnlyOnce)
    {

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<IDamageable>() != null)
        {
            collision.collider.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
