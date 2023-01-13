using UnityEngine;

public class BreakableWallLife : MonoBehaviour {

    [SerializeField] private int _life;

    public void TakeDamage(int _damage) {
        _life -= _damage;
    
        if (_life <= 0)
            Dead();
    }

    public void Dead() { 
        Destroy(this.gameObject);
    }

}
