using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterPatrol : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;

    public GameObject spriteUp;
    public GameObject spriteDown;

    private Vector2 target;
    public float delayBeforeMainMenu = 1.5f;

    private void Start()
    {
        target = (Vector2)endPoint.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards((Vector2)transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance((Vector2)transform.position, target) < 0.1f)
        {
            if (target == (Vector2)endPoint.position)
            {
                target = (Vector2)startPoint.position;
                spriteUp.SetActive(true);
                spriteDown.SetActive(false);
            }
            else
            {
                target = (Vector2)endPoint.position;
                spriteUp.SetActive(false);
                spriteDown.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Invoke(nameof(GoToMainMenu), delayBeforeMainMenu);
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
