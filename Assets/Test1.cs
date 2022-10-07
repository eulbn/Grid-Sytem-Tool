using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream
using GridSpace;

public class Test1 : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log(GridSnapSystem.ObjectIndex(transform.position));

            GridExtened.AssignObjects("SomeTag", ObjectKind.ObjectTag);
        }
    }
=======
using System.Linq;
using System;
using GridSpace;
public class Test1 : MonoBehaviour
{

    public ConstrainedAnimCurve animationCurve;

    public Vector2 pos1;
    public Vector2 pos2;
    public float duration;


    public float curveTime;
    private void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(MoveTo(gameObject, pos2, pos1, duration, animationCurve));
            StartCoroutine(MoveTo(gameObject, pos1, pos2, duration, animationCurve));


        }


    }

    IEnumerator MoveTo(GameObject currentObj, Vector2 startPos, Vector2 endPos, float duration, ConstrainedAnimCurve constrainedAnimCurve)
    {

        float durationComplete = duration;
        while (durationComplete > 0)
        {
            currentObj.transform.position = Vector3.LerpUnclamped(startPos, endPos, constrainedAnimCurve.curve.Evaluate(1 - (durationComplete -= Time.deltaTime)/duration));
            yield return null;
        }
    }

>>>>>>> Stashed changes
}
