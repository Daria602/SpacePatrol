using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public RectTransform HeartsHolder;
    public void SetHeartInactive()
    {
        Debug.Log("Animation Event was called");
        gameObject.SetActive(false);
    }
    public void SetHeartActive()
    {
        Debug.Log("Animation Event was called");
        gameObject.GetComponent<Animator>().SetBool("hasAppeared", true);
    }
}
