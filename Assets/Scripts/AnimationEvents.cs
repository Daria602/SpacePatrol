using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public RectTransform HeartsHolder;
    public void SetHeartInactive()
    {
        gameObject.SetActive(false);
    }
    public void SetHeartActive()
    {
        gameObject.GetComponent<Animator>().SetBool("hasAppeared", true);
    }
}
