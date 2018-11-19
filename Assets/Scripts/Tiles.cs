using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public int Column;
    public int Row;
    public int PositionWidth;
    public int RexColumn;
    public int RezRow;
    public int PositionHight;
    private GameBoard _board;
    public bool IsMatched = false;
    private GameObject _otherTiles;
    private GameManegerToScore _gameManeger;
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private Vector2 _tempPosition;
    public float SwipeAngle = 0;
    public float SwipeResist = 1f;

    void Start()
    {
        _gameManeger = FindObjectOfType<GameManegerToScore>();
        _board = FindObjectOfType<GameBoard>();
        PositionWidth = (int)transform.position.x;
        PositionHight = (int)transform.position.y;
        Row = PositionHight;
        Column = PositionWidth;
        RexColumn = Column;
        RezRow = Row;
    }


    void Update()
    {
        if (_gameManeger.IsOwer == false)
        {
            FindLines();
            if (IsMatched)
            {
                SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
                Color currentColor = mySprite.color;
                mySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
            }
            PositionWidth = Column;
            PositionHight = Row;
            if (Mathf.Abs(PositionWidth - transform.position.x) > .1)
            {
                _tempPosition = new Vector2(PositionWidth, transform.position.y);
                transform.position = Vector2.Lerp(transform.position, _tempPosition, .6f);
                if (_board.AllTileInBoard[Column, Row] != this.gameObject)
                {
                    _board.AllTileInBoard[Column, Row] = this.gameObject;
                }
            }
            else
            {
                _tempPosition = new Vector2(PositionWidth, transform.position.y);
                transform.position = _tempPosition;
            }
            if (Mathf.Abs(PositionHight - transform.position.y) > .1)
            {
                _tempPosition = new Vector2(transform.position.x, PositionHight);
                transform.position = Vector2.Lerp(transform.position, _tempPosition, .6f);
                _board.AllTileInBoard[Column, Row] = this.gameObject;
                if (_board.AllTileInBoard[Column, Row] != this.gameObject)
                {
                    _board.AllTileInBoard[Column, Row] = this.gameObject;
                }
            }
            else
            {
                _tempPosition = new Vector2(transform.position.x, PositionHight);
                transform.position = _tempPosition;
            }
        }
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        if(_otherTiles != null)
        {
            if(!IsMatched && !_otherTiles.GetComponent<Tiles>().IsMatched)
            {
                _otherTiles.GetComponent<Tiles>().Row = Row;
                _otherTiles.GetComponent<Tiles>().Column = Column;
                Row = RezRow;
                Column = RexColumn;
            }
            else
            {
                _board.DestroyMatches();
            }
            _otherTiles = null;
        }
    }

    private void OnMouseDown()
    {
        _startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        _endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngleToMove();
    }

    void CalculateAngleToMove()
    {
        if(Mathf.Abs(_endTouchPosition.y - _startTouchPosition.y )> SwipeResist || Mathf.Abs(_endTouchPosition.x - _startTouchPosition.x) > SwipeResist)
        {
           SwipeAngle = Mathf.Atan2(_endTouchPosition.y - _startTouchPosition.y, _endTouchPosition.x - _startTouchPosition.x) * 180 / Mathf.PI;
            MoveToAngle();
        }
    }

    void MoveToAngle()
    {
        if (SwipeAngle > -45 && SwipeAngle <= 45 && Column < _board.Width - 1)
        {
            _otherTiles = _board.AllTileInBoard[Column + 1, Row];
            _otherTiles.GetComponent<Tiles>().Column -= 1;
            Column += 1;
        }
        else if (SwipeAngle > 45 && SwipeAngle <= 135 && Row < _board.Height - 1)
        {
            _otherTiles = _board.AllTileInBoard[Column, Row + 1];
            _otherTiles.GetComponent<Tiles>().Row -= 1;
            Row += 1;
        }
        else if ((SwipeAngle > 135 || SwipeAngle <= -135) && Column > 0)
        {
            _otherTiles = _board.AllTileInBoard[Column - 1, Row];
            _otherTiles.GetComponent<Tiles>().Column += 1;
            Column -= 1;
        }
        else if (SwipeAngle < -45 && SwipeAngle >= -135 && Row > 0)
        {
            _otherTiles = _board.AllTileInBoard[Column, Row - 1];
            _otherTiles.GetComponent<Tiles>().Row += 1;
            Row -= 1;
        }
        StartCoroutine(CheckMove());
    }

    void FindLines()
    {
        if (Column > 0 && Column < _board.Width - 1)
        {
            GameObject leftDot1 = _board.AllTileInBoard[Column - 1, Row];
            GameObject rightDot1 = _board.AllTileInBoard[Column + 1, Row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1 != null && rightDot1 != null)
                {
                    if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                    {
                        leftDot1.GetComponent<Tiles>().IsMatched = true;
                        rightDot1.GetComponent<Tiles>().IsMatched = true;
                        IsMatched = true;
                    }
                }
            }
        }
        if (Row > 0 && Row < _board.Height - 1)
        {
            GameObject upDot1 = _board.AllTileInBoard[Column, Row + 1];
            GameObject downDot1 = _board.AllTileInBoard[Column, Row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1 != null && downDot1 != null)
                {
                    if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                    {
                        upDot1.GetComponent<Tiles>().IsMatched = true;
                        downDot1.GetComponent<Tiles>().IsMatched = true;
                        IsMatched = true;
                    }
                }
            }
        }
    }
}