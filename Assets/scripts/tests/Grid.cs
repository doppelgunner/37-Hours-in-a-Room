using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    [SerializeField]
    private Sprite cellSprite;
    [SerializeField]
    private Vector2 cellSize;
    [SerializeField]
    private Vector2 cellScale;

    //Delete on build
    [SerializeField]
    private bool gizmoOn = true;

    void Start() {
        InitCells();
    }

    void InitCells() {
        GameObject cellObject = new GameObject();
        cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;
        cellSize = cellSprite.bounds.size;

        Vector2 newCellSize = new Vector2(gridSize.x / (float) cols, gridSize.y / (float) rows);

        cellScale.x = newCellSize.x / cellSize.y;
        cellScale.y = newCellSize.y / cellSize.y;
        cellSize = newCellSize;

        cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);
        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;

        for(int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {

                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);
                GameObject cO = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;
                cO.transform.parent = transform;
            }
        }

        Destroy(cellObject);
    }

    void OnDrawGizmos() {
        if (gizmoOn) {
            Gizmos.DrawWireCube(transform.position, gridSize);
        }
    }

}
