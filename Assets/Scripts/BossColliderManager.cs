using UnityEngine;
using System.Collections.Generic;

public class HeadColliderManager : MonoBehaviour
{
    [SerializeField] private List<PolygonCollider2D> colliderSteps;
    [SerializeField] private PolygonCollider2D rootCollider;
    [SerializeField] private int colliderStep = 0;

    public void Advance()
    {
        if (colliderStep >= 0 && colliderStep < colliderSteps.Count)
        {
            PolygonCollider2D next = colliderSteps[colliderStep];
            Vector2[] tempArray = (Vector2[])next.points.Clone();
            rootCollider.SetPath(0, tempArray);
        }
    }

    public void SetColliderStep(int step)
    {
        colliderStep = step;
    }
}
