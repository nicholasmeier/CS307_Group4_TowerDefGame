using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_bullet : MonoBehaviour, Projectile_script {
    public float damage;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            Destroy(this.gameObject);
            other.gameObject.GetComponent<Monster_script>().damage(damage);
            //Debug.Log("bang");
        }
    }

    public void SetDamage(float damage){
        this.damage = damage;
    }
}
