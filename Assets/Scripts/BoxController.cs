using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private bool isMoving = false;

    public void Move(Vector2 moveDirection)
    {
        // Calculamos la posición a la que se quiere mover la caja
        Vector2 targetPos = new Vector2(transform.position.x, transform.position.y) + moveDirection;

        // Comprobamos si la nueva posición está vacía
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, LayerMask.GetMask("Box") | LayerMask.GetMask("Wall"));

        if (hit == null)
        {
            // Si está vacía, movemos la caja
            StartCoroutine(MoveBox(moveDirection));
        }
    }

    IEnumerator MoveBox(Vector2 moveDirection)
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