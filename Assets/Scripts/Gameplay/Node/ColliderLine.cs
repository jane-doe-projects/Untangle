using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLine : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 endPoint;

    List<Vector2> path;
    LineRenderer lineRenderer;
    PolygonCollider2D polyCollider;
    Rigidbody2D rb;

    GameObject lineEffectObj;
    LineEffect le;

    // line properites
    float thickness = 0.01f;
    float trimmingLength = -0.30f;
    float widthMultiplierForLine = 0.12f;

    Node node;
    [SerializeField] bool isCrossed;

    [Header("Particle Effect")]
    public bool showEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Line";

        node = GetComponentInParent<Node>();
        startPoint = transform.position;
        endPoint = node.successor.transform.position;

        InitPoints();
        InitLineRenderer();
        InitCollider();

        if (showEffect)
            InitEffect();
    }

    public void UpdateLine(Vector3 start, Vector3 end)
    {
        startPoint = start;
        endPoint = end;

        SetPoints();
        RenderLine();
    }

    void RenderLine()
    {
        // update line renderer
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);

        if (showEffect)
            UpdateEffect();

        // update collider position / path
        polyCollider.SetPath(0, path);
    }

    void UpdateEffect()
    {
        float distance = Vector3.Distance(startPoint, endPoint);
        Vector3 mid = Vector3.Lerp(startPoint, endPoint, 0.5f);
        le.SetRadius(distance/2);
        le.SetPosition(mid);
        le.SetRotation(endPoint);
    }

    void InitEffect()
    {
        lineEffectObj = Instantiate(GameManager.Instance.gameElements.lineEffect, transform);
        le = lineEffectObj.GetComponent<LineEffect>();
        le.Init();
        UpdateEffect();

    }

    void InitLineRenderer()
    {
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = GameManager.Instance.currentTheme.lineColorStart;
        lineRenderer.endColor = GameManager.Instance.currentTheme.lineColorEnd;
        lineRenderer.material = GameManager.Instance.gameElements.crossedLineMaterialRuntime;
        lineRenderer.widthMultiplier = widthMultiplierForLine;
        lineRenderer.textureMode = LineTextureMode.Tile;

        lineRenderer.positionCount = 2;
        Vector3 clampStart = startPoint;
        clampStart.z = 0;

        lineRenderer.SetPosition(0, clampStart);
        lineRenderer.SetPosition(1, endPoint);

        /*
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint); */
    }

    public void ReinitializeLineRendererColors()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    void InitCollider()
    {
        polyCollider = this.gameObject.AddComponent<PolygonCollider2D>();
        polyCollider.isTrigger = true;
        polyCollider.SetPath(0, path);
        rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    void Crossed()
    {
        lineRenderer.material = GameManager.Instance.gameElements.crossedLineMaterialRuntime;
    }

    void Uncrossed()
    {
        lineRenderer.material = GameManager.Instance.gameElements.uncrossedLineMaterialRuntime;
    }

    void InitPoints()
    {
        path = new List<Vector2>();

        Vector2 start1 = Vector2.zero;
        Vector2 start2 = Vector2.zero;

        Vector2 end1 = Vector2.zero;
        Vector2 end2 = Vector2.zero;

        path.Add(start1);
        path.Add(start2);
        path.Add(end1);
        path.Add(end2);

        CalculateColliderPath();
    }

    void SetPoints()
    {
        CalculateColliderPath();
    }

    public bool UpdateCrossingState()
    {
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        int resultCount = polyCollider.OverlapCollider(filter, results);

        bool collisionWithLine = false;

        foreach (Collider2D collision in results)
        {
            if (collision.gameObject.CompareTag("Line"))
            {
                bool isNeighbouringLine = IsNeighbour(collision.gameObject);
                if (!isNeighbouringLine)
                {
                    collisionWithLine = true;
                    break;
                }
            }
        }

        isCrossed = collisionWithLine;
        UpdateCrossingVisual();
        return collisionWithLine;
    }

    void UpdateCrossingVisual()
    {
        if (isCrossed)
            Crossed();
        else
            Uncrossed();
    }

    List<Vector2> CalculateColliderPath()
    {
        List<Vector2> colliderPath = new List<Vector2>();
        Vector2 shortB = endPoint;
        Vector2 shortA = startPoint;

        float width = shortB.x - shortA.x;
        float height = shortB.y - shortA.y;

        float length = Mathf.Sqrt(width * width + height * height);

        float xShift = (thickness * height / length) / 2;
        float yShift = (thickness * width / length) / 2;

        Vector2 start1 = new Vector2(shortA.x - xShift, shortA.y + yShift);
        Vector2 start2 = new Vector2(shortA.x + xShift, shortA.y - yShift);
        Vector2 end1 = new Vector2(shortB.x + xShift, shortB.y - yShift);
        Vector2 end2 = new Vector2(shortB.x - xShift, shortB.y + yShift);

        path[0] = transform.InverseTransformPoint(start1);
        path[1] = transform.InverseTransformPoint(start2);
        path[2] = transform.InverseTransformPoint(end1);
        path[3] = transform.InverseTransformPoint(end2);

        return colliderPath;
    }

    Vector2 ShortenEnd()
    {
        Vector2 shortA = new Vector2(startPoint.x, startPoint.y);
        Vector2 shortB = new Vector2(endPoint.x, endPoint.y);

        float dx = endPoint.x - startPoint.x;
        float dy = endPoint.y - startPoint.y;

        if (dx == 0)
        {
            if (endPoint.y < startPoint.y)
                shortB.y -= trimmingLength;
            else
                shortB.y += trimmingLength;
        } else if (dy == 0)
        {
            if (endPoint.x < startPoint.x)
                shortB.x -= trimmingLength;
            else
                shortB.x += trimmingLength;
        } else
        {
            float length = Mathf.Sqrt(dx * dx + dy * dy);
            float scale = (length + trimmingLength) / length;
            dx *= scale;
            dy *= scale;
            shortB.x = startPoint.x + dx;
            shortB.y = startPoint.y + dy;
        }

        return shortB;
    }

    bool IsNeighbour(GameObject go)
    {
        ColliderLine colLine = go.GetComponent<ColliderLine>();
        if (node.successor.GetLine() == colLine || node.predecessor.GetLine() == colLine)
            return true;
        return false;
    }
}
