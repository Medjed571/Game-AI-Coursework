using UnityEngine;

/// <summary>
/// works alongisde the SeekToEnemy script to create a list of enemies in order of distance from the agent that said agent running that script will move towards.
/// </summary>

public class EnemySeekPriority : MonoBehaviour
{
    private GameObject enemyParent;     //refers to the "Enemies" game object
    private Vector3[] enemyChildren;    //used to get an array of all its attached childrens position

    public Vector3 chosenEnemy;        //used to get the closest enemy coordinates in ClosestEnemy().

    void Start()
    {
        enemyParent = GameObject.Find("Enemies"); //I wanted to find an alternative solution but GameObject.Find seemed to be the best option I could find to get something by its exact name.
    }

    public Vector3 ChosenEnemy()
    {
        int randNum; 

        enemyChildren = new Vector3[enemyParent.transform.childCount];

        for (int i = 0; i < enemyChildren.Length; i++) 
        {
            enemyChildren[i] = enemyParent.transform.GetChild(i).position; //gets the coordinates of every child
            Debug.Log(enemyChildren[i]);
        }

        randNum = Random.Range(0, enemyChildren.Length - 1); //Get a random number from 0 to (enemies present - 1) to pick which enemy will be pursued/seeked.
        chosenEnemy = enemyChildren[randNum]; //applies the random number to the array, the agent this script is attached to will then go after the enemy this number corelates to.

        return chosenEnemy; //return the closest enemy's coordinates
    }
}
