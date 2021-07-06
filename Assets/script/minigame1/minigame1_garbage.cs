using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame1_garbage : MonoBehaviour
{
    public string garbageType;
    public float speedScale;
    public minigame1_mainScript _mainScript;
    [SerializeField]
    float xValue;
    [SerializeField]
    float xMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    float yValue;
    [SerializeField]
    float yMax;
    [SerializeField]
    float yMin;

    public GameObject wave_position;
    bool waveUp;
    float currentY;
    [SerializeField]
    float dis;
    [SerializeField]
    bool startUpWave;
    [SerializeField]
    bool isDrag;
    [SerializeField]
    Vector2 startDragPosition;
    [SerializeField]
    float speedComeBack;
    [SerializeField]
    bool readyDrag;
    [SerializeField]
    bool readyBack;
    // Start is called before the first frame update
    void Start()
    {
        currentY = transform.position.y;
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = Vector2.Lerp(transform.localScale,
            Vector2.one,Time.fixedDeltaTime * speedScale
            );

        if (!isDrag)
        {

            if (wave_position.GetComponent<minigame1_wave>().startWaveUP && !startUpWave)
            {
                startUpWave = true;
                StartCoroutine(delayWaveUP());
            }

            if (waveUp)
            {
                moveFollowWave();
            }

            if (transform.position.y > 2.7f)
            {
                _mainScript.setCountGarbage();
                Destroy(this.gameObject);
            }
        }

        if (readyBack)
        {
            backMove();
        }
    }

     IEnumerator delayWaveUP()
    {
        yield return new WaitForSeconds(.5f);
        dis = transform.position.y - wave_position.transform.position.y;
        waveUp = true;

    }
    void moveFollowWave()
    {
        transform.position = new Vector2(transform.position.x, ((wave_position.transform.position.y)*.9f+dis));
    }
    private void OnMouseDrag()
    {
        if (readyDrag)
        {
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            xValue = Mathf.Clamp(mouse_position.x, xMin, xMax);
            yValue = Mathf.Clamp(mouse_position.y, yMin, yMax);
            transform.position = new Vector2(xValue, yValue);
            isDrag = true;
        }
    }

    private void OnMouseDown()
    {
        if (readyDrag)
        {
            startDragPosition = transform.position;
        }
    }

    private void OnMouseUp()
    {
        isDrag = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "bin")
        {
            minigame1_bin _bin = collision.GetComponent<minigame1_bin>();
            if(_bin.binType == garbageType)
            {
                _mainScript.keepGarbageDiscount();
                _mainScript.playCorrectSound();
                collision.GetComponent<Animator>().SetTrigger("correct");
                Destroy(this.gameObject);
            }
            else
            {
                collision.GetComponent<Animator>().SetTrigger("wrong");
                _mainScript.playWrongSound();
                movetoPosistion();
            }
        }
    }


    public void movetoPosistion()
    {
        readyDrag = false;
        StartCoroutine(delayBack());
    }

    IEnumerator delayBack()
    {
        yield return new WaitForSeconds(1f);
        readyBack = true;
        
    }

    void backMove()
    {
        transform.position = Vector2.Lerp(transform.position, startDragPosition, Time.fixedDeltaTime * speedComeBack);
        if (Vector2.Distance(transform.position, startDragPosition) <= .1f)
        {
            readyDrag = true;
            readyBack = false;
        }
    }
}
