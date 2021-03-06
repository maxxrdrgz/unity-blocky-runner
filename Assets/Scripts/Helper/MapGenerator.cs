﻿using System.Collections;
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
        Initialize();
    }

    /** 
        Creates a singleton that only persists in the current Scene
    */
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

    /** 
        Intializes all of the environmental prefabs which include the road, 
        grass, ground and trees.
    */
    void Initialize()
    {
        InitializePlatform(
            roadPrefab, 
            ref last_Pos_Of_Road_Tile, 
            roadPrefab.transform.position,
            start_Road_Tile, 
            road_Holder, 
            ref road_Tiles, 
            ref last_Order_Of_Road, 
            new Vector3(1.5f, 0f, 0f)
        );

        InitializePlatform(
            grass_Prefab, 
            ref last_Pos_Of_Top_Near_Grass, 
            grass_Prefab.transform.position,
            start_Grass_Tile, 
            top_Near_Side_Walk_Holder, 
            ref top_Near_Grass_Tiles, 
            ref last_Order_Of_Top_Near_Grass,
            new Vector3(1.2f, 0f, 0f)
        );

        InitializePlatform(
            groundPrefab_3, 
            ref last_Pos_Of_Top_Far_Grass, 
            groundPrefab_3.transform.position,
            start_Ground3_Tile, 
            top_Far_Side_Walk_Holder, 
            ref top_Far_Grass_Tiles, 
            ref last_Order_Of_Top_Far_Grass,
            new Vector3(4.8f, 0f, 0f)
        );

        InitializePlatform(
            grass_Bottom_Prefab, 
            ref last_Pos_Of_Bottom_Near_Grass, 
            new Vector3(2.0f, grass_Bottom_Prefab.transform.position.y, 0f),
            start_Grass_Tile,
            bottom_Near_Side_walk_Holder, 
            ref bottom_Near_Grass_Tiles, 
            ref last_Order_Of_Bottom_Near_Grass,
            new Vector3(1.2f, 0f, 0f)
        );
        InitializeBottomFarLand();
    }

    /** 
        A lot of this just creates and positions the tiles, using the Pos_For_X_X 
        arrays that are set in the inspector, those numbers determine the order 
        layer for a type of tile.

        This function will intialize a row of gameobjects depending on the prefab
        that is passed in. The number of gameobjects created is dependent upon
        the amountTile integer which comes from the unity inspector. Depending 
        on the tag of the clone, the clone will be positioned apropriately on
        screen. Posistion is also given from the inspector.

        @param {GameObject} prefab to be cloned
        @param {ref Vector3} reference to the last position of the prefab
        @param {Vector3} value of the last position of the prefab
        @param {int} amount of tiles to clone 
        @param {GameObject} parent gameobject that all clones will be a child of
        @param {ref List<GameObject>} reference to list of clones
        @param {ref int} reference to last order in layer of the prefab
        @param {Vector3} the amount of offset the next clone will be spawned at

    */
    void InitializePlatform(GameObject prefab, ref Vector3 last_Pos, Vector3 last_Pos_Of_Tile,
        int amountTile, GameObject holder, ref List<GameObject> list_Tile, ref int last_Order, Vector3 offset)
    {
        int orderInLayer = 0;
        last_Pos = last_Pos_Of_Tile; //storing last pos of tile

        for(int i = 0; i < amountTile; i++)
        {
            GameObject clone = Instantiate(
                                    prefab, 
                                    last_Pos, 
                                    prefab.transform.rotation
                               ) as GameObject;
            clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;

            if(clone.tag == Tags.TOP_NEAR_GRASS)
            {
                SetNearScene(
                    big_Grass_Prefab, 
                    ref clone, 
                    ref orderInLayer, 
                    pos_For_Top_Big_Grass,
                    pos_For_Top_Tree_1, 
                    pos_For_Top_Tree_2, 
                    pos_For_Top_Tree_3
                );
                
            }else if(clone.tag == Tags.BOTTOM_NEAR_GRASS)
            {
                SetNearScene(
                    big_Grass_Bottom_Prefab, 
                    ref clone, 
                    ref orderInLayer, 
                    pos_For_Bottom_Big_Grass,
                    pos_For_Top_Tree_1, 
                    pos_For_Top_Tree_2, 
                    pos_For_Top_Tree_3
                );
            }else if(clone.tag == Tags.BOTTOM_FAR_LAND_2)
            {
                if(orderInLayer == 5)
                {
                    CreateTreeOrGround(
                        big_Tree_Prefab, 
                        ref clone, 
                        new Vector3(-0.57f, -1.34f, 0f)
                    );
                }
            }else if(clone.tag == Tags.TOP_FAR_GRASS)
            {
                CreateGround(ref clone, ref orderInLayer);
            }
            clone.transform.SetParent(holder.transform);
            list_Tile.Add(clone);
            orderInLayer++;
            last_Order = orderInLayer;
            last_Pos += offset;
        } //for loop
    } //InitializePlatform

    /** 
        This function is responsible for creating the scenery. It will instantiate
        and position the grass prefabs at the position of the passed in
        tileClone. The tileClone becomes the parent of the newly instantiated
        clone.

        @param {GameObject} the grass prefab to be cloned
        @param {ref GameObject} reference to a clone of the prefab
        @param {int} order in layer
    */
    void CreateScene(GameObject bigGrassPrefab, ref GameObject tileClone, int orderInLayer)
    {
        //instantiates big grass prefab
        GameObject clone = Instantiate(bigGrassPrefab, tileClone.transform.position, 
            bigGrassPrefab.transform.rotation) as GameObject;
        clone.GetComponent<SpriteRenderer>().sortingOrder = orderInLayer;
        clone.transform.SetParent(tileClone.transform);
        clone.transform.localPosition = new Vector3(-0.183f, 0.106f, 0f);

        CreateTreeOrGround(treePrefab_1, ref clone, new Vector3(0f, 1.52f, 0f));
        //turn off parent tile to show child tile
        tileClone.GetComponent<SpriteRenderer>().enabled = false;

    } // createscene()

    /** 
        Creates a tree or ground gameobject depending on the prefab that is
        passed in at the position of the tileClone. The tileClone is set as the
        parent of the newly created clone and has it's parent's renderer disabled. 

        @param {GameObject} prefab to be cloned
        @param {ref GameObject} clone of the prefab
        @param {Vector3} local position in which newly create clone should be 
                         placed relative to parent
    */
    void CreateTreeOrGround(GameObject prefab, ref GameObject tileClone, Vector3 localPos)
    {
        GameObject clone = Instantiate(
                                prefab, 
                                tileClone.transform.position, 
                                prefab.transform.rotation
                            ) as GameObject;

        SpriteRenderer tileCloneRenderer = tileClone.GetComponent<SpriteRenderer>();
        SpriteRenderer cloneRenderer = clone.GetComponent<SpriteRenderer>();
        cloneRenderer.sortingOrder = tileCloneRenderer.sortingOrder;
        clone.transform.SetParent(tileClone.transform);
        clone.transform.localPosition = localPos;
        if(prefab == groundPrefab_1 || prefab == groundPrefab_2 || prefab == groundPrefab_4)
        {
            tileCloneRenderer.enabled = false;
        }

    } //CreateTreeOrGround

    /** 
        This function will create the ground gameobjects using the orderInLayer
        that can be seen around the road.

        @param {ref GameObject} reference to the clone of the ground prefab
        @param {ref int} reference to the order in layer int
    */
    void CreateGround(ref GameObject clone, ref int orderInLayer)
    {
        for(int i = 0; i < pos_For_Top_Ground_1.Length; i++)
        {
            if(orderInLayer == pos_For_Top_Ground_1[i])
            {
                CreateTreeOrGround(groundPrefab_1, ref clone, Vector3.zero);
                break;
            }
        }
        for (int i = 0; i < pos_For_Top_Ground_2.Length; i++)
        {
            if (orderInLayer == pos_For_Top_Ground_2[i])
            {
                CreateTreeOrGround(groundPrefab_2, ref clone, Vector3.zero);
                break;
            }
        }
        for (int i = 0; i < pos_For_Top_Ground_4.Length; i++)
        {
            if (orderInLayer == pos_For_Top_Ground_4[i])
            {
                CreateTreeOrGround(groundPrefab_4, ref clone, Vector3.zero);
                break;
            }
        }
    } //CreateGround

    /** 
        This function creates big grass gameobjects near the road. They're the
        grass objects that have a slight elevation. This function will also
        determine if a tree should be created on top of the ground. Note that
        the arrays are set in the inspector.

        @param {GameObject} big grass prefab
        @param {ref GameObject} clone of the big grass prefab
        @param {ref int} order in layer
        @param {int[]} array that contains order in layers for the big grass
        @param {int[]} array that contains order in layers for the tree1
        @param {int[]} array that contains order in layers for the tree2
        @param {int[]} array that contains order in layers for the tree3
    */
    void SetNearScene(GameObject bigGrassPrefab, ref GameObject clone, ref int orderInLayer,
        int[] pos_For_BigGrass, int[] pos_For_Tree1, int[] pos_For_Tree2, int[] pos_For_Tree3)
    {
        for(int i = 0; i < pos_For_BigGrass.Length; i++)
        {
            if(orderInLayer == pos_For_BigGrass[i])
            {
                CreateScene(bigGrassPrefab, ref clone, orderInLayer);
                break;
            }
        }

        for(int i = 0; i < pos_For_Tree1.Length; i++)
        {
            if(orderInLayer == pos_For_Tree1[i])
            {
                CreateTreeOrGround(treePrefab_1, ref clone, new Vector3(0f, 1.15f, 0f));
                break;
            }
        }

        for (int i = 0; i < pos_For_Tree2.Length; i++)
        {
            if (orderInLayer == pos_For_Tree2[i])
            {
                CreateTreeOrGround(treePrefab_2, ref clone, new Vector3(0f, 1.15f, 0f));
                break;
            }
        }

        for (int i = 0; i < pos_For_Tree3.Length; i++)
        {
            if (orderInLayer == pos_For_Tree3[i])
            {
                CreateTreeOrGround(treePrefab_3, ref clone, new Vector3(0f, 1.15f, 0f));
                break;
            }
        }
    } // SetNearScene

    /** 
        This function will create all of the flat land found at the bottom of 
        the camera.
    */
    void InitializeBottomFarLand()
    {
        InitializePlatform(land_Prefab_1, ref last_Pos_Of_Bottom_Far_Land_F1, land_Prefab_1.transform.position,
            start_Land_Tile, bottom_Far_Side_Walk_Holder, ref bottom_Far_Land_F1_Tiles,
            ref last_Order_Of_Bottom_Far_Land_F1, new Vector3(1.6f, 0f, 0f));

        InitializePlatform(land_Prefab_2, ref last_Pos_Of_Bottom_Far_Land_F2, land_Prefab_2.transform.position,
            start_Land_Tile-3, bottom_Far_Side_Walk_Holder, ref bottom_Far_Land_F2_Tiles,
            ref last_Order_Of_Bottom_Far_Land_F2, new Vector3(1.6f, 0f, 0f));

        InitializePlatform(land_Prefab_3, ref last_Pos_Of_Bottom_Far_Land_F3, land_Prefab_3.transform.position,
            start_Land_Tile-4, bottom_Far_Side_Walk_Holder, ref bottom_Far_Land_F3_Tiles,
            ref last_Order_Of_Bottom_Far_Land_F3, new Vector3(1.6f, 0f, 0f));

        InitializePlatform(land_Prefab_4, ref last_Pos_Of_Bottom_Far_Land_F4, land_Prefab_4.transform.position,
            start_Land_Tile-7, bottom_Far_Side_Walk_Holder, ref bottom_Far_Land_F4_Tiles,
            ref last_Order_Of_Bottom_Far_Land_F4, new Vector3(1.6f, 0f, 0f));

        InitializePlatform(land_Prefab_5, ref last_Pos_Of_Bottom_Far_Land_F5, land_Prefab_5.transform.position,
            start_Land_Tile-10, bottom_Far_Side_Walk_Holder, ref bottom_Far_Land_F5_Tiles,
            ref last_Order_Of_Bottom_Far_Land_F5, new Vector3(1.6f, 0f, 0f));
    }
}
