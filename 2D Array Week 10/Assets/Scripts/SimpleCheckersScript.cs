using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using static System.Numerics.Vector2;

public class SimpleCheckersScript : MonoBehaviour
{
    public int gridWidth = 8;

    public int gridHeight = 8;

    public int[,] grid; //on the grid we define empty spaces (0), spaces occupied by a red piece (1) and spaces occupied by a black piece (2); 
    // Start is called before the first frame update
    public List<GameObject> spawnedPieces = new List<GameObject>(); //here we keep track of the instantiated gameObjects throughout play, use list because it changes each turn
    
    public GameObject redPiecePrefab;
    public GameObject blackPiecePrefab;

    public bool redTurn = true;
    
    public GameObject choiceMarker;
    private GameObject chosenPiece;

    public TextMeshProUGUI displayText;
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
        #region Column0
        grid[0, 0] = 0; 
        grid[0, 1] = 1; 
        grid[0, 2] = 0;
        grid[0, 3] = 1;
        grid[0, 4] = 0;
        grid[0, 5] = 1;
        grid[0, 6] = 0;
        grid[0, 7] = 1;
        #endregion
        #region Column1
        grid[1, 0] = 1;
        grid[1, 1] = 0;
        grid[1, 2] = 1;
        grid[1, 3] = 0;
        grid[1, 4] = 1;
        grid[1, 5] = 0;
        grid[1, 6] = 1;
        grid[1, 7] = 0;
        #endregion
        #region Column2
        grid[2, 0] = 0;
        grid[2, 1] = 1;
        grid[2, 2] = 0;
        grid[2, 3] = 1;
        grid[2, 4] = 0;
        grid[2, 5] = 1;
        grid[2, 6] = 0;
        grid[2, 7] = 1;
        #endregion
        #region Column3
        grid[3, 0] = 0;
        grid[3, 1] = 0;
        grid[3, 2] = 0;
        grid[3, 3] = 0;
        grid[3, 4] = 0;
        grid[3, 5] = 0;
        grid[3, 6] = 0;
        grid[3, 7] = 0;
        #endregion
        #region Column4
        grid[4, 0] = 0;
        grid[4, 1] = 0;
        grid[4, 2] = 0;
        grid[4, 3] = 0;
        grid[4, 4] = 0;
        grid[4, 5] = 0;
        grid[4, 6] = 0;
        grid[4, 7] = 0;
        #endregion
        #region Column5
        grid[5, 0] = 2;
        grid[5, 1] = 0;
        grid[5, 2] = 2;
        grid[5, 3] = 0;
        grid[5, 4] = 2;
        grid[5, 5] = 0;
        grid[5, 6] = 2;
        grid[5, 7] = 0;
        #endregion
        #region Column6
        grid[6, 0] = 0;
        grid[6, 1] = 2;
        grid[6, 2] = 0;
        grid[6, 3] = 2;
        grid[6, 4] = 0;
        grid[6, 5] = 2;
        grid[6, 6] = 0;
        grid[6, 7] = 2;
        #endregion
        #region Column7
        grid[7, 0] = 2;
        grid[7, 1] = 0;
        grid[7, 2] = 2;
        grid[7, 3] = 0;
        grid[7, 4] = 2;
        grid[7, 5] = 0;
        grid[7, 6] = 2;
        grid[7, 7] = 0;
        #endregion
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
        //reloading the scene
        if (Input.GetKeyDown(KeyCode.Space)) //on SPACE
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the current scene
        }

        if (Input.GetMouseButtonDown(0)) //when left clicked pressed
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //shoot ray
            if (hit.collider.CompareTag("Piece")) //if ray hits object with object tag
            {
                chosenPiece = hit.collider.gameObject; //object hit will become chosen piece
                CheckSpace(chosenPiece); //will call check space function with chosen piece
                Debug.Log("Hit Piece");
            }

            if (hit.collider.CompareTag("Marker")) //if ray hits object with marker tag
            {
                Move(chosenPiece, hit.collider.gameObject); //call move function with chosen piece and the marker that was hit
                Debug.Log("Hit marker");
            }
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
        //changes text to show red's turn
        if (redTurn)
        {
            displayText.text = "Red's Turn";
        }
        //changes text to show black's turn
        else if (!redTurn)
        {
            displayText.text = "Black's Turn";
        }
                 
    }

    public void CheckSpace(GameObject piece) 
    {
        DestroyAllMarkers("Marker"); //calls function with tag marker
        int x = (int)piece.transform.position.x;
        int y = (int)piece.transform.position.y;
        
        if (redTurn && SpaceRed(x,y))
        {
            if (SpaceEmpty(x + 1, y + 1) && y < gridHeight - 1)
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3( x + 1,  y+ 1);
            } 
            if (SpaceEmpty(x + 1, y - 1) && y > gridHeight - 9)
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x + 1, y - 1);
            }
            

            if (SpaceBlack(x + 1, y + 1) && SpaceEmpty(x +2, y + 2))
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x + 2, y + 2);

            }
            if (SpaceBlack(x + 1, y - 1) && SpaceEmpty(x +2, y - 2))
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x + 2, y - 2);

            }
        }
        
        if (!redTurn && SpaceBlack(x,y))
        {
            if (SpaceEmpty(x - 1, y + 1) && y < gridHeight - 1)
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x - 1, y + 1);
            }
            if (SpaceEmpty(x - 1, y - 1) && y > gridHeight - 9)
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x - 1, y - 1);
            }
            
            if (SpaceRed(x - 1, y + 1) && SpaceEmpty(x - 2, y + 2))
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x - 2, y + 2);

            }
            if (SpaceRed(x - 1, y - 1) && SpaceEmpty(x - 2, y - 2))
            {
                var marker = Instantiate(choiceMarker);
                marker.transform.position = new Vector3(x - 2, y - 2);
            }
        }
      
    }

    public void Move(GameObject piece, GameObject marker)
    {
        //SpaceEmpty((int)piece.transform.position.x, (int)piece.transform.position.y) = true (It made me declare another variable.)
        
        bool spaceEmpty = SpaceEmpty((int)piece.transform.position.x, (int)piece.transform.position.y); //creates bool out of Space empty function with the position of the piece chosen
        spaceEmpty = true; //sets bool to true, turning the space empty
        piece.transform.position = marker.transform.position; //sets position of chosen piece to the position of the chosen marker
        bool spaceChange; //creates spaceChange bool
        DestroyAllMarkers("Marker"); //calls function with tag marker
        if (redTurn)
        {
            spaceChange = SpaceRed((int)piece.transform.position.x, (int)piece.transform.position.y); //space change bool will equal red space function with position of piece if red's turn
            spaceChange = true; //will set bool to true
            redTurn = !redTurn; //will change the player turns
        }
        else if (!redTurn)
        {
            spaceChange = SpaceBlack((int)piece.transform.position.x, (int)piece.transform.position.y); //space change bool will equal black space function
            spaceChange = true; 
            redTurn = !redTurn;
        }
       
        UpdateDisplay(); //will call update display
    }

    void DestroyAllMarkers(string tag) 
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag(tag); //makes an array with gameobjects that have the tag marker

        for (int i = 0; i < markers.Length; i++) //cycles through array
        {
            Destroy(markers[i]); //deletes all markers in array
        }
    }
}
    
