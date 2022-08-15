using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGen : MonoBehaviour
{
    public int sizeX = 10;
    public int sizeY = 10;
    public int mines = 10;
    public Tilemap tiles;
    public TileBase[] tileTextures = new TileBase[12];
    private Vector3 mousePos;
    private Vector3Int mouseTilePos;

    public int[,] tileValues;
    public int[,] tileStates;
    void Start()
    {
        tileValues = new int[sizeY, sizeX];
        tileStates = new int[sizeY, sizeX];
        placeMines();
        initializeTileValues();
        textureTiles();
        
        Debug.Log(printTileStates());
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseTilePos = tiles.WorldToCell(mousePos);

        if    (mouseTilePos.x < sizeX && mouseTilePos.x >= 0
            && mouseTilePos.y < sizeY && mouseTilePos.y >= 0)
        {
            if (Input.GetMouseButtonDown(0) && tileStates[mouseTilePos.y,mouseTilePos.x] == 2 && checkAdjacentStates(mouseTilePos.x, mouseTilePos.y, 1) == tileValues[mouseTilePos.y,mouseTilePos.x]){ 
                revealAdjacent(mouseTilePos.x, mouseTilePos.y, 1);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                reveal(mouseTilePos.x, mouseTilePos.y,1);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //place flag
                if (tileStates[mouseTilePos.y, mouseTilePos.x] == 0)
                {
                    tileStates[mouseTilePos.y, mouseTilePos.x] = 1;
                    tiles.SetTile(new Vector3Int(mouseTilePos.x, mouseTilePos.y, 0), tileTextures[11]);
                } 
                else if (tileStates[mouseTilePos.y, mouseTilePos.x] == 1)
                {
                    tileStates[mouseTilePos.y, mouseTilePos.x] = 0;
                    tiles.SetTile(new Vector3Int(mouseTilePos.x, mouseTilePos.y, 0), tileTextures[9]);
                }
            }

        }
    }

    //algorithm for randomly distributing mines. 
    void placeMines()
    {
        int[] places = new int[sizeX * sizeY];
        for (int i = 0; i < places.Length; i++)
        {
            places[i] = i;
        }
        for (int i = 0; i < mines; i++)
        {
            int holder = places[i];
            int rand = Random.Range(i, (sizeX * sizeY) - 1);
            places[i] = places[rand];
            places[rand] = holder;

            int yPos = places[i] / sizeX;
            int xPos = places[i] % sizeX;
            tileValues[yPos, xPos] = 10;
        }
    }

    void initializeTileValues()
    {
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                if (tileValues[i, j] != 10) tileValues[i, j] = checkAdjacentValues(j, i, 10);
            }
        }
    }
    int checkAdjacentValues(int x, int y, int value)
    {
        int adjacent = 0;
        if (checkValue(x + 1, y + 1, value)) adjacent++;
        if (checkValue(x + 1, y, value)) adjacent++;
        if (checkValue(x + 1, y - 1, value)) adjacent++;
        if (checkValue(x, y - 1, value)) adjacent++;
        if (checkValue(x - 1, y + 1, value)) adjacent++;
        if (checkValue(x - 1, y, value)) adjacent++;
        if (checkValue(x - 1, y - 1, value)) adjacent++;
        if (checkValue(x, y + 1, value)) adjacent++;
        return adjacent;
    }
    bool checkValue(int x, int y, int value)
    {
        if (x < sizeX && x >= 0
            && y < sizeY && y >= 0
            && tileValues[y, x] == value)
        {
            return true;
        }
        return false;
    }

    void textureTiles()
    {
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                tiles.SetTile(new Vector3Int(j, i, 0), tileTextures[9]);
            }
        }
    }


    //flagsetting of 1 means flags will not be revealed, flagsetting of 2 means flags will be revealed (only used when revealing cells adjacent to 0 cells).
    void revealAdjacent(int x, int y, int flagsetting)
    {
        reveal(x + 1, y + 1, flagsetting);
        reveal(x + 1, y, flagsetting);
        reveal(x + 1, y - 1, flagsetting);
        reveal(x, y - 1, flagsetting);
        reveal(x - 1, y - 1, flagsetting);
        reveal(x - 1, y, flagsetting);
        reveal(x - 1, y + 1, flagsetting);
        reveal(x, y + 1, flagsetting);
    }
    void reveal(int x, int y, int flagsetting) 
    {
        if (x < sizeX && x >= 0
            && y < sizeY && y >= 0
            && tileStates[y,x] < flagsetting)
        {
            tileStates[y, x] = 2;
            tiles.SetTile(new Vector3Int(x, y, 0), tileTextures[tileValues[y, x]]);
            if (tileValues[y,x] == 0)
            {
                revealAdjacent(x, y, 2);
            }
        }
    }

    int checkAdjacentStates(int x, int y, int state)
    {
        int adjacent = 0;
        if (checkState(x + 1, y + 1, state)) adjacent++;
        if (checkState(x + 1, y, state)) adjacent++;
        if (checkState(x + 1, y - 1, state)) adjacent++;
        if (checkState(x, y - 1, state)) adjacent++;
        if (checkState(x - 1, y + 1, state)) adjacent++;
        if (checkState(x - 1, y, state)) adjacent++;
        if (checkState(x - 1, y - 1, state)) adjacent++;
        if (checkState(x, y + 1, state)) adjacent++;
        return adjacent;
    }
    bool checkState(int x, int y, int state)
    {
        if (x < sizeX && x >= 0
            && y < sizeY && y >= 0
            && tileStates[y, x] == state)
        {
            return true;
        }
        return false;
    }


    string printTileStates()
    {
        string str = "[";
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                str += ("," + tileValues[i,j]);
            }
            str += "\n";
        }
        return str += "]";
    }
}
