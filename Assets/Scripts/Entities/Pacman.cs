using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.Collections;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;

    private List<Transform> pellets;
    private int pelletLayer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();

        pelletLayer = LayerMask.NameToLayer("Pellet");
    }

    private void Start()
    {
        pellets = FindObjectsOfType<Transform>()
            .Where(t => t.gameObject.layer == pelletLayer)
            .ToList();
    }

    private void Update()
    {
        // 1. Choisir la nouvelle direction
        Vector2 newDirection = ChooseDirection();
        movement.SetDirection(newDirection);
        
        // 2. Rotation de Pacman
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
    
    private Vector2 ChooseDirection()
    {
        // MODE FUITE 
        Vector2 fleeDirection = TryFleeMode();
        if (fleeDirection != Vector2.zero) {
            Debug.Log("MODE FUITE activé - Direction: " + fleeDirection);
            return fleeDirection;
        }
        
        // MODE CHASSE 
        Vector2 chaseDirection = TryChaseMode();
        if (chaseDirection != Vector2.zero)
        {
            Debug.Log("MODE CHASSE activé - Direction: " + chaseDirection);
            return chaseDirection;
        }
        
        // MODE COLLECTE (par défaut)
        Vector2 collectDirection = CollectMode();
        Debug.Log("MODE COLLECTE activé - Direction: " + collectDirection);
        return collectDirection;
    }

    private Vector2 TryFleeMode()
    {
        // Trouver le fantôme dangereux le plus proche
        Transform closestDangerousGhost = GetClosestDangerousGhost();
        
        if (closestDangerousGhost == null){
            //Debug.Log("FUITE: Aucun fantôme dangereux trouvé");
            return Vector2.zero;
        }
        // Vérifier la distance
        float distance = Vector2.Distance(transform.position, closestDangerousGhost.position);
        
        if (distance >= 4f)
            return Vector2.zero; // Pas assez proche pour fuir
        
        // Calculer la meilleure direction de fuite
        return GetFleeDirection(closestDangerousGhost);
    }

    private Transform GetClosestDangerousGhost()
    {
        Transform closest = null;
        float minDistance = float.MaxValue;
        
        foreach (Ghost ghost in GameManager.Instance.Ghosts)
        {
            if (ghost == null) continue;
            
            // Ignorer si frightened ou dans la maison
            if (ghost.frightened.enabled) continue;
            if (ghost.home != null && ghost.home.enabled) continue;
            
            // Calculer distance
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = ghost.transform;
            }
        }
        
        return closest;
    }

    private Vector2 GetFleeDirection(Transform dangerousGhost)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 opposite = -movement.direction;
        
        Vector2 bestDirection = movement.direction;
        float bestScore = float.MinValue;
        
        // Compter le nombre de directions disponibles
        int availableDirections = 0;
        foreach (Vector2 dir in directions)
        {
            if (!movement.Occupied(dir))
                availableDirections++;
        }
        
        bool inCorridor = availableDirections <= 2;
        
        // Vérifier si le fantôme est droit devant (dans la direction actuelle)
        bool ghostAhead = false;
        if (inCorridor)
        {
            Vector2 toGhost = (Vector2)(dangerousGhost.position - transform.position);
            float alignment = Vector2.Dot(toGhost.normalized, movement.direction);
            ghostAhead = alignment > 0.7f; // Le fantôme est dans la direction actuelle
        }
        
        
        foreach (Vector2 dir in directions)
        {
            // Ignorer direction si bloquée par un mur
            if (movement.Occupied(dir))
            {
                Debug.Log($"Direction {dir} : BLOQUÉE");
                continue;
            }
            
            // Dans un couloir, interdire le demi-tour SAUF si fantôme droit devant
            bool isOpposite = Vector2.Dot(dir, opposite) > 0.9f;
            if (inCorridor && isOpposite && !ghostAhead)
            {
                continue;
            }
            
            Vector2 hypotheticalPos = (Vector2)transform.position + dir;
            float distanceToGhost = Vector2.Distance(hypotheticalPos, dangerousGhost.position);
            float score = distanceToGhost;
            
            // Hysteresis léger (bonus si direction actuelle)
            if (Vector2.Dot(dir, movement.direction) > 0.9f)
            {
                score += 0.5f;
            }
            
            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = dir;
            }
        }
        
        return bestDirection;
    }

    private Vector2 TryChaseMode()
    {
        // Trouver le fantôme frightened le plus proche
        Transform closestFrightenedGhost = GetClosestFrightenedGhost();
        
        if (closestFrightenedGhost == null)
            return Vector2.zero;
        
        // Vérifier la distance
        float distance = Vector2.Distance(transform.position, closestFrightenedGhost.position);
        
        if (distance >= 7f) // Seuil à confirmer
            return Vector2.zero;
        
        // Calculer la direction de chasse
        return GetChaseDirection(closestFrightenedGhost);
    }

    private Transform GetClosestFrightenedGhost()
    {
        Transform closest = null;
        float minDistance = float.MaxValue;
        
        foreach (Ghost ghost in GameManager.Instance.Ghosts)
        {
            if (ghost == null) continue;
            
            // On veut uniquement les frightened
            if (!ghost.frightened.enabled) continue;
            
            // Ignorer s'il est dans la maison
            if (ghost.home != null && ghost.home.enabled) continue;
            
            // Calculer distance
            float distance = Vector2.Distance(transform.position, ghost.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = ghost.transform;
            }
        }
        
        return closest;
    }

    private Vector2 GetChaseDirection(Transform frightenedGhost)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 opposite = -movement.direction;
        
        Vector2 bestDirection = movement.direction;
        float bestScore = float.MinValue;
        
        // Compter le nombre de directions disponibles
        int availableDirections = 0;
        foreach (Vector2 dir in directions)
        {
            if (!movement.Occupied(dir))
                availableDirections++;
        }
        
        bool inCorridor = availableDirections <= 2;
        
        foreach (Vector2 dir in directions)
        {
            // Ignorer si bloquée
            if (movement.Occupied(dir)) continue;
            
            // Dans un couloir, interdire le demi-tour ? (ptet pas)
            

            bool isOpposite = Vector2.Dot(dir, opposite) > 0.9f;
            if (inCorridor && isOpposite)
            {
                continue;
            }

            
            
            // Position hypothétique
            Vector2 hypotheticalPos = (Vector2)transform.position + dir;
            
            // Distance au fantôme frightened
            float distanceToGhost = Vector2.Distance(hypotheticalPos, frightenedGhost.position);
            
            // Score = -distance (on veut minimiser, donc se rapprocher)
            float score = -distanceToGhost;
            
            // Hysteresis : bonus si c'est la direction actuelle
            if (Vector2.Dot(dir, movement.direction) > 0.9f)
            {
                score += 3f;
            }
            
            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = dir;
            }
        }
        
        return bestDirection;
    }

    private Vector2 CollectMode()
    {
        // Trouver la pellet cible
        Transform targetPellet = GetTargetPellet();
        Debug.Log("Pellet cible transform : " + targetPellet.transform.position.x + "," + targetPellet.transform.position.y);
        
        if (targetPellet == null)
        {
            // Aucune pellet disponible, comportement par défaut
            return movement.direction; // Continuer tout droit
        }
        
        // Calculer la meilleure direction vers cette pellet
        return GetDirectionTowardPellet(targetPellet);
    }

    private Transform GetTargetPellet()
    {
        // Nettoyer la liste des pellets (retirer celles qui sont null ou inactives)
        pellets.RemoveAll(p => p == null || !p.gameObject.activeInHierarchy);
        
        if (pellets.Count == 0){
            Debug.Log("Aucune pellet restante !");
            return null;
        }
        
        Vector2 pacPos = transform.position;
        
        // Chercher d'abord une pellet sûre
        Transform closestSafePellet = null;
        float minSafeDistance = float.MaxValue;
        
        foreach (Transform pellet in pellets)
        {
            if (IsPelletSafe(pellet, 3f)) // Sûre si aucun fantôme < 3 cases
            {
                float distance = Vector2.Distance(pacPos, pellet.position);
                if (distance < minSafeDistance)
                {
                    minSafeDistance = distance;
                    closestSafePellet = pellet;
                }
            }
        }
        
        // Si pellet sûre trouvée, la retourner
        if (closestSafePellet != null)
            return closestSafePellet;
        
        // Sinon, retourner la plus proche quoi qu'il arrive
        return pellets.OrderBy(p => Vector2.Distance(pacPos, p.position)).FirstOrDefault();
    }

    private bool IsPelletSafe(Transform pellet, float safeDistance)
    {
        foreach (Ghost ghost in GameManager.Instance.Ghosts)
        {
            if (ghost == null) continue;
            
            // Ignorer les fantômes frightened (pas dangereux)
            if (ghost.frightened.enabled) continue;
            
            // Ignorer les fantômes dans la maison
            if (ghost.home != null && ghost.home.enabled) continue;
            
            // Vérifier la distance
            float distance = Vector2.Distance(pellet.position, ghost.transform.position);
            
            if (distance < safeDistance)
                return false; // Pellet dangereuse
        }
        
        return true; // Pellet sûre
    }

    private Vector2 GetDirectionTowardPellet(Transform targetPellet)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 opposite = -movement.direction;
        
        Vector2 bestDirection = movement.direction;
        float bestScore = float.MinValue;
        
        // Compter le nombre de directions disponibles
        int availableDirections = 0;
        foreach (Vector2 dir in directions)
        {
            if (!movement.Occupied(dir))
                availableDirections++;
        }
        
        bool inCorridor = availableDirections <= 2;
        
        Debug.Log($"=== COLLECTE - Vers pellet {targetPellet.name} (disponibles: {availableDirections}, couloir: {inCorridor}) ===");
        
        foreach (Vector2 dir in directions)
        {
            // Ignorer direction si bloquée par un mur
            if (movement.Occupied(dir))
            {
                Debug.Log($"Direction {dir} : BLOQUÉE");
                continue;
            }
            
            // Dans un couloir, interdire le demi-tour
            bool isOpposite = Vector2.Dot(dir, opposite) > 0.9f;
            if (inCorridor && isOpposite)
            {
                Debug.Log($"Direction {dir} : DEMI-TOUR INTERDIT (couloir)");
                continue;
            }
            
            // Position hypothétique
            Vector2 hypotheticalPos = (Vector2)transform.position + dir;
            
            // Distance à la pellet cible
            float distanceToPellet = Vector2.Distance(hypotheticalPos, targetPellet.position);
            
            // Facteur de danger dans cette direction
            float dangerFactor = GetDangerFactor(hypotheticalPos);
            
            float score = -distanceToPellet - (dangerFactor * 10f);
            
            // Hysteresis : bonus si direction actuelle
            if (Vector2.Dot(dir, movement.direction) > 0.9f)
            {
                score += 1.0f;
            }
                        
            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = dir;
            }
        }
        
        return bestDirection;
    }

    private float GetDangerFactor(Vector2 position)
    {
        float minDistanceToGhost = float.MaxValue;
        
        foreach (Ghost ghost in GameManager.Instance.Ghosts)
        {
            if (ghost == null) continue;
            
            // Ignorer les fantômes frightened
            if (ghost.frightened.enabled) continue;
            
            // Ignorer les fantômes dans la maison
            if (ghost.home != null && ghost.home.enabled) continue;
            
            // Distance au fantôme
            float distance = Vector2.Distance(position, ghost.transform.position);
            
            if (distance < minDistanceToGhost)
                minDistanceToGhost = distance;
        }
        
        // Calcul du facteur par paliers
        if (minDistanceToGhost < 5f)
            return 1.0f; // Danger élevé
        else if (minDistanceToGhost < 7f)
            return 0.5f; // Danger moyen
        else
            return 0.0f; // Pas de danger
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

}
