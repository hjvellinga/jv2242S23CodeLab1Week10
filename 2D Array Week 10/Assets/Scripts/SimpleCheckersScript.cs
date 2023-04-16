using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class SimpleCheckersScript : MonoBehaviour
{
    public int gridWidth = 8;

    public int gridHeight = 8;

    public int[,] grid; //on the grid we define empty spaces (0), spaces occupied by a red piece (1) and spaces occupied by a black piece (2); 
    // Start is called before the first frame update
    public List<GameObject> spawnedPieces = new List<GameObject>(); //here we keep track of the instantiated gameObjects throughout play, use list because it changes each turn

    public GameObject redPiecePrefab;
    public GameObject blackPiecePrefab; 
    void Start()
    //let's instantiate and initialize the grid
    {
        grid = new int[gridWidth, gridHeight]; //define parameters of grid

        for (var x = 0; x < gridWidth; x++) //for loop through columns (vertical)
        {
            for (var y = 0; y < gridHeight; y++) //for loops through rows (horizontal)
            {
                grid[x, y] = 0; //the grid is empty
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //reloading the scene
        if (Input.GetKeyDown(KeyCode.Space)) //on SPACE
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the current scene
        }
        
    }
    
    // three functions for defining the values of EMPTY, RED and BLACK occupied spaces on the grid 
    public bool SpaceEmpty(int x, int y) //EMPTY space == 0;
    {
        return grid[x, y] == 0;
    }

    public bool SpaceRed(int x, int y) //RED space == 1; 
    {
        return grid[x, y] == 1; 
    }

    public bool SpaceBlack(int x, int y) //BLACK space ==2; 
    {
        return grid[x, y] == 2; 
    }

    public void UpdateDisplay() //after each turn the grid[] must be updated with the new x,y values
    {
        foreach (var piece in
                 spawnedPieces) //for loop referring to all the gameObjects called "piece" in spawnedPieces list
        {
            Destroy(piece); //destroy all the pieces in the scene
        }

        spawnedPieces.Clear(); //clear the list 


        //then spawn the correct pieces in the grid; 

        for (var x = 0; x < gridWidth; x++) //loop through the grid columns
        {
            for (var y = 0; y < gridHeight; y++) //loop through grid lines; 
            {
                if (SpaceRed(x, y)) //if the value is 1 we spawn the redPiece gameObject;
                {
                    var redPiece = Instantiate(redPiecePrefab); //instantiate the gameObject by making a copy of the redPiecePrefab;
                    redPiece.transform.position = new Vector3(x, y); //set the location of the redPiece to the current x,y position;
                    spawnedPieces.Add(redPiece); //add the redPiece gameObject to our spawnedPieces list; 
                }

                if (SpaceBlack(x, y)) //if the value of x,y is 2 we spawn the blackPiece gameObject;
                {
                    var blackPiece = Instantiate(blackPiecePrefab); //spawn blackPiece gameObject by making a copy of the blackPiecePrefab;
                    blackPiece.transform.position = new Vector3(x, y); //location of blackPiece gameObject is current x,y position
                    spawnedPieces.Add(blackPiece); //add the new blackPiece gameObject to our spawnedPieces list; 

                }
            }
        }
                 
    }
}
    
