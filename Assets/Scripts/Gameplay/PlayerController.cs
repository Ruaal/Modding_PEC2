using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private bool isMoving = false;
    private LayerMask layerMask;
    public static event Action OnPlayerMove;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Box", "Wall");
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if (!isMoving && moveInput != Vector2.zero)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 1, layerMask);
            if (hit.collider == null)
            {
                StartCoroutine(MovePlayer(moveInput));
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                bool canMove = hit.collider.gameObject.GetComponent<BoxController>().Move(moveInput);
                if (canMove)
                {
                    StartCoroutine(MovePlayer(moveInput));
                }
            }
        }

    }

    IEnumerator MovePlayer(Vector2 moveDirection)
    {
        OnPlayerMove?.Invoke();
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
