using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_splash: MonoBehaviour, Projectile_script {
    public float damage;
    public float aoe = 3;
    public Vector3 targetPosition;
    public AudioSource explodeAud;
    public GameObject shockWaveVFX;

    float epsilon = 0.1f;
    bool active = true;

    public void FixedUpdate()
    {
        if((transform.position - targetPosition).magnitude < epsilon && active){
            StartCoroutine(explode());
            active = false;
        }
    }

    public IEnumerator explode(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, aoe);
        foreach(Collider o in hitColliders){
            if(o.CompareTag("Monster")){
                o.GetComponent<Monster_script>().damage(damage);
            }
        }
        //Wait for the SFX to finish playing
        transform.Find("SplashBullet_original").gameObject.SetActive(false); 
        explodeAud.Play();
        GameObject shockWaveInstance = Instantiate(shockWaveVFX, transform.position, transform.rotation);
        shockWaveInstance.GetComponent<ShockWaveVFX>().SetAoe(aoe);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        yield return 0;
    }

    public void SetDamage(float damage){
        this.damage = damage;
    }

    public void SetTarget(Vector3 targetPosition){
        this.targetPosition = targetPosition;
    }

    public void SetAoe(float aoe){
        this.aoe = aoe;
    }
}
