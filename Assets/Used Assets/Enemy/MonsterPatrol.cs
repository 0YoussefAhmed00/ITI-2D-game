using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;
    
    [Header("Sprite Flips")]
    public GameObject spriteUp;
    public GameObject spriteDown;

    // Track if trap has been triggered
    private bool triggered = false;

    private Vector2 target;

    private void Start()
    {
        target = endPoint.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            if (target == (Vector2)endPoint.position)
            {
                target = startPoint.position;
                spriteUp.SetActive(true);
                spriteDown.SetActive(false);
            }
            else
            {
                target = endPoint.position;
                spriteUp.SetActive(false);
                spriteDown.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.KillPlayer(); // Direct call without delay
            }
        }
    }

    private void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
