using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;

    protected override void Eat()
    {
        GameManager.Instance.PowerPelletEaten(this, collector);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {   
        collector = other.GetComponent<MonoBehaviour>();
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman") ) 
        {
            Eat();
            //print ("Pacman Power pellet collected");
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ghost") ) 
        {
            Eat();
            //print ("Ghost Power pellet collected");
        }

    }
    
}

