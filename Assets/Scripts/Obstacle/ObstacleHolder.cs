using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHolder : MonoBehaviour
{
    public GameObject[] childs;
    public float limitAxisX;
    public Vector3 firstPos, secondPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /** 
        Moves the obstacles towards the player and disables them if the
        localPosition.x is less then the limitAxisX which can be found in the
        inspector.
    */
    void Update()
    {
        transform.position += new Vector3(
            -GameplayController.instance.moveSpeed * Time.deltaTime, 
            0f, 
            0f
        );
        if(transform.localPosition.x <= limitAxisX)
        {
            //inform gameplay controller that the obstacle is not active
            GameplayController.instance.obstacles_Is_Active = false;
            gameObject.SetActive(false);
        }
    }

    /** 
        When this gameobject is enabled, this function will enable all child
        gameobjects with a 50% random chance that the children will be spawned
        at either the top or bottom position of the road.
    */
    private void OnEnable()
    {
        for(int i = 0; i< childs.Length; i++)
        {
            childs[i].SetActive(true);
        }

        if(Random.value <= 0.5f)
        {
            transform.localPosition = firstPos;
        }
        else
        {
            transform.localPosition = secondPos;
        }
    }
} //obstacleHolder
