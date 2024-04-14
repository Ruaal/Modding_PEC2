using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private bool isMoving = false;
    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Box", "Wall");
    }

    void Update()
    {
        // Entrada del jugador
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        // Movemos al jugador
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

        // Calcula el punto final basado en la dirección de movimiento
        Vector2 startPoint = transform.position;
        Vector2 endPoint = startPoint + moveDirection;

        while (elapsedTime < 0.3f)
        {
            // Interpola entre la posición actual y el punto final
            transform.position = Vector2.Lerp(startPoint, endPoint, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que el jugador llegue al punto final
        transform.position = endPoint;

        isMoving = false;
    }
}
