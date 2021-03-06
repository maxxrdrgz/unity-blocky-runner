﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreen : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    /** 
        This function will check to see if a tile is out of the main cameras
        view
    */
    void Update()
    {
        //this will return 6 planes that form the camera view
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        if(!GeometryUtility.TestPlanesAABB(planes, sr.bounds))
        {
            if(transform.position.x - Camera.main.transform.position.x < 0.0f)
            {
                CheckTile();
            }
        }
    }

    /** 
        This function will check which gameobject this script is currently acting
        upon. Afterwards, it will change the position of the tile to it's last
        known position, plus the offset, using it's last know order in layer.
    */
    void CheckTile()
    {
        if(this.tag == Tags.ROAD)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Road_Tile, new Vector3(1.5f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Road);
        }
        else if (this.tag == Tags.TOP_NEAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Top_Near_Grass, new Vector3(1.2f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Top_Near_Grass);
        }
        else if (this.tag == Tags.TOP_FAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Top_Far_Grass, new Vector3(4.8f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Top_Far_Grass);
        }
        else if (this.tag == Tags.BOTTOM_NEAR_GRASS)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Near_Grass, new Vector3(1.2f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Near_Grass);
        }
        else if (this.tag == Tags.BOTTOM_FAR_LAND_1)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F1, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F1);
        }
        else if (this.tag == Tags.BOTTOM_FAR_LAND_2)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F2, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F2);
        }
        else if (this.tag == Tags.BOTTOM_FAR_LAND_3)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F3, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F3);
        }
        else if (this.tag == Tags.BOTTOM_FAR_LAND_4)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F4, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F4);
        }
        else if (this.tag == Tags.BOTTOM_FAR_LAND_5)
        {
            Change(ref MapGenerator.instance.last_Pos_Of_Bottom_Far_Land_F5, new Vector3(1.6f, 0f, 0f),
                ref MapGenerator.instance.last_Order_Of_Bottom_Far_Land_F5);
        }
    }

    /** 
        This function will change the transform position, order in layer of the
        gameobject this script is attached to.

        @param {ref Vector3} reference to last knowm position of gameobject
        @param {Vector3} coordinates to offset the gameobjects position
        @param {ref int} order in layer
    */
    void Change(ref Vector3 pos, Vector3 offSet, ref int orderLayer)
    {
        transform.position = pos;
        pos += offSet;
        sr.sortingOrder = orderLayer;
        orderLayer++;
    }
}
