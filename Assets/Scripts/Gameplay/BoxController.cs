using System;
using System.Collections;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private bool isMoving = false;
    private LayerMask layerMask;
    public static event Action OnBoxMove;


    private void Start()
    {
        layerMask = LayerMask.GetMask("Box", "Wall");
    }
    public bool Move(Vector2 moveDirection)
    {
        int originalLayer = gameObject.layer;

        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1, layerMask);

        gameObject.layer = originalLayer;

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                return false;
            }
        }
        if (!isMoving)
        {
            StartCoroutine(MoveBox(moveDirection));
        }
        return true;
    }

    IEnumerator MoveBox(Vector2 moveDirection)
    {
        OnBoxMove?.Invoke();
        isMoving = true;
        float elapsedTime = 0;
        if (Mathf.Abs(moveDirection.x) > 0.5f)
        {
            moveDirection.y = 0;
        }
        else
        {
            moveDirection.x = 0;
        }

        Vector2 startPoint = transform.position;
        Vector2 endPoint = startPoint + moveDirection;

        while (elapsedTime < 0.3f)
        {
            transform.position = Vector2.Lerp(startPoint, endPoint, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint;

        isMoving = false;
    }
}