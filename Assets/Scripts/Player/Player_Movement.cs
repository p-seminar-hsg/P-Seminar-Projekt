﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Zuletzt geändert am 01.07.2019
/// Movementklasse des Players
/// </summary>
public class Player_Movement : MonoBehaviour
{
    [Header("Referenzen")]
    private Joystick joystick; // Referenz zum GameObject MovementJoystick
    private Rigidbody2D rb; // Rigidbody des Players
    private Animator animator; // Animator des Players

    [Header("Knockback")]
    public float knockbackLength;   // Länge des Knockbacks
    private bool isKnockback;

    [Header("Stats")]
    public float speed; // Geschwindigkeit des Players
    public float moveY, moveX;


    // Start wird einmal bei Erstellung des GameObjects aufgerufen
    // Referenzen zu den Components werden hergestellt
    void Start()
    {
        joystick = (Joystick)FindObjectOfType(typeof(Joystick));
        rb = GameObject.Find("Player").GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        animator = GameObject.Find("Player").GetComponent(typeof(Animator)) as Animator;
        isKnockback = false;
    }

    // FixedUpdate wird einmal pro Frame aufgerufen und fragt jedes mal die Position des Joysticks ab. Diese Position wird dann in eine Bewegung für den Player umgerechnet.
    void FixedUpdate()
    {
        if(!GameManager.gameOver)
        {
        moveY = joystick.Vertical;
        moveX = joystick.Horizontal;
        } else{
            moveX = 0;
            moveY = 0;

            //hiermit wird das Knockback deaktiviert
            rb.velocity = Vector2.zero;

            //oder hier die Sterbeanimation einfügen
            animator.SetFloat("speed_horizontal", moveX);
            animator.SetFloat("speed_vertical", moveY);
        }

        // Movement funktioniert nur, wenn gerade kein Knockback stattfindet
        if (isKnockback == false) {
            //Die Geschwindigkeit des Rigidbodys wird je nach position des Joysticks eingestellt
            rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, Time.deltaTime * joystick.Vertical * speed);

        //Die Parameter des Animators werden aktualisiert
        animator.SetFloat("speed_horizontal", moveX);
        animator.SetFloat("speed_vertical", moveY);
        }
    }


    //OnCollsionEnter wird ausgelöst, wenn der Player eine collision detektiert. Wenn der collider als Boundary getagged ist, dann wird die Bewegung des Players eingeschränkt. Dient für unsichtbare Bewegungsgrenzen, kp, ob das jemand braucht
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Bei einer Collision mit einem als "boundaryX" getaggten GameObject wird die x-Bewegung eingeschränkt
        if (collision.collider.tag == "boundaryX")
        {

            rb.velocity = new Vector2(0, Time.deltaTime * joystick.Vertical * speed);
            Debug.Log("Collision with boundaryX");
        }

        //Bei einer Collision mit einem als "boundaryX" getaggten GameObject wird die x-Bewegung eingeschränkt
        if (collision.collider.tag == "boundaryY")
        {

            rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, 0);
            Debug.Log("Collision with boundaryY");
        }
    }

    //Die Geschwindigkeit des Players erhöht sich um den übergebenen Wert

    /// <summary>
    /// Methode, um die Geschwindigkeit des Players zu verändern
    /// </summary>
    /// <param name="newSpeed">Wert, auf den die Geschwindigkeit gesetzt werden soll</param>
    public void setSpeed(float newSpeed)
    {
        speed += newSpeed;
    }

    /// <summary>
    /// Knockback-Couroutine vom Enemy geklaut xD
    /// </summary>
    /// <param name="knockbackDirection">Richtung des Knockbacks</param>
    /// <param name="knockbackStrength">Stärke des Knockbacks</param>
    /// <returns></returns>
    public IEnumerator KnockbackCo(Vector2 knockbackDirection, float knockbackStrength)
    {
        isKnockback = true;
        rb.velocity = knockbackDirection.normalized * knockbackStrength;
        yield return new WaitForSeconds(knockbackLength);
        rb.velocity = Vector2.zero;

        isKnockback = false;
        
    }

}
