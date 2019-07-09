//Authors: Joshua B.; Patrick M.    Main Author: Patrick M.      Last Update: 18.06.2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStandard : Projectile
{

    public void Update()
    {
        if(lifetime > 0)
        {
            lifetime -= Time.deltaTime;     //Lebenszeit wird immer runtergesetzt
        }
        if(lifetime < 0)
        {
            SelfDestruct();     // Wenn die Lebenszeit abgelaufen ist, zerstört sich das Game Object selbt
        }
        float distanceThisFrame = movementSpeed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);     //Das Projektil geht einfach gerade eine Bahn ab
    }




}
