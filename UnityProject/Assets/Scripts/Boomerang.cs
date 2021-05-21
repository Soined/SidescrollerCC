using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Projectile
{
    float maxSpeed = 3f;
    float timeToReturn = 0f;
    float _timeToReturn = 0f;
    bool turnOnlyOnce = false;
    float slowDownTime = .5f;

    public override void Setup(Vector2 direction, float maxSpeed, int damage, float timeToReturn, bool turnOnlyOnce)
    {
        this.direction = direction;
        this.maxSpeed = maxSpeed;
        this.damage = damage;
        this.timeToReturn = _timeToReturn = timeToReturn;
        this.turnOnlyOnce = turnOnlyOnce;
    }



    // Update is called once per frame
    void Update()
    {
        _timeToReturn = Mathf.Max(_timeToReturn - Time.deltaTime, 0);

        if(timeToReturn - _timeToReturn <= slowDownTime) //Ramping up
        {
            speed = ((timeToReturn - _timeToReturn) / slowDownTime) * maxSpeed; 
        } else if(_timeToReturn <= slowDownTime) //Ramping down
        {
            speed = (_timeToReturn / slowDownTime) * maxSpeed;

            if(_timeToReturn == 0)
            {
                direction = -1f * direction;
                _timeToReturn = timeToReturn;
            }
        } else
        {
            speed = maxSpeed;
        }


        Move();


        //if (speed <= 0)
        //{
        //    oppositeDirection = true;
        //    this.direction = Vector2.right;
        //}

        //transform.Translate(direction * Time.deltaTime * speed);

        //if (!oppositeDirection)
        //{     

        //    speed -= 0.5f * Time.deltaTime;
        //}
        //if(oppositeDirection)
        //{
        //    Mathf.Min(speed += 0.5f * Time.deltaTime, maxSpeed);
                
        //}
    }
}
