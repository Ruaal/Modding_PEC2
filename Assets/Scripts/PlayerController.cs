using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    private bool isMoving = false;

    void Update()
    {
        // Entrada del jugador
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        // Movemos al jugador
        if (!isMoving)
        {
            StartCoroutine(MovePlayer(moveInput));
        }
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.4f, LayerMask.GetMask("Box"));
        if (hit != null)
        {
            hit.gameObject.GetComponent<BoxController>().Move(moveInput);
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
