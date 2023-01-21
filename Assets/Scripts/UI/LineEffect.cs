using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffect : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.ShapeModule shape;

    public void Init()
    {
        ps = GetComponent<ParticleSystem>();
        shape = ps.shape;
    }

    public void SetRadius(float radius)
    {
        shape.radius = radius;
    }

    public void SetPosition(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    public void SetRotation(Vector3 end)
    {
        transform.right = end - transform.position;
    }

    
}
