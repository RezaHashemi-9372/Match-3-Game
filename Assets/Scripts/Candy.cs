using System.Collections.Generic;
using UnityEngine;
public class Candy : MonoBehaviour
{
    [Header("Lerp in the Candy")]
    [SerializeField, Range(0.0f, 50.0f)]
    private float lerpSideAmount = 0.15f;
    [SerializeField, Range(0.0f, 150.0f)]
    private float downLerpAmount = 85.0f;
    [SerializeField, Range(0.0f, 50.0f)]
    private float vanishLerpAmo = 5.0f;
    private SpriteRenderer thisSpriteRenderer;
    private bool isdragged = false;
    private bool isMatched = false;
    private bool matchFound = false;
    private bool downSetted = false;
    public static bool isChecking = false;
    private bool isSeted = false;
    private float width = 0;
    private float height = 0;
    private float candyHeight = 0.0f;

    private Vector3 originalPos;
    private Vector3 nextPostion = Vector3.zero;
    private Vector3 pos;
    private Vector3 reachDistance;
    private Vector2 tempDirection = Vector2.zero;
    private GameObject hitRenderer;
    private Vector3 originalScale;
    private Vector3 downNextPosition = new Vector3();
    private GameObject tempObject;
    private Vector3 hitObjPosition;

    private Goal goal;
    private GameMode gameMode;
    private GUIManager gUIManager;
    private float nextShiftCandy = 0.0f;

    private static SpriteRenderer previousselected;
    private void Awake()
    {
        gUIManager = FindObjectOfType<GUIManager>();
        gameMode = FindObjectOfType<GameMode>();
        goal = FindObjectOfType<Goal>();
        thisSpriteRenderer = this.GetComponent<SpriteRenderer>();
        width = this.GetComponent<SpriteRenderer>().size.x / 4;
        height = this.GetComponent<SpriteRenderer>().size.y / 4;
        candyHeight = .75f;
        Physics2D.queriesStartInColliders = false;
    }
    void Start()
    {
        originalScale = this.GetComponent<SpriteRenderer>().transform.localScale;
        originalPos = this.transform.position;
        this.ClearAllMatch();
    }

    void Update()
    {
        if (downSetted)
        {
            isChecking = true;
            this.transform.position = Vector3.Lerp(this.transform.position,
                downNextPosition, downLerpAmount * Time.deltaTime);
            if (this.transform.position == downNextPosition)
            {
                isChecking = false;
                downSetted = false;
                
            }
        }

        if (isSeted)
        {
            thisSpriteRenderer.transform.position = Vector3.Lerp(thisSpriteRenderer.transform.position,
                nextPostion, lerpSideAmount * Time.deltaTime);

            if (thisSpriteRenderer.transform.position == reachDistance )
            {
                if (this.ClearAllMatch() || hitRenderer.GetComponent<Candy>().ClearAllMatch())
                {
                    isSeted = false;
                    hitRenderer.GetComponent<Candy>().isSeted = false;
                    isChecking = false;
                    gameMode.move -= 1;
                    if (gameMode.move <= 0)
                    {
                        gUIManager.GameOver();
                    }
                }
                else if (!hitRenderer.GetComponent<Candy>().ClearAllMatch() || !this.ClearAllMatch())
                {
                    SetPosition( this.gameObject, hitRenderer);

                    isChecking = true;
                }
                hitRenderer = null;
            }
        }
        if (isMatched)
        {
            isChecking = true;
            this.GetComponent<SpriteRenderer>().transform.localScale =
                Vector3.Lerp(this.GetComponent<SpriteRenderer>().transform.localScale,
                new Vector3(.1f, .1f, .1f),
                vanishLerpAmo * Time.deltaTime);
            if (this.GetComponent<SpriteRenderer>().transform.localScale.x <= .11f)
            {
                isMatched = false;
                goal.EvaluationOfGoal(this.GetComponent<SpriteRenderer>().sprite);
                isChecking = false;
                gameMode.AddScore(20);
                Destroy(this.gameObject);
            }
        }
        
        if (isdragged)
        {
            pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
   
            if (pos.y > originalPos.y + height / 2)
            {
                tempDirection = Vector2.up;
            }
            else if (pos.y < originalPos.y - height /2)
            {
                tempDirection = Vector2.down;
            }
            else if (pos.x > originalPos.x + width / 2)
            {
                tempDirection = Vector2.right;
            }
            else if (pos.x < originalPos.x - width /2)
            {
                tempDirection = Vector2.left;
            }
        }
        CheckDownCandy();
    }
    
    private void OnMouseDown()
    {
        if (previousselected != null)
        {
            previousselected = null;
        }
        else
        {
            isdragged = true;
            previousselected = thisSpriteRenderer;
            isChecking = true;

        }
    }
    
    private void OnMouseUp()
    {
        if (tempDirection == Vector2.zero)
        {
            return;
        }
        isdragged = false;
        hitRenderer = Raycast(tempDirection);
        reachDistance = hitRenderer.GetComponent<SpriteRenderer>().transform.position;
        SetPosition(this.gameObject, hitRenderer);

    }


    private void SetPosition(GameObject first , GameObject second)
    {
        this.ChangePos(second.transform.position);
        second.GetComponent<Candy>().ChangePos(first.transform.position);
    }

    private GameObject Raycast(Vector2 dir)
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(this.transform.position, dir);
       
        if (hit && hit.collider.GetComponent<Candy>())
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private void ChangePos(Vector3 pos)
    {
        isSeted = true;
        nextPostion = new Vector3(pos.x, pos.y, pos.z);
    }

    private void CheckDownCandy()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(this.transform.position, Vector2.down * .5f, .5f);

        if (!hit && !isChecking)
        {
            downNextPosition.y = this.transform.position.y - .75f;
            downNextPosition.x = this.transform.position.x;
            downSetted = true;
        }
        

    }
    private List<GameObject> FindMatch(Vector2 castdir)
    {

        List<GameObject> matchCandy = new List<GameObject>();
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, castdir, .5f);

            while (hit && hit.collider.GetComponent<SpriteRenderer>().sprite ==
                this.GetComponent<SpriteRenderer>().sprite)
            {

                matchCandy.Add(hit.collider.gameObject);
                hit = Physics2D.Raycast(hit.collider.transform.position, castdir, .5f);
            }
        
        return matchCandy;
    }
    private void ClearMatch(Vector2[] paths)
    {
        List<GameObject> matchCandy = new List<GameObject>();
        matchCandy.Add(this.gameObject);
        for (int i = 0; i < paths.Length; i++)
        {
            matchCandy.AddRange(FindMatch(paths[i]));
        }

        if (matchCandy.Count >= 3)
        {
            isChecking = true;
            for (int i = 0; i < matchCandy.Count; i++)
            {
                matchCandy[i].GetComponent<Candy>().isMatched = true;
            }

            matchFound = true;
        }

    }

    public bool ClearAllMatch()
    {

        ClearMatch(new Vector2[] { Vector2.up, Vector2.down });
        ClearMatch(new Vector2[] { Vector2.left, Vector2.right });

        if (matchFound)
        {
            matchFound = false;
            return true;
        }
        return false;
    }
}
