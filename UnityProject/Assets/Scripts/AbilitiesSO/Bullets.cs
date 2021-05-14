using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : Projectile
{
    private void Update()
    {
        Move();
    }

    public override void Setup(Vector2 direction, float speed, int damage)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
    }
    
}
