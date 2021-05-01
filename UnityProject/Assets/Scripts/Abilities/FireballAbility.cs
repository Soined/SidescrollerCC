using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Ability/Fireball", order = 0)]
public class FireballAbility : CharacterAbility
{
    [SerializeField] private Bullet projectile;
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private int bulletDamage = 2;
    protected override void OnAbilityStarted()
    {
        Bullet newBullet = Instantiate(projectile, character.transform.position, Quaternion.identity);

        newBullet.Setup(Vector2.left, bulletSpeed, bulletDamage);
        //Instantiate(projectile, character.transform.position, Quaternion.identity)
        //    .Setup(Vector2.left, bulletSpeed, bulletDamage);


        OnAbilityEnded();
    }
}
