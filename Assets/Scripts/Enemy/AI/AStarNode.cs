
/// <summary>
/// Ersteller: Luca Kellermann;
/// Zuletzt geändert am: 7.07.2019
/// 
/// Klasse, die für die Pathfinding-Funktion des EnemyAI-Scripts benötigt wird.
/// Jede AStarNode repräsentiert eine Tile der Ground-Tilemap eines Rooms.
/// </summary>

public class AStarNode{

    /// <summary>Die x-Koordinate der Position dieser AStarNode in der Scene.</summary>
    public int posX;
    
    /// <summary>Die y-Koordinate der Position dieser AStarNode in der Scene.</summary>
    public int posY;

    /// <summary>Die x-Koordinate der Position dieser AStarNode relativ zum benutzten Teil der Tilemap.</summary>
    public int indX;

    /// <summary>Die y-Koordinate der Position dieser AStarNode relativ zum benutzten Teil der Tilemap.</summary>
    public int indY;

    /// <summary>Die Kosten von der Startnode zu dieser AStarNode.</summary>
    public int gCost;

    /// <summary>Die geschätzten Kosten von dieser AStarNode zur Targetnode.</summary>
    public int hCost;

    /// <summary>Die Summe aus gCost und hCost.</summary>
    public int fCost { get { return gCost + hCost; } }

    /// <summary>Die Vorgängernode dieser AStarNode im aktuellen Pfad von der Startnode zu dieser Node.</summary>
    public AStarNode parentNode;

    /// <param name="posX">Die x-Koordinate der Position dieser AStarNode in der Scene.</param>
    /// <param name="posY">Die y-Koordinate der Position dieser AStarNode in der Scene.</param>
    /// <param name="indX">Die x-Koordinate der Position dieser AStarNode relativ zum benutzten Teil der Tilemap.</param>
    /// <param name="indY">Die y-Koordinate der Position dieser AStarNode relativ zum benutzten Teil der Tilemap.</param>
    /// <summary>Erstellt eine neue AStarNode mit den angegebenen Koordinaten.</summary>
    public AStarNode(int posX, int posY, int indX, int indY){
        this.posX = posX;
        this.posY = posY;
        this.indX = indX;
        this.indY = indY;
    }
}
