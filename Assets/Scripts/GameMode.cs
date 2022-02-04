using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefabe;
    [SerializeField]
    private Candy candyPrefab;
    [SerializeField]
    private int xSize;
    [SerializeField]
    private int ySize;
    [SerializeField]
    private GameObject originPlace;
    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField, Range(0.0f, 5.0f)]
    private float shiftDelay = .3f;
    [SerializeField]
    private Image highImg;
    [SerializeField]
    private Text shuffleTxt;
    [SerializeField, Range(0, 100)]
    public int move = 0;
    [SerializeField, Range(0.0f, 1.0f)]
    private float delaytoShow = 0.5f;
    [SerializeField, Range(0.0f, 3.0f)]
    private float gap = 1.5f;
    [SerializeField]
    private static Candy[,] tiles;
    [SerializeField]
    private Transform topRowFirst;
    [SerializeField]
    private GameObject topBlock;


    public bool IsShifting { get; set; }
    public static int Score { get; private set; }
    public static int Record { get; private set; }
    private float timeToShow = 0.0f;
    private float height;
    private float width;
    private BoxCollider2D boxCollider2;
    private Vector2 pos;
    //private Candy candy;
    private float nextDelayToShift = 0.0f;

    public int Move
    {
        get
        {
            return move;
        }
        set
        {
            move = value;
        }
    }

    
    private void Awake()
    {
        shuffleTxt.enabled = false;
        highImg.enabled = false;
        Record = PlayerPrefs.GetInt("Record");
        width =  tilePrefabe.transform.localScale.x;
        height =  tilePrefabe.transform.localScale.y;
        pos = originPlace.transform.position;
        GridGenerate();
    }
    void Update()
    {
        CreateCandy();
    }

     
    private void CreateCandy()
    {
        Vector2 position = new Vector2();
        position.y = (xSize) * height;

        for (int i = 0; i < xSize; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down , .75f);
            Debug.DrawRay(position, Vector2.down * .75f, Color.red, 5.0f);
            if (!hit)
             {
                Instantiate(candyPrefab, new Vector2(position.x, position.y - .75f), Quaternion.identity).
                    GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
            }
            position.x += height;
        }
    }


    private void GridGenerate()
    {
        gap = 1.1f;

        for (int i = 0; i < xSize; i++)
        {
            pos.x = i * height;
            for (int j = 0; j < ySize; j++)
            {
                pos.y = j * width; 
                 Instantiate(tilePrefabe, pos, Quaternion.identity);
                Candy newCandy = Instantiate(candyPrefab, pos, Quaternion.identity) as Candy;
                newCandy.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
                newCandy.transform.SetParent(this.transform);
            }
        }

    }
 

  


   
    public void AddScore(int score)
    {
        Score += score;
        if (Score > Record)
        {
            Record = Score;
            highImg.enabled = true;
            PlayerPrefs.SetInt("Record", Record);
        }
    }


    public bool CheckForMatch()
    {
        
        for (int x = 0; x < ySize; x++)
        {
            for (int y = 0; y < xSize; y++)
            {
                Sprite tempSprite = tiles[x, y].GetComponent<SpriteRenderer>().sprite;

                if (tempSprite == null)
                {
                    break;
                }
                if ( x + 3 < xSize)
                    {
                        if (tiles[x + 2, y].GetComponent<SpriteRenderer>().sprite == tempSprite
                            && tiles[x + 3, y].GetComponent<SpriteRenderer>().sprite == tempSprite)
                        {
                            return true;
                        }
                        else if (y + 1 < ySize)
                        {
                            if (tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x + 2, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                                }
                            }
                        }
                        else if (y - 1 >= 0)
                        {
                            if (tiles[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {

                                if (tiles[x + 2, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                     return true;

                                }

                            }
                        }

                    }
                    else if ( y + 3 < ySize)
                    {
                        if (tiles[x, y + 2].GetComponent<SpriteRenderer>().sprite == tempSprite &&
                            tiles[x, y + 3].GetComponent<SpriteRenderer>().sprite == tempSprite)
                        {
                            return true;
                        }
                        else if (x + 1 < xSize)
                        {
                            if (tiles[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x + 1, y + 2].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                            }
                            }
                        }
                        else if (x - 1 >= 0)
                        {
                            if (tiles[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x - 1, y + 2].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (x - 3 >= 0)
                    {
                        if (tiles[x - 2, y].GetComponent<SpriteRenderer>().sprite == tempSprite &&
                            tiles[x - 3, y].GetComponent<SpriteRenderer>().sprite == tempSprite)
                        {
                            return true;
                    }
                        else if (y + 1 < ySize)
                        {
                            if (tiles[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x - 2, y + 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                            }
                            }
                        }
                        else if (y - 1 >= 0)
                        {
                            if (tiles[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x - 2, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else if (y - 3 >= 0)
                    {
                        if (tiles[x, y - 2].GetComponent<SpriteRenderer>().sprite == tempSprite &&
                            tiles[x, y - 3].GetComponent<SpriteRenderer>().sprite == tempSprite)
                        {
                            return true;
                    }
                        else if (x - 1 >= 0)
                        {
                            if (tiles[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x - 1, y - 2].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                                }
                            }
                        }
                        else if (x + 1 < xSize)
                        {
                            if (tiles[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == tempSprite)
                            {
                                if (tiles[x - 1, y - 2].GetComponent<SpriteRenderer>().sprite == tempSprite)
                                {
                                    return true;
                                }
                                
                            }
                        }
                    }
                
            }
        }
        return false;
    }

    public void Shuffle()
    {
        if (IsShifting)
        {
            return;
        }

        timeToShow = Time.time + delaytoShow;
        shuffleTxt.enabled = true;
        Debug.Log("Shuffle works");
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                int n = Random.Range(0, xSize-1);
                int m = Random.Range(0, ySize-1);

                Sprite temp = tiles[n, m].GetComponent<SpriteRenderer>().sprite;
                tiles[n, m].GetComponent<SpriteRenderer>().sprite = tiles[x, y].GetComponent<SpriteRenderer>().sprite;
                tiles[x, y].GetComponent<SpriteRenderer>().sprite = temp;
            }
        }
    }
}
