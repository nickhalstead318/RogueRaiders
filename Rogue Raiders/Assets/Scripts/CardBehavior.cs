using System;
using Unity.VisualScripting;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public Color hoverColor = Color.red;
    public float cardSpeed = 10.0f;
    public int cardIndex = 0;
    [SerializeField] Vector3 unhoveredPos = new Vector3(65, 5, 0);
    [SerializeField] Vector3 hoveredPos = new Vector3(40, 5, 0);

    private Renderer objectRenderer;
    private CardDirection direction;
    private bool isDragging = false;

    public enum CardDirection
    {
        Unhovered,
        Hovered
    }

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        direction = CardDirection.Unhovered;

        // Update z coords
        hoveredPos.z = getZ();
        unhoveredPos.z = getZ();

        transform.position = unhoveredPos;
    }

    private float getZ()
    {
        return (transform.localScale.z * (10.1f * (cardIndex - 2))) + Camera.main.transform.position.z;
    }

    void Update()
    {
        // Dragging the card
        if (isDragging)
        {
            if (Input.GetMouseButtonDown(1))
            {
                ReleaseCard();
            }

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain Z distance
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            return; // Early quits if dragging
        }

        // Update z coords
        hoveredPos.z = getZ();
        unhoveredPos.z = getZ();

        // Not dragging the card
        if (direction == CardDirection.Hovered || direction == CardDirection.Unhovered)
        {
            Vector3 destination = (direction == CardDirection.Hovered) ? hoveredPos : unhoveredPos;
            transform.position = Vector3.MoveTowards(transform.position, destination, cardSpeed * Time.deltaTime);

            // Stationary in threshhold
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                transform.position = destination; // Snaps card
            }
        }
    }


    public void OnMouseEnter()
    {
        // Highlight the object
        objectRenderer.material.color = hoverColor;
        direction = CardDirection.Hovered;
    }

    public void OnMouseExit()
    {
        if (isDragging) return; // Don't unhover if actively dragging
        objectRenderer.material.color = Color.white;
        direction = CardDirection.Unhovered;
    }

    public void OnMouseDown()
    {
        if (!isDragging)
        {
            isDragging = true;
        }
        else
        {
            ReleaseCard();
        }
    }

    private void ReleaseCard()
    {
        isDragging = false;
        if (!transform.position.Equals(hoveredPos))
        {
            direction = CardDirection.Hovered;
        }
    }
}
