using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_splash: MonoBehaviour, Projectile_script {
    public float damage;
    public float aoe = 3;
    public Vector3 targetPosition;
    public AudioSource explodeAud;

    float epsilon = 0.1f;

    public void Update()
    {
        if((transform.position - targetPosition).magnitude < epsilon){
            explode();
        }
    }

    public void explode(){
        explodeAud.Play();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoe);
        foreach(Collider o in hitColliders){
            if(o.CompareTag("Monster")){
                o.GetComponent<Monster_script>().damage(damage);
            }
        }
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage){
        this.damage = damage;
    }

    public void SetTarget(Vector3 targetPosition){
        this.targetPosition = targetPosition;
    }
}
