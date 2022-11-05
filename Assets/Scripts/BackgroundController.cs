using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float yStart;
    public float yDifference;
  

    // Start is called before the first frame update
    void Start()
    {
        yStart = transform.localPosition.y;
        yDifference = 222;
    }

    // Update is called once per frame
    void Update()
    {
        VerticalMovement();
    }

    private void VerticalMovement()
    {
        float yCurrent = transform.localPosition.y;
        float toSubstract = 0.3f;
        if (yCurrent - toSubstract > yStart - yDifference)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yCurrent - toSubstract);
        }
    }
}
