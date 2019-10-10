using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Florian Müller-Martin und Tobias Schwarz
/// Mitarbeiter: Rene Jokiel (Knockback Coroutine vom Enemy geklaut)
/// Zuletzt geändert am 27.07.2019
/// Movementklasse des Players
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
    public float moveY, moveX; //Bewgungsvektorwerte x und y, die eigentlich nur für die Bestimmung der Blickrichtung dienen
    public float actualMoveX, actualMoveY;
    public Vector2 PositionStartOfFrame;
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

        if (!GameManager.gameOver)
        {
            //Die Bewegungsvekrotwerte werden nur aktualisiert, wenn der Joystick nicht in Nullstellung ist
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

            //oder hier die Sterbeanimation einfügen
            animator.SetFloat("speed_horizontal", moveX);
            animator.SetFloat("speed_vertical", moveY);
        }

        // Movement funktioniert nur, wenn gerade kein Knockback stattfindet
        if (isKnockback == false)
        {
            //Die Geschwindigkeit des Rigidbodys wird je nach position des Joysticks eingestellt
            rb.velocity = new Vector2(Time.deltaTime * joystick.Horizontal * speed, Time.deltaTime * joystick.Vertical * speed);




        }
    }

    //Wird nach FixedUpdate aufgerufen
    void LateUpdate()
    {
        // Die Parameter des Animators werden aktualisiert
        if (!isKnockback)
        {
            actualMoveY = (transform.position.y - PositionStartOfFrame.y) * 10;
            actualMoveX = (transform.position.x - PositionStartOfFrame.x) * 10;

            animator.SetFloat("speed_horizontal", actualMoveX);
            animator.SetFloat("speed_vertical", actualMoveY);


            //View Direction wird als Float übergeben, Zahlenwerte parallel zur Anordnung der Attack-Hitboxen vom Player
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
    /// Knockback-Couroutine vom Enemy (Rene Jokiel) geklaut xD
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
