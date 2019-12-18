using UnityEngine;

/// <summary>
/// Ersteller: Joshua Brandes und Patrick Moldavskiy <br/>
/// Zuletzt geändert am: 18.06.2019 <br/>
/// 
/// </summary>
public class ProjectileStandard : Projectile
{

    public void Update()
    {
        if (lifetime > 0)
        {
            lifetime -= Time.deltaTime;     //Lebenszeit wird immer runtergesetzt
        }
        if (lifetime < 0)
        {
            SelfDestruct();     // Wenn die Lebenszeit abgelaufen ist, zerstört sich das Game Object selbt
        }
        float distanceThisFrame = movementSpeed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);     //Das Projektil geht einfach gerade eine Bahn ab
    }
}
