using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostDetector : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    [SerializeField] GameObject miniIndicators;

    [SerializeField] GameObject innerCircle;
    [SerializeField] GameObject outerCircle;


    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this == innerCircle)
        {
            if (collision.tag == "Ghost")
            {
                indicator.SetActive(true);
            }
        }
        else if (this == outerCircle)
        {
            if (collision.tag == "Ghost")
            {
                miniIndicators.SetActive(true);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this == innerCircle)
        {
            if (collision.tag == "Ghost")
            {
                indicator.SetActive(false);
            }
        }
        else if (this == outerCircle)
        {
            if (collision.tag == "Ghost")
            {
                miniIndicators.SetActive(false);
            }
        }
    }

    //Originals in case things break
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            indicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ghost")
        {
            indicator.SetActive(false);
        }
    }
    */
}