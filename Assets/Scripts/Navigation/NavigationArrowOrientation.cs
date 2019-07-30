using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ersteller: Luca Kellermann
/// Zuletzt geändert am: 29.07.2019
/// Dieses Script regelt die Drehung des NavigationArrows.
/// </summary>
public class NavigationArrowOrientation : MonoBehaviour
{

    /// <summary>
    /// Der benötigte Abstandsunterschied zwischen Gegnern, 
    /// damit der Pfeil einen neuen, näheren Gegner anvisiert.
    /// </summary>
    public float requiredDistanceDifference;

    /// <summary>
    /// Die Farbe des Pfeils, wenn er auf einen Gegner zeigt.
    /// </summary>    
    public Color colorEnemy;

    /// <summary>
    /// Die Farbe des Pfeils, wenn er auf den Teleporter zeigt.
    /// </summary>   
    public Color colorTeleporter;

    private GameObject player;
    private GameObject currentNearestEnemy;
    private Image arrow;

    // Start is called before the first frame update
    void Start(){
        //Player und Pfeil finden
        player = GameObject.FindGameObjectWithTag("Player");
        arrow = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        //Gibt es Gegner?
        if(MapManager.instance.currentRoomScript.GetEnemiesAlive() > 0){

            //zur Vermeidung von Exceptions
            if(MapManager.instance.currentRoomScript.enemies.Length > 0){

                //Farbe des Pfeils anpassen
                arrow.color = colorEnemy;

                //hier wird der geringste Abstand eines Gegners während diesem Update() gespeichert
                float minDistance = -1;

                //der Gegner mit dem kleinsten Abstand
                GameObject nearestEnemy = null;

                //bei allen Gegnern den Abstand überprüfen
                foreach(GameObject enemy in MapManager.instance.currentRoomScript.enemies){

                    //zur Vermeidung von Exceptions
                    if(enemy != null){
                        
                        //der Vektor vom Player zum aktuellen Gegner
                        Vector3 playerToEnemy = enemy.transform.position - player.transform.position;

                        float distance = playerToEnemy.magnitude;

                        //falls es der erste Gegner ist: minDistance = -1
                        if((minDistance == -1) || (distance < minDistance)){
                            minDistance = distance;
                            nearestEnemy = enemy;
                        }
                    }
                }

                //                                   ist die neuste kürzeste Distanz auch mit requiredDistanceDifference kleiner als der bisher geringste Abstand?
                if( (currentNearestEnemy == null) || (minDistance + requiredDistanceDifference) < (currentNearestEnemy.transform.position - player.transform.position).magnitude){
                    currentNearestEnemy = nearestEnemy;
                }
                
                //der Vektor vom Player zum nähesten Gegner
                Vector3 playerToNearestEnemy = currentNearestEnemy.transform.position - player.transform.position;

                //             gibt Winkel des Vektors im Bogenmaß       =>       Umwandlung in Gradmaß ;   - 90 (Grad) für den richtigen Winkel
                float angle = (Mathf.Atan2(playerToNearestEnemy.y, playerToNearestEnemy.x) * Mathf.Rad2Deg) - 90;

                //dreht den Pfeil mit dem oben berechneten Winkel um die z-Achse
                transform.rotation = Quaternion.Euler(0, 0, angle);

            }

        } else{

            //Farbe des Pfeils anpassen
            arrow.color = colorTeleporter;

            //der Vektor vom Player zum Teleporter
            Vector3 playerToTeleporter = MapManager.instance.currentRoomScript.GetTeleporterPosition() - player.transform.position;

            //             gibt Winkel des Vektors im Bogenmaß     =>     Umwandlung in Gradmaß ;   - 90 (Grad) für den richtigen Winkel
            float angle = (Mathf.Atan2(playerToTeleporter.y, playerToTeleporter.x) * Mathf.Rad2Deg) - 90;

            //dreht den Pfeil mit dem oben berechneten Winkel um die z-Achse
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
