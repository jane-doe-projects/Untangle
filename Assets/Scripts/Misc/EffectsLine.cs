using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsLine : MonoBehaviour
{
    [SerializeField] GameObject pingPong;
    Vector3 currentTarget;
    Vector3 currentStart;
    float speed = 5f;

    ColliderLine colLine;

    private void Start()
    {
        colLine = GetComponent<ColliderLine>();
    }

    private void Update()
    {
        if (pingPong.transform.position == currentTarget)
        {
            // swap positions once reached
            Vector3 temp = currentTarget;
            currentTarget = currentStart;
            currentStart = temp;
        }

        pingPong.transform.position = Vector3.Lerp(currentStart, currentTarget, speed * Time.deltaTime);
    }

    public void UpdateTargets(Vector3 start, Vector3 end)
    {
        currentStart = start;
        currentTarget = end;
    }
}
