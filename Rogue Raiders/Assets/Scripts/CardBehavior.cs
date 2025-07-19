using Unity.VisualScripting;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public Color hoverColor = Color.red;
    public float cardSpeed = 10.0f;

    private Renderer objectRenderer;
    private Vector3 hoveredPosition;
    private Vector3 stationaryPosition;
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

        // These are the positions based on a main camera at 0, 50, 0 and at angle 90, -90, 0
        stationaryPosition = new Vector3(50, -39, 0);
        transform.position = stationaryPosition;
        hoveredPosition = new Vector3(33, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && isDragging)
        {
            ReleaseCard();
        }

        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain Z distance
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldMousePosition;
        }
        else if (direction == CardDirection.Hovered || direction == CardDirection.Unhovered)
        {
            Vector3 destination = direction == CardDirection.Hovered ? hoveredPosition : stationaryPosition;
            transform.position = Vector3.MoveTowards(transform.position, destination, cardSpeed*Time.deltaTime);
            if (transform.position.Equals(destination))
            {
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
        if (!transform.position.Equals(hoveredPosition))
        {
            direction = CardDirection.Hovered;
        }
    }
}
