using System.Collections;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int Width;
    public int Height;
    public int OffSet;
    private MenuPause _menuPause;
    public GameObject TilePrefab;
    private int _scoreToCount = 0;
    private GameManegerToScore _managerScoreScr;
    public GameObject[] Tiles;
    public GameObject[,] AllTileInBoard;

    void Start()
    {
        _managerScoreScr = FindObjectOfType<GameManegerToScore>();
        AllTileInBoard = new GameObject[Width, Height];
        CreatedBoart();
    }

    private void CreatedBoart()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + OffSet);
                GameObject backgroundTile = Instantiate(TilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                int dotToUse = Random.Range(0, Tiles.Length);
                int maxIteration = 0;
                while (TilesTiLine(i, j, Tiles[dotToUse]) && maxIteration < 100)
                {
                    dotToUse = Random.Range(0, Tiles.Length);
                    maxIteration++;
                }
                maxIteration = 0;
                GameObject dot = Instantiate(Tiles[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Tiles>().Row = j;
                dot.GetComponent<Tiles>().Column = i;
                dot.transform.parent = this.transform;
                AllTileInBoard[i, j] = dot;
            }

        }
    }

    private bool TilesTiLine(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (AllTileInBoard[column - 1, row].tag == piece.tag && AllTileInBoard[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (AllTileInBoard[column, row - 1].tag == piece.tag && AllTileInBoard[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (AllTileInBoard[column, row - 1].tag == piece.tag && AllTileInBoard[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (AllTileInBoard[column - 1, row].tag == piece.tag && AllTileInBoard[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyPlayLines(int column, int row)
    {
        if (AllTileInBoard[column, row].GetComponent<Tiles>().IsMatched)
        {
            Destroy(AllTileInBoard[column, row]);
            AllTileInBoard[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (AllTileInBoard[i, j] != null)
                {
                    DestroyPlayLines(i, j);
                }
            }
        }
        _scoreToCount+= 50;
        _managerScoreScr.ManagerScore(_scoreToCount);
        StartCoroutine(ReSort());
    }

    

    private IEnumerator ReSort()
    {
        int nullCount = 0;
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (AllTileInBoard[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    AllTileInBoard[i, j].GetComponent<Tiles>().Row -= nullCount;
                    AllTileInBoard[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoard());
    }

    private void RefreshBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (AllTileInBoard[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j);
                    int dotToUse = Random.Range(0, Tiles.Length);
                    GameObject piece = Instantiate(Tiles[dotToUse], tempPosition, Quaternion.identity);
                    AllTileInBoard[i, j] = piece;
                    piece.GetComponent<Tiles>().Row = j;
                    piece.GetComponent<Tiles>().Column = i;
                }
            }
        }
    }

    private bool MatchOnBoard()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (AllTileInBoard[i, j] != null)
                {
                    if (AllTileInBoard[i, j].GetComponent<Tiles>().IsMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoard()
    {
        RefreshBoard();
        yield return new WaitForSeconds(.5f);
        while (MatchOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}