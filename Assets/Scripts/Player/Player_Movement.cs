﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz <br/>
/// Mitarbeiter: Benedikt Wille (Knockback Coroutine vom Enemy geklaut) <br/>
/// Zuletzt geändert am: 07.12.2019 <br/>
/// Movementklasse des Players.
/// </summary>
public class Player_Movement : MonoBehaviour
{
    #region Variablen
    [Header("Referenzen")]
    private Joystick joystick; // Referenz zum GameObject MovementJoystick
    private Rigidbody2D rb; // Rigidbody des Players
    private Animator animator; // Animator des Players

    [Header("Knockback")]
    public float knockbackLength;   // Länge des Knockbacks
    private bool isKnockback;

    [Header("Movement")]
    public static float speed; // Geschwindigkeit des Players   (Jetzt static, damit ich darauf Zugriff im Main Script habe. Rene Jokiel; 27.7.2019)
    public float moveY, moveX; //Bewegungsvektorwerte x und y, die eigentlich nur für die Bestimmung der Blickrichtung dienen
    public float actualMoveX, actualMoveY; //Reale Bewegungsvektorwerte x und y
    public Vector2 PositionStartOfFrame; //Position des Players wird am Anfang des Frames gespeichert
    #endregion

    #region Lifecycle-Methoden
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
        //Position vor der Bewegung wird gespeichert
        PositionStartOfFrame = transform.position;

        if (!GameManager.gameOver && !GameObject.Find("Player").GetComponent<Player_Main>().isDead)
        {
            //Die Bewegungsvektorwerte werden nur aktualisiert, wenn der Joystick nicht in Nullstellung ist
            if (joystick.Vertical != 0 || joystick.Vertical != 0)
            {

                moveY = joystick.Vertical;
                moveX = joystick.Horizontal;

            }

        }
        else
        {
            moveX = 0;
            moveY = 0;

            //hiermit wird das Knockback deaktiviert
            rb.velocity = Vector2.zero;

            animator.SetFloat("speed_horizontal", moveX);
            animator.SetFloat("speed_vertical", moveY);
        }

        // Movement funktioniert nur, wenn gerade kein Knockback stattfindet und der Spieler noch lebt
        if (isKnockback == false && !GameObject.Find("Player").GetComponent<Player_Main>().isDead)
        {
            //Die Geschwindigkeit des Rigidbodys wird je nach position des Joysticks eingestellt

            rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, Time.deltaTime * joystick.Vertical * speed);
        }
    }

    //Wird nach FixedUpdate aufgerufen
    void LateUpdate()
    {
        // Die Parameter des Animators werden aktualisiert
        if (!isKnockback && animator.GetInteger("die")==0)
        {
            actualMoveY = (transform.position.y - PositionStartOfFrame.y) * 10;
            actualMoveX = (transform.position.x - PositionStartOfFrame.x) * 10;

            animator.SetFloat("speed_horizontal", actualMoveX);
            animator.SetFloat("speed_vertical", actualMoveY);


            //View Direction wird als Float übergeben, Zahlenwerte parallel zur Anordnung der Idle-Animationen im BlendTreeIdle
            Direction viewDirection = Player_Main.getViewDirection();
            if (viewDirection == Direction.DOWN)
            {
                animator.SetFloat("viewDirection", 1);
            }
            else if (viewDirection == Direction.RIGHT)
            {
                animator.SetFloat("viewDirection", 2);
            }
            else if (viewDirection == Direction.UP)
            {
                animator.SetFloat("viewDirection", 3);
            }
            else if (viewDirection == Direction.LEFT)
            {
                animator.SetFloat("viewDirection", 4);
            }
        }
    }
    #endregion

    /// <summary>
    /// Knockback-Couroutine vom Enemy (Benedikt Wille) geklaut xD
    /// </summary>
    /// <param name="knockbackDirection">Richtung des Knockbacks.</param>
    /// <param name="knockbackStrength">Stärke des Knockbacks.</param>
    public IEnumerator KnockbackCo(Vector2 knockbackDirection, float knockbackStrength)
    {
        isKnockback = true;
        rb.velocity = knockbackDirection.normalized * knockbackStrength;
        yield return new WaitForSeconds(knockbackLength);
        rb.velocity = Vector2.zero;

        isKnockback = false;
    }

}
