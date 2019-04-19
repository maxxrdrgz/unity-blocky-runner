using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    public GameObject roadPrefab,
        grass_Prefab,
        groundPrefab_1,
        groundPrefab_2,
        groundPrefab_3,
        groundPrefab_4,
        grass_Bottom_Prefab,
        land_Prefab_1,
        land_Prefab_2,
        land_Prefab_3,
        land_Prefab_4,
        land_Prefab_5,
        big_Grass_Prefab,
        big_Grass_Bottom_Prefab,
        treePrefab_1,
        treePrefab_2,
        treePrefab_3,
        big_Tree_Prefab,
        road_Holder,
        top_Near_Side_Walk_Holder,
        top_Far_Side_Walk_Holder,
        bottom_Near_Side_walk_Holder,
        bottom_Far_Side_Walk_Holder;
    public int start_Road_Tile, //init number of tiles
        start_Grass_Tile,
        start_Ground3_Tile,
        start_Land_Tile;
    public List<GameObject> road_Tiles,
        top_Near_Grass_Tiles,
        top_Far_Grass_Tiles,
        bottom_Near_Grass_Tiles,
        bottom_Far_Land_F1_Tiles,
        bottom_Far_Land_F2_Tiles,
        bottom_Far_Land_F3_Tiles,
        bottom_Far_Land_F4_Tiles,
        bottom_Far_Land_F5_Tiles;
    //pos for ground1 on top from 0 to startGround3Tile
    public int[] pos_For_Top_Ground_1;
    //pos for ground2 on top from 0 to startGround3Tile
    public int[] pos_For_Top_Ground_2;
    //pos for ground4 on top from 0 to startGround3Tile
    public int[] pos_For_Top_Ground_4;
    //pos for big grass with tree on top near grass from 0 to startGrassTile
    public int[] pos_For_Top_Big_Grass;
    //pos for tree1 on top near grass from 0 to startGrassTile
    public int[] pos_For_Top_Tree_1;
    //pos for tree2 on top near grass from 0 to startGrassTile
    public int[] pos_For_Top_Tree_2;
    //pos for tree3 on top near grass from 0 to startGrassTile
    public int[] pos_For_Top_Tree_3;
    //pos for road tile on road from 0 to startRoadTile
    public int pos_For_Road_Tile_1;
    public int pos_For_Road_Tile_2;
    public int pos_For_Road_Tile_3;
    //pos for big grass with tree on bottom near grass from 0 to startGrassTile
    public int[] pos_For_Bottom_Big_Grass;
    //pos for treeX on bottom near grass from 0 to startGrassTile
    public int[] pos_For_Bottom_Tree1;
    public int[] pos_For_Bottom_Tree2;
    public int[] pos_For_Bottom_Tree3;

    [HideInInspector]
    public Vector3 last_Pos_Of_Road_Tile,
        last_Pos_Of_Top_Near_Grass,
        last_Pos_Of_Top_Far_Grass,
        last_Pos_Of_Bottom_Near_Grass,
        last_Pos_Of_Bottom_Far_Land_F1,
        last_Pos_Of_Bottom_Far_Land_F2,
        last_Pos_Of_Bottom_Far_Land_F3,
        last_Pos_Of_Bottom_Far_Land_F4,
        last_Pos_Of_Bottom_Far_Land_F5;

    [HideInInspector]
    public int last_Order_Of_Road,
        last_Order_Of_Top_Near_Grass,
        last_Order_Of_Top_Far_Grass,
        last_Order_Of_Bottom_Near_Grass,
        last_Order_Of_Bottom_Far_Land_F1,
        last_Order_Of_Bottom_Far_Land_F2,
        last_Order_Of_Bottom_Far_Land_F3,
        last_Order_Of_Bottom_Far_Land_F4,
        last_Order_Of_Bottom_Far_Land_F5;

    private void Awake()
    {
        MakeInstance();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        } else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
}
