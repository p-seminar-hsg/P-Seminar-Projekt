
/// <summary>
/// Ersteller: Luca Kellermann;
/// Zuletzt geändert am: 7.07.2019
/// 
/// Script, das für die Bewegung der Enemies verantwortlich ist.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour{


    /// <summary>Referenz auf das Enemy-Script eines Enemy.</summary>
    public Enemy enemyScript;


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
    

    // Start is called before the first frame update
    void Start(){

        //Player finden
        player = GameObject.FindGameObjectWithTag("Player");

        //bool-Werte mit false initialisieren
        foundNodes = false;
        
        StartCoroutine(LateStart());
    }

    // Update is called once per frame
    void Update(){

        if(foundNodes && !enemyScript.movementLocked && PlayerInRange() && !PlayerNear()){
            
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

    void FixedUpdate(){
        if(foundNodes && !enemyScript.movementLocked && PlayerInRange()){
            if(PlayerNear() && !PlayerTooNear()){
            //if(!Physics.Linecast(transform.position, player.transform.position)){
                //Enemy in gerader Linie zum Player bewegen
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemyScript.speed * Time.deltaTime);
            } else{
                //Enemy zur nächsten Node im gefundenen Pfad bewegen
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentWaypoint.posX + 0.5f, currentWaypoint.posY + 0.5f, player.transform.position.z), enemyScript.speed * Time.deltaTime);
            }
        }
    }

    /// <summary>Halben Sekunde verzögerte Initialisierungen.</summary>
    private IEnumerator LateStart(){

        //erst mit Pathfinding beginnen, wenn vollständig zur Scene gefadet wurde
        yield return new WaitForSeconds(0.5f);

        //Initialisierungen
        currentRoomScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>().currentRoom.GetComponent<Room>();
        currentRoomNodes = currentRoomScript.nodes;

        //jetzt kann mit Pathfinding begonnen werden
        foundNodes = true;
    }

    /// <summary>Überprüft, ob sich der Player innerhalb der Reichweite des Enemy befindet.</summary>
    private bool PlayerInRange(){
        //aus Renes Enemy1 Script übernommen
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= Mathf.Pow(enemyScript.range, 2);
    }

    /// <summary>Überprüft ob der Enemy weniger als 1 vom Player entfernt ist.</summary>
    private bool PlayerNear(){
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= 1;
    }

    /// <summary>Überprüft ob der Enemy fast direkt im Player ist.</summary>
    private bool PlayerTooNear(){
        return Vector3.SqrMagnitude(player.transform.position - transform.position) <= 0.01f;
    }

    /// <summary>Findet einen möglichst kurzen Pfad mithilfe des A*-Algorythmus.</summary>
    /// <param name="start">AStarNode, von der der Pfad ausgehen soll.</param>
    /// <param name="target">AStarNode, zu der der Pfad führen soll.</param>
    private void FindPath(AStarNode start, AStarNode target){

        //hier werden die Nodes während der Pfadfindung nach ihren jeweiligen Zuständen gespeichert
        List<AStarNode> openNodes = new List<AStarNode>();
        HashSet<AStarNode> closedNodes = new HashSet<AStarNode>();

        openNodes.Add(start);

        //noch Nodes zum überprüfen übrig?
        while(openNodes.Count > 0){
            AStarNode currentNode = openNodes[0];

            //currentNode auf Node mit geringster fCost setzen (bzw. hCost bei gleicher fCost)
            for(int i=1; i<openNodes.Count; i++){
                if(openNodes[i].fCost < currentNode.fCost || (openNodes[i].fCost == currentNode.fCost && openNodes[i].hCost < currentNode.hCost)){
                    currentNode = openNodes[i];
                }
            }

            //currentNode aus besuchten entfernen und zu fertigen hinzufügen
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            //ist currentNode == targetNode (Ziel erreicht)?
            if(currentNode.posX == target.posX && currentNode.posY == target.posY){
                //nächsten Wegpunkt für den Gegnerermitteln
                SetNextWaypoint(start, target);
                //FindPath beenden
                return;
            }

            //alle Nachbarn der aktuellen Node durchgehen
            for(int x=-1; x<=1; x++){
                for(int y=-1; y<=1; y++){

                    //Koordinaten des aktuellen Nachbarn
                    int indXNeighbour = currentNode.indX + x;
                    int indYNeighbour = currentNode.indY + y;

                    //existiert Nachbar (also ist er in currentRoomNodes vorhanden)?
                    if(!(x==0 && y==0) && ( indXNeighbour >= 0 ) && ( indYNeighbour >= 0 )
                        && ( indXNeighbour < currentRoomNodes.GetLength(0) )
                        && ( indYNeighbour < currentRoomNodes.GetLength(1) ) ){

                            //der aktuelle Nachbar
                            AStarNode neighbour = currentRoomNodes[indXNeighbour, indYNeighbour];

                            //ist der Nachbar nicht null und noch nicht fertig?
                            if(neighbour != null && !closedNodes.Contains(neighbour)){

                                //Kosten von startNode über currentNode zum Nachbarn berechnen
                                int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                                //Kosten sind geringer als bisherig oder Nachbar wurde noch nicht besucht?
                                if(newCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour)){

                                    //Kosten ändern
                                    neighbour.gCost = newCostToNeighbour;
                                    neighbour.hCost = GetDistance(neighbour, target);

                                    //Vorgänger des Nachbars neu setzen
                                    neighbour.parentNode = currentNode;

                                    //Nachbar zu besuchten Nodes hinzufügen
                                    if(!openNodes.Contains(neighbour)){
                                        openNodes.Add(neighbour);
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
    void SetNextWaypoint(AStarNode start, AStarNode target){
        List<AStarNode> path = new List<AStarNode>();
        AStarNode currentNode = target;

        //Start noch nicht erreicht (Pfad wird rückwärts durchlaufen)?
        while(currentNode != start){
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        //Pfad umdrehen
        path.Reverse();

        // ersten Waypoint im path als aktuellen setzen
        currentWaypoint = path[0];
    }

    /// <summary>
    /// Berechnet die Distanz zwischen zwei AStarNodes mit der Manhattan-Methode.
    /// Die Reihenfolge der Parameter ist egal.
    /// </summary>
    /// <param name="nodeA">Eine AStarNode.</param>
    /// <param name="nodeB">Die andere AStarNode.</param>
    /// <returns>Die Distanz zwischen den beiden übergebenen AStarNodes.</returns>
    int GetDistance(AStarNode nodeA, AStarNode nodeB){
        //Unterschied x-Richtung
        int dX = Mathf.Abs(nodeA.posX - nodeB.posX);
        //Unterschied y-Richtung
        int dY = Mathf.Abs(nodeA.posY - nodeB.posY);

        //Berechnung der Distanz mit Manhattan-Methode
        if(dX > dY){
            return (14 * dY + 10 * (dX-dY));
        }
        return (14 * dX + 10 * (dY-dX));
    }
}
