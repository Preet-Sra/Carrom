using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Slider Player1strikerSlider,Player2strikerSlider;
    [SerializeField] private bool canAdjust = true;
    private Rigidbody2D rb;
    private GameObject MousepointA,MousePointB,Arrow,Circle;
    private float currentDistance, safePlace, ShootPower;
    Vector3 shootDirection;
    float maxDistance=2f;
    Vector3 Startingpos;
    private bool shoot = false;
    private Collider2D col;

    public bool Player1Turn=true;

    public Vector3 Player1Startpos,Player2StartPos;
    public bool TookToken = false;
    public GameObject ErrorMessage;
    private bool AlreadySwitched = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        
        ChangeStrikerPosition(0);
        Startingpos = transform.position;
        MousepointA = GameObject.FindGameObjectWithTag("PointA");
        MousePointB = GameObject.FindGameObjectWithTag("PointB");
        Arrow = GameObject.FindGameObjectWithTag("Arrow");
        Circle = GameObject.FindGameObjectWithTag("Circle");
      
        
    }

    public void ChangeStrikerPosition(float value)
    {
        if (canAdjust)
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
            col.isTrigger = true;
        }
    }

    private void OnMouseDrag()
    {
        MousepointA.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,10f));
        currentDistance = Vector3.Distance(MousepointA.transform.position, transform.position);
        
        if (currentDistance <= maxDistance)
        {
            safePlace = currentDistance;
        }
        else
        {
            safePlace = maxDistance;
        }
        UpdateArrowAndCircle();

        ShootPower = Mathf.Abs(safePlace) * 10;
        Vector3 dirxy = MousepointA.transform.position - transform.position;
        float difference = dirxy.magnitude;
        MousePointB.transform.position = transform.position + ((dirxy / difference) * currentDistance * -1);
        MousePointB.transform.position = new Vector3(MousePointB.transform.position.x, MousePointB.transform.position.y, -0.8f);
        float radius = 0.5f; //radius of *black circle*
        Vector3 centerPosition = transform.localPosition; //center of *black circle*
        float distance = Vector3.Distance(MousePointB.transform.position, centerPosition); //distance from ~green object~ to *black circle*

        if (distance > radius) //If the distance is less than the radius, it is already within the circle.
        {
            Vector3 fromOriginToObject = MousePointB.transform.position - centerPosition; //~GreenPosition~ - *BlackCenter*
            fromOriginToObject *= radius / distance; //Multiply by radius //Divide by Distance
            MousePointB.transform.position= centerPosition + fromOriginToObject; //*BlackCenter* + all that Math
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(distance);
        }
        shootDirection = Vector3.Normalize(MousepointA.transform.position - transform.position);
        
    }

    private void OnMouseUp()
    {
        Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Circle.GetComponent<SpriteRenderer>().enabled = false;
        col.isTrigger = false;
        Vector3 push = shootDirection * ShootPower * -1;
        rb.AddForce(push, ForceMode2D.Impulse);
        shoot = true;
        

    }


    public void SwitchTurns()
    {
        if (Player1Turn)
        {

            Player1Turn = false;
        }
        else
        {
            Player1Turn = true;
        }
        AlreadySwitched = true;
    }

    void UpdateArrowAndCircle()
    {
        Arrow.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        Circle.GetComponent<SpriteRenderer>().enabled = true;
        TookToken = false;
        if (currentDistance < -maxDistance)
        {
            Arrow.transform.position = new Vector3((2 * transform.position.x) - MousepointA.transform.position.x,
                (2 * transform.position.y) - MousepointA.transform.position.y, 10f);
        }
        else
        {
            Vector3 dirxy = MousepointA.transform.position - transform.position;
            float difference = dirxy.magnitude;
            Arrow.transform.position = transform.position + ((dirxy / difference) * currentDistance * -1);
            Arrow.transform.position = new Vector3(transform.position.x, transform.position.y, -0.8f);
        }

        Circle.transform.position = transform.position + new Vector3(0, 0, 0.04f);
        Vector3 dir = MousepointA.transform.position - transform.position;
        float rot;
        if (Vector3.Angle(dir, transform.forward) > 90)
        {
            rot = Vector3.Angle(dir, transform.right);
        }
        else
        {
            rot = Vector3.Angle(dir, transform.right) * -1;
        }

        if (Vector3.Angle(dir, transform.up) < 90)
        {
            rot = Vector3.Angle(dir, transform.right);
        }
        else
        {
            rot = Vector3.Angle(dir, transform.right) * -1;
        }
        
        Vector2 direction = MousepointA.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Arrow.transform.rotation = Quaternion.Slerp(Arrow.transform.rotation, rotation, 6 * Time.deltaTime);


        float ScaleX = Mathf.Log(1 + safePlace / 2, 2);
        float ScaleY = Mathf.Log(1 + safePlace / 2, 2);
        Arrow.transform.localScale = new Vector3((1 + ScaleX) / 2, (1 + ScaleY) / 2, 0.001f);
        Circle.transform.localScale = new Vector3((1 + ScaleX) / 15, (1 + ScaleY) / 15, 0.001f);
    }


  

    private void FixedUpdate()
    {
          if (Player1Turn)
        {
            Player1strikerSlider.onValueChanged.AddListener(ChangeStrikerPosition);
        }
        else
        {
            Player2strikerSlider.onValueChanged.AddListener(ChangeStrikerPosition);
        }
        if (shoot)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (rb.velocity.sqrMagnitude <= 0.001f)
            {
                if (!TookToken)
                {
                    if (!AlreadySwitched)
                    {
                        SwitchTurns();
                    }
                }
                Invoke("ResetStriker", 2f);
        
                col.enabled = false;
            }
            Player1strikerSlider.transform.gameObject.SetActive(false);
         //   Player1strikerSlider.value = 0;
            Player2strikerSlider.transform.gameObject.SetActive(false);
           // Player2strikerSlider.value = 0;
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);

            if (Player1Turn)
            {
                Player1strikerSlider.transform.gameObject.SetActive(true);
               
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                Player2strikerSlider.transform.gameObject.SetActive(true);
               
                transform.eulerAngles = new Vector3(0,0,180);
            }
            
        }
    }

    public void ResetStriker()
    {
      



        if (Player1Turn)
        {
            Startingpos = Player1Startpos;
            Startingpos.x = Player1strikerSlider.value;
        }
        else
        {
            Startingpos = Player2StartPos;
            Startingpos.x = Player2strikerSlider.value;
        }
        transform.position = Vector3.MoveTowards(transform.position,Startingpos, 6 * Time.deltaTime);
        col.isTrigger = true;
        if (Vector3.Distance(transform.position, Startingpos) <= 0.05f)
        {
          
            rb.velocity = Vector3.zero;
            shoot = false;
            Invoke("EnabledCOllider", 0.5f);
            AlreadySwitched = false;
        }
    }

    void EnabledCOllider()
    {
        col.enabled = true;
      
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Black") || collision.CompareTag("White") || collision.CompareTag("Pink"))
        {
            canAdjust = false;
            ErrorMessage.SetActive(true);
            ErrorMessage.GetComponent<ErrorMessage>().ShowMessage();
        }
    }

    public void SwitchCanAdjust()
    {
        canAdjust = true;
        col.isTrigger = false;
        ErrorMessage.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

}
