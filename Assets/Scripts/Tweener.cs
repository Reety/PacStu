using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private List<Tween> activeTween = new List<Tween>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Tween tween in activeTween.ToList())
        {
            float elapsedTime = Time.time - tween.StartTime;
            float timeFraction = elapsedTime / tween.Duration;
            //float cubicEaseIn = timeFraction * timeFraction * timeFraction;

            if (Vector3.Distance(tween.Target.position, tween.EndPos) > 0.1f)
            {
                tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, timeFraction);
            }

            else
            {
                tween.Target.position = tween.EndPos;
                activeTween.Remove(tween);
                //print($"removed {tween.Target.gameObject}");
            }
        }
        /*if (activeTween != null)
        {
            float elapsedTime = Time.time - activeTween.StartTime;
            float timeFraction = elapsedTime / activeTween.Duration;
            float cubicEaseIn = timeFraction * timeFraction * timeFraction;

            if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
                activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, cubicEaseIn);
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
            }
        }*/


    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        //activeTween ??= new Tween(targetObject, startPos, endPos, Time.time, duration);
        if (TweenExists(targetObject)) return false;
        else
        {
           activeTween.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
           return true;
        }
    }

    public bool TweenExists(Transform target)
    {
        return activeTween.Exists(x => x.Target.Equals(target));
    }
}
