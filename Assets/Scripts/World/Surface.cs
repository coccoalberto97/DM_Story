using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surface : MonoBehaviour, IHittable
{

    public string surfaceHitParticleTag;

    public void OnHit(Vector3 position, Projectile projectile)
    {
        ObjectPoolManager.instance.SpawnFromPool(surfaceHitParticleTag, position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
