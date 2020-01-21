using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ersteller: Luca Kellermann <br/>
/// Zuletzt geändert am: 21.01.2020 <br/>
/// Script, das für die Bewegung und Wegfindung der Enemies verantwortlich ist.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    /// <summary>Referenz auf das Enemy-Script eines Enemy.</summary>
    public Enemy enemyScript;

    /// <summary>Referenz auf den Collider eines Enemy.</summary>
    private BoxCollider2D enemyCollider;

    /// <summary>Referenz auf den Player.</summary>
    private GameObject player;

    /// <summary>Referenz auf den aktuellen Room</summary>
    private Room currentRoomScript;

    /// <summary>2D-Array mit allen existierenden Nodes des aktuellen Rooms.</summary>
    private AStarNode[,] currentRoomNodes;

    /// <summary>Node, zu der der Enemy als nächstes gehen soll.</summary>
    private AStarNode currentWaypoint;

    /// <summary>Nodes initialisiert?</summary>
    private bool foundNodes;

    /// <summary>Blockiert kein Hindernis den direkten Weg vom Enemy zum Player?</summary>
    private bool straightLineToPlayer;

    /// <summary>Ist der Enemy bereits "im" Player?</summary>
    private bool playerTooNear;

    /// <summary>Ist der Player in der Range des Enemy?</summary>
    private bool playerInRange;

    /// <summary>Int-Wert für die Layer der Collision-Tilemaps der Rooms.</summary>
    private int layerMask;


    void Awake()
    {
        //Player finden
        player = GameObject.FindGameObjectWithTag("Player");

        //Collider finden
        enemyCollider = GetComponent<BoxCollider2D>();

        //Nodes noch nicht gefunden
        foundNodes = false;

        //Layer initialisieren (Info zu Layer Masks: https://www.youtube.com/watch?v=oFoDZvUdfq0)
        layerMask = 1 << 10;
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
    }


    // Update is called once per frame
    void Update()
    {

        if (!GameManager.gameOver && player != null)
        {

            //Updates der bool-Werte
            UpdateStraightLineToPlayer();
            UpdatePlayerTooNear();
            UpdatePlayerInRange();

            if (foundNodes && !enemyScript.movementLocked && playerInRange && !straightLineToPlayer && !playerTooNear)
            {

                //Tile-Koordinaten von startNode und targetNode
                Vector3Int start = currentRoomScript.groundTilemap.WorldToCell(gameObject.transform.position);
                Vector3Int target = currentRoomScript.groundTilemap.WorldToCell((Vector3)player.GetComponent<Rigidbody2D>().worldCenterOfMass);

                //startNade und targetNode erstellen
                AStarNode startNode = currentRoomScript.GetNodeWith(start.x, start.y);
                AStarNode targetNode = currentRoomScript.GetNodeWith(target.x, target.y);

                //Pfad zum Player finden
                FindPath(startNode, targetNode);
            }
        }
    }

    void FixedUpdate()
    {

        if (!GameManager.gameOver && player != null && foundNodes && !enemyScript.movementLocked && playerInRange && !playerTooNear)
        {
            if (straightLineToPlayer)
            {
                //Enemy in gerader Linie zum Player bewegen
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyScript.speed * Time.deltaTime);
            }
            else if (currentWaypoint != null)
            {
                //Enemy zur nächsten Node im gefundenen Pfad bewegen
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.posX + 0.5f, currentWaypoint.posY + 0.5f, player.transform.position.z), enemyScript.speed * Time.deltaTime);
            }
        }
    }

    /// <summary>Halbe Sekunde verzögerte Initialisierungen.</summary>
    private IEnumerator LateStart()
    {

        //erst mit Pathfinding beginnen, wenn vollständig zur Scene gefadet wurde
        yield return new WaitForSeconds(0.5f);

        //Initialisierungen
        currentRoomScript = MapManager.instance.currentRoomScript;
        currentRoomNodes = currentRoomScript.nodes;

        //jetzt kann mit Pathfinding begonnen werden
        foundNodes = true;
    }


    /// <summary>Überprüft ob sich der Player in einer vom Enemy aus geraden Linie ohne Hindernisse befindet.</summary>
    private void UpdateStraightLineToPlayer()
    {
        Bounds colliderBounds = enemyCollider.bounds;

        //Vektoren mit den Begrenzungen des enemyColliders
        Vector3 bound1 = colliderBounds.min;
        Vector3 bound2 = colliderBounds.max;
        Vector3 bound3 = new Vector3(colliderBounds.min.x, colliderBounds.max.y, 0);
        Vector3 bound4 = new Vector3(colliderBounds.max.x, colliderBounds.min.y, 0);

        //Linecast() ermittelt das erste Hindernis in der Linie von Start zu Ziel (durch layerMask werden nur die Collider der Tilemaps mit "ColliderTilemap"-Layer beachtet)
        //Linecast().rigidbody ist das erste Hindernis => ist es null, ist der Weg frei => straightLineToPlayer = true
        straightLineToPlayer = (Physics2D.Linecast((Vector2)bound1, (Vector2)player.transform.position, layerMask).rigidbody == null) &&
                               (Physics2D.Linecast((Vector2)bound2, (Vector2)player.transform.position, layerMask).rigidbody == null) &&
                               (Physics2D.Linecast((Vector2)bound3, (Vector2)player.transform.position, layerMask).rigidbody == null) &&
                               (Physics2D.Linecast((Vector2)bound4, (Vector2)player.transform.position, layerMask).rigidbody == null);
    }

    /// <summary>Überprüft, ob sich der Player innerhalb der Reichweite des Enemy befindet.</summary>
    private void UpdatePlayerInRange()
    {
        //aus Renes Enemy1 Script übernommen
        playerInRange = (Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(enemyScript.range, 2));
    }

    /// <summary>Überprüft ob der Enemy fast direkt im Player ist.</summary>
    private void UpdatePlayerTooNear()
    {
        playerTooNear = (Vector3.SqrMagnitude(player.transform.position - transform.position) <= 0.25f);
    }

    /// <summary>Findet einen möglichst kurzen Pfad mithilfe des A*-Algorythmus.</summary>
    /// <param name="start">AStarNode, von der der Pfad ausgehen soll.</param>
    /// <param name="target">AStarNode, zu der der Pfad führen soll.</param>
    private void FindPath(AStarNode start, AStarNode target)
    {

        //hier werden die Nodes während der Pfadfindung nach ihren jeweiligen Zuständen gespeichert
        List<AStarNode> openNodes = new List<AStarNode>();
        HashSet<AStarNode> closedNodes = new HashSet<AStarNode>();

        openNodes.Add(start);

        //noch Nodes zum überprüfen übrig?
        while (openNodes.Count > 0)
        {
            AStarNode currentNode = openNodes[0];

            //currentNode auf Node mit geringster fCost setzen (bzw. hCost bei gleicher fCost)
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].gPlusHCost < currentNode.gPlusHCost || (openNodes[i].gPlusHCost == currentNode.gPlusHCost && openNodes[i].hCost < currentNode.hCost))
                {
                    currentNode = openNodes[i];
                }
            }

            //currentNode aus besuchten entfernen und zu fertigen hinzufügen
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            //ist currentNode == targetNode (Ziel erreicht)?
            if (currentNode == target)
            {
                //nächsten Wegpunkt für den Gegnerermitteln
                SetNextWaypoint(start, target);
                //FindPath beenden
                return;
            }

            //zur Vermeidung von Exceptions
            if (currentNode != null)
            {
                //alle Nachbarn der aktuellen Node durchgehen
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {

                        //Koordinaten des aktuellen Nachbarn
                        int indXNeighbour = currentNode.indX + x;
                        int indYNeighbour = currentNode.indY + y;

                        //existiert Nachbar (also ist er in currentRoomNodes vorhanden)?
                        if (!(x == 0 && y == 0) && (indXNeighbour >= 0) && (indYNeighbour >= 0)
                            && (indXNeighbour < currentRoomNodes.GetLength(0))
                            && (indYNeighbour < currentRoomNodes.GetLength(1)))
                        {

                            //der aktuelle Nachbar
                            AStarNode neighbour = currentRoomNodes[indXNeighbour, indYNeighbour];

                            //ist der Nachbar nicht null und noch nicht fertig?
                            if (neighbour != null && !closedNodes.Contains(neighbour))
                            {

                                //Kosten von startNode über currentNode zum Nachbarn berechnen
                                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                                //Kosten sind geringer als bisherig oder Nachbar wurde noch nicht besucht?
                                if (newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                                {

                                    //Kosten ändern
                                    neighbour.gCost = newCostToNeighbour;
                                    neighbour.hCost = GetDistance(neighbour, target);

                                    //Vorgänger des Nachbars neu setzen
                                    neighbour.parentNode = currentNode;

                                    //Nachbar zu besuchten Nodes hinzufügen
                                    if (!openNodes.Contains(neighbour))
                                    {
                                        openNodes.Add(neighbour);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    /// <summary>Setzt den Wegpunkt, zu dem sich der Enemy bewegen soll neu.</summary>
    /// <param name="start">AStarNode, von der der Pfad ausgeht.</param>
    /// <param name="target">AStarNode, zu der der Pfad führt.</param>
    void SetNextWaypoint(AStarNode start, AStarNode target)
    {
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = target;

        //Start noch nicht erreicht (Pfad wird rückwärts durchlaufen)?
        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        //Pfad umdrehen
        path.Reverse();

        // ersten Waypoint im path als aktuellen setzen (vor Exceptions abgesichert)
        try { currentWaypoint = path[0]; } catch (System.ArgumentOutOfRangeException) { }

    }

    /// <summary>
    /// Berechnet die Distanz zwischen zwei AStarNodes mit der Manhattan-Methode.
    /// Die Reihenfolge der Parameter ist egal.
    /// </summary>
    /// <param name="nodeA">Eine AStarNode.</param>
    /// <param name="nodeB">Die andere AStarNode.</param>
    /// <returns>Die Distanz zwischen den beiden übergebenen AStarNodes.</returns>
    int GetDistance(AStarNode nodeA, AStarNode nodeB)
    {
        //Unterschied x-Richtung
        int dX = Mathf.Abs(nodeA.posX - nodeB.posX);
        //Unterschied y-Richtung
        int dY = Mathf.Abs(nodeA.posY - nodeB.posY);

        //Berechnung der Distanz mit Manhattan-Methode
        if (dX > dY)
        {
            return (14 * dY + 10 * (dX - dY));
        }
        return (14 * dX + 10 * (dY - dX));
    }
}
