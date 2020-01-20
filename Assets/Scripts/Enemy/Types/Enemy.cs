using System.Collections;
using UnityEngine;

/// <summary>
/// Ersteller: Rene Jokiel und Benedikt Wille <br/>
/// Mitarbeiter: Florian Müller-Martin (Combatsystem und Animationen) <br/>
/// Zuletzt geändert am: 19.01.2020 <br/>
/// Die Superklasse und damit Grundlage für alle Enemies.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float healthPoints_max;
    public float speed;
    public int strength;
    public float range;

    [Header("Knockback")]
    public float knockbackLength;
    public float attackKnockback;

    [Header("Scaling")]
    public int scoreForDefeat = 10;

    [Header("Cooldowns")]
    public float damageCooldown;
    public float attackCooldown;
    public float takeDamageCooldown;

    [Header("Drops")]
    public GameObject[] drops;
    [Range(0, 1)]
    [Tooltip("Die Wahrscheinlichkeiten für die Drops. WICHTIG: Jeder Index hier gehört zum selben Index in Drops")]
    public float[] dropProbs;

    [Header("Variablen für die Animation (Flomm)")]
    public Animator animator; // Link zum Animator
    public float actualMoveX, actualMoveY; // Die Bewegungswerte des Enemies im letzten Frame
    public float stoppedActualMoveX, stoppedActualMoveY; // Die Bewegungswerte des Enemies im letzten Frame, die aber beibehalten werden, wenn der Enemy sich nicht bewegt. Nötig für die Idle Animationen
    public Vector2 positionStartOfFrame; // Die Position am Anfang des Frames

    protected Rigidbody2D rb;
    public bool movementLocked;

    /* Am Anfang 0 - Sobald der Enemy gehittet wird, wird localDamageCooldown
     * damageCooldown gleich gesetzt und dann jeden Frame um die verstrichene
     * Zeit verringert (muss in den erbenden Scripts extra implementiert werden!) */
    protected float localDamageCooldown;

    /// <summary>
    /// TakeHit-Methode, die jeder Enemy implementieren muss (wird aufgerufen sobald der Enemy getroffen wird) (überarbeitet von Flomm)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    /// <param name="damage">Schaden, der zugefügt wird</param>
    public abstract void TakeHit(Vector2 knockbackDirection, float damage);

    /// <summary>
    /// Knockback-Coroutine, die man zusammen mit TakeHit()  
    /// verwenden sollte (kann optional überschrieben werden!) (überarbeitet von Flomm)
    /// </summary>
    /// <param name="knockbackDirection">Der Richtungsvektor des Knockbacks - wird intern normalisiert</param>
    public virtual IEnumerator KnockbackCo(Vector2 knockbackDirection)
    {
        movementLocked = true;
        rb.velocity = knockbackDirection.normalized * knockbackLength * 17; // (17 ~= 5/0,3)
        yield return new WaitForSeconds(knockbackLength);
        rb.velocity = Vector2.zero;
        movementLocked = false;
    }

    /// <summary>
    /// Dropt mit einer gewissen Wahrscheinlichkeit ein oder mehrere Item(s) aus drops.
    /// Sollte in den Unterklassen beim Tod aufgerufen werden!
    /// (Kann bei Bedarf in den Unterklassen überschrieben werden)
    /// </summary>
    protected virtual void DropItem()
    {
        // Der Gegner kann mehrere Items droppen
        for (int i = 0; i < drops.Length; i++)
        {
            // Wahrscheinlichkeit
            float random = ((float)Random.Range(0, 100)) * 0.01f;
            if (dropProbs[i] != 0 && random <= dropProbs[i])
            {
                GameObject.Instantiate(drops[i], transform.position, Quaternion.identity);
            }
        }
    }

    /// <summary>
    /// Scaling. Verändert die Werte des Enemies basierend auf dem derzeitigen Score.
    /// </summary>
    public virtual void ScaleStats()
    {
        int score = GameManager.GetScore();

        int roomsCleared = GameManager.instance.roomsCleared;

        // Nur jeden 2. Raum wird "stark" gescaled 
        if (roomsCleared % 2 == 0)
        {
            healthPoints_max += score / 180;
            speed += score / 2750;
            strength += score / 2750;
        } else
        {
            healthPoints_max += score / 250;
            speed += score / 3300;
            strength += score / 3300;
        }
        //Debug.Log("Health: " + healthPoints_max + " / Speed: " + speed + " / Strength: " + strength);
    }

    #region Animationen
    /// <summary>
    /// Aktualisiert die Bewegungs- und Blickrichtungsparameter des Animators
    /// <paramref name="isShootingEnemy">Ist der Enemy ein Fernkampfgegner?</paramref>/>
    /// </summary>
    public void RefreshAnimator(bool isShootingEnemy)
    {
        Direction viewDirection;

        actualMoveY = (this.gameObject.transform.position.y - positionStartOfFrame.y) * 10;
        actualMoveX = (this.gameObject.transform.position.x - positionStartOfFrame.x) * 10;

        animator.SetFloat("speed_horizontal", actualMoveX);
        animator.SetFloat("speed_vertical", actualMoveY);

        if (!isShootingEnemy)
        {
            if (this.gameObject.transform.position.y - positionStartOfFrame.y != 0) //Wird nur aktualisiert, wenn der Enemy sich bewegt hat
            {
                stoppedActualMoveY = (this.gameObject.transform.position.y - positionStartOfFrame.y) * 10;
            }

            if (this.gameObject.transform.position.x - positionStartOfFrame.x != 0) //Wird nur aktualisiert, wenn der Enemy sich bewegt hat
            {
                stoppedActualMoveX = (this.gameObject.transform.position.x - positionStartOfFrame.x) * 10;
            }
            viewDirection = GetViewDirection(stoppedActualMoveX, stoppedActualMoveY);
        }
        else
        {
            Vector2 directionPlayer = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
            viewDirection = GetViewDirection(directionPlayer.x, directionPlayer.y);
        }

        //View Direction wird als Float übergeben, Zahlenwerte parallel zur Anordnung der Idle-Animationen im BlendTreeIdle

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

    /// <summary>
    /// Diese Methode gibt die aktuelle Blickrichtung des Enemies als Direction zurück
    /// <paramref name="moveX">Tatsächlicher X-Bewegungswert des Gegners</paramref>/>
    /// <paramref name="moveY">Tatsächlicher Y-Bewegungswert des Gegners</>
    /// </summary>
    public Direction GetViewDirection(float moveX, float moveY)
    {

        // Wenn der Enemy in keine Richtung schaut, dann schaut er nach unten; wichtig wenn der Enemy vorher noch nicht gelaufen ist}
        Direction viewDirection = Direction.DOWN;

        float absMoveX = Mathf.Abs(moveX);
        float absMoveY = Mathf.Abs(moveY);

        // Die Richtung in die der Enemy schaut, wird bestimmt

        if (moveX > 0 && absMoveX > absMoveY)
        {
            // Enemy schaut nach rechts
            viewDirection = Direction.RIGHT;
        }

        else if (moveY > 0 && absMoveY > absMoveX)
        {
            // Enemy schaut nach oben
            viewDirection = Direction.UP;
        }

        else if (moveX < 0 && absMoveX > absMoveY)
        {
            // Enemy schaut nach links
            viewDirection = Direction.LEFT;
        }

        else if (moveY < 0 && absMoveY > absMoveX)
        {
            // Enemy schaut nach unten
            viewDirection = Direction.DOWN;
        }

        return viewDirection;
    }
    #endregion
}
