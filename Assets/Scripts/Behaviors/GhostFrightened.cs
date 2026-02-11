using UnityEngine;

public class GhostFrightened : GhostBehavior
{

    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    public float frightenedSpeedMultiplier = 1f;

    private bool eaten;
    private GhostBehavior previousBehavior;
    private float previousDuration;
    private float previousSpeedMultiplier;

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
        
        // Rétablir le comportement précédent avec ses propriétés originales
        if (previousBehavior != null)
        {
            ghost.movement.speedMultiplier = previousSpeedMultiplier;
            previousBehavior.Enable(previousDuration);
        }
    }

    private void Eaten()
    {
        eaten = true;
        ghost.SetPosition(ghost.home.inside.position);
        ghost.home.Enable(duration);

        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();
        ghost.movement.speedMultiplier = frightenedSpeedMultiplier;
        eaten = false;

        // Sauvegarder et désactiver le comportement actuellement actif
        if (ghost.chase.enabled)
        {
            previousBehavior = ghost.chase;
            previousDuration = ghost.chase.duration;
            previousSpeedMultiplier = ghost.chase.GetComponent<GhostChase>().chaseSpeedMultiplier;
            ghost.chase.enabled = false;
            ghost.chase.CancelInvoke();
        }
        else if (ghost.scatter.enabled)
        {
            previousBehavior = ghost.scatter;
            previousDuration = ghost.scatter.duration;
            previousSpeedMultiplier = ghost.scatter.GetComponent<GhostScatter>().scatterSpeedMultiplier;
            ghost.scatter.enabled = false;
            ghost.scatter.CancelInvoke();
        }
        else if (ghost.home.enabled)
        {
            previousBehavior = ghost.home;
            previousDuration = ghost.home.duration;
            previousSpeedMultiplier = 1f; // Home n'a pas de speedMultiplier spécifique
            ghost.home.enabled = false;
            ghost.home.CancelInvoke();
        }
    }

    private void OnDisable()
    {;
        eaten = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Find the available direction that moves farthest from pacman
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // If the distance in this direction is greater than the current
                // max distance then this direction becomes the new farthest
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled) {
                Eaten();
            }
        }
    }

}
