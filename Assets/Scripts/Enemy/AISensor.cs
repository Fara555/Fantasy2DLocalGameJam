using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    [HideInInspector] public bool seenPlayer;
    [SerializeField] private float radius;
    [SerializeField] private bool drawGizmo;
    [SerializeField] private Color color;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = radius;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) seenPlayer = true;
    }

    #region Debug

    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, radius);  
        }
    }

    #endregion
}
