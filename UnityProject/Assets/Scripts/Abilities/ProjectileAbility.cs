using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "Ability/Projectile", order = 0)]
public class ProjectileAbility : CharacterAbility
{
    [Header("Projectile Values")]
    [SerializeField] private Projectile projectile;
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private int bulletDamage = 2;
    [SerializeField] private float timeToSelfDestruct = 4f;

    [Header("Boomerang")]
    [SerializeField] private float timeToReturn = 2f;
    [SerializeField] private bool turnOnlyOnce = false;
    protected override void OnAbilityStarted()
    {
        Projectile newBullet = Instantiate(projectile, character.transform.position, Quaternion.identity);

        if(newBullet.GetType() == typeof(Boomerang))
        {
            newBullet.Setup(Vector2.left, bulletSpeed, bulletDamage, timeToReturn, turnOnlyOnce);
        } 
        else if (newBullet.GetType() ==  typeof(Bullet))
        {
            newBullet.Setup(Vector2.left, bulletSpeed, bulletDamage);
        }

        newBullet.Setup(Vector2.left, bulletSpeed, bulletDamage);

        OnAbilityEnded();
    }
}
