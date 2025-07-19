using System;
using Unity.VisualScripting;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public Color hoverColor = Color.red;
    public float cardSpeed = 10.0f;
    [SerializeField] private int cardIndex = 0;
    [SerializeField] Vector3 stationaryPos = new Vector3(65, 5, 0);
    [SerializeField] Vector3 hoveredPos = new Vector3(40, 5, 0);

    private Renderer objectRenderer;
    private CardDirection direction;
    private bool isDragging = false;
    private GameManager gameManager;

    public enum CardDirection
    {
        Hovered,
        Unhovered,
        Stationary,
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        objectRenderer = GetComponent<Renderer>();
        direction = CardDirection.Stationary;

        // 
        hoveredPos.z = getZ();
        stationaryPos.z = getZ();

        transform.position = stationaryPos;
    }

    private float getZ()
    {
        return (transform.localScale.z * (cardIndex - 2)) + Camera.main.transform.position.z;
    }

    void Update()
    {
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

        if (direction == CardDirection.Hovered || direction == CardDirection.Unhovered)
        {
            Vector3 destination = (direction == CardDirection.Hovered) ? hoveredPos : stationaryPos;
            transform.position = Vector3.MoveTowards(transform.position, destination, cardSpeed * Time.deltaTime);

            // Stationary in threshhold
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                transform.position = destination; // Snaps card
                direction = CardDirection.Stationary;
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
