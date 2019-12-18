using UnityEngine;

/// <summary>
/// Ersteller: Joshua Brandes und Patrick Moldavskiy <br/>
/// Zuletzt geändert am: 18.06.2019 <br/>
/// 
/// </summary>
public class ProjectileHoming : Projectile  // Erbt von der Überklasse Projectile
{
    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;     //Dauerhafter Lebensabzug
        }
        if (lifetime < 0)
        {
            SelfDestruct();
        }
        float distanceThisFrame = movementSpeed * Time.deltaTime;
        direction = player.position - transform.position;       //Die Richtung wird regelmäßig verändert, so dass das Projektil den Player verfolgt.
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }
}
