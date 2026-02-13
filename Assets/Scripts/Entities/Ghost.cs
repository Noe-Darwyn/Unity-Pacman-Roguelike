using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(GhostLifeManager))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostLifeManager lifeManager { get; private set; }
    public GhostBehavior initialBehavior;
    public GhostBehaviorType initialBehaviorType;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
        lifeManager = GetComponent<GhostLifeManager>();
        
        initialBehavior = GetBehaviorFromType(initialBehaviorType);
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Z détermine la profondeur de dessin, donc on la garde inchangée
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.GhostEaten(this);
            } else {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

    private GhostBehavior GetBehaviorFromType(GhostBehaviorType behaviorType)
    {
        return behaviorType switch
        {
            GhostBehaviorType.Home => home,
            GhostBehaviorType.Scatter => scatter,
            GhostBehaviorType.Chase => chase,
            GhostBehaviorType.Frightened => frightened,
            _ => chase
        };
    }

}
