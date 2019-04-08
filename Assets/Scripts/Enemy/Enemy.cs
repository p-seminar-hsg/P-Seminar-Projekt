using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Diese Attribute regeln die Eigenschaften der Gegner. Jeder Enemy erbt von dieser Klasse diese Attribute
    public float speed;
    public float strength;
    public float range;
    public float healthPoints;
    public float attackRadius;
    public float attackCooldown;
    public float takeDamageCooldown;
    //Der Tag ist wichtig, damit die Gegner NUR mit dem Spieler interagieren und nicht mit anderen GameObjects
    public string enemyTag = "Player";




	}



