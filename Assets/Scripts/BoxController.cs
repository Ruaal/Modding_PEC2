using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    private bool isMoving = false;
    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Box", "Wall");
    }
    public bool Move(Vector2 moveDirection)
    {
        // Almacena la capa original del objeto
        int originalLayer = gameObject.layer;

        // Cambia la capa del objeto a una que no sea detectada por el Raycast
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // Comprobamos si la nueva posición está vacía
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1, layerMask);

        // Restaura la capa original del objeto
        gameObject.layer = originalLayer;

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                return false;
            }
        }

        StartCoroutine(MoveBox(moveDirection));
        return true;
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