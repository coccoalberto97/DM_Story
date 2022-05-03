using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeDamageSource : DynamicDamageSource
{
    protected float maxTime;
    protected float time;
    protected Collider2D colliderElement;

    private ContactFilter2D filter2D;

    protected override void OnEnable()
    {
        base.OnEnable();
        colliderElement = GetComponent<Collider2D>();
        filter2D = new ContactFilter2D();
        filter2D.layerMask = hittableMask;
        CheckForCollisions2D();
    }


    /// <summary>
    /// Questo potrebbe restare più generico e sovrascritto in caso di cambiamento
    /// per ora consideriamo il melee un'arma che colpisce sempre tutti i nemici che trova
    /// invece di fermarsi al primo
    /// </summary>
    /// <returns>vero se ha colpito almeno 1 nemico falso altrimenti</returns>
    public override bool CheckForCollisions2D()
    {
        List<Collider2D> hits = new List<Collider2D>();
        colliderElement.OverlapCollider(filter2D, hits);
        bool ret = false;
        if (hits != null && hits.Count > 0)
        {
            foreach (Collider2D hit in hits)
            {
                IHittable[] hittables = hit.GetComponents<IHittable>();
                foreach (IHittable hittable in hittables)
                {
                    if (!hittableMask.Contains(hit.gameObject.layer))
                    {
                        continue;
                    }

                    //per ora prendo il primo punto di contatto successivamente potrei alternarli
                    //ad esempio 1 ogni 20 in caso di boss molto grandi da capire
                    List<ContactPoint2D> contacts = new List<ContactPoint2D>();
                    hit.GetContacts(contacts);
                    if (contacts != null && contacts.Count > 0)
                    {
                        hittable.OnHit(new Vector3(contacts[0].point.x, contacts[0].point.y, 0), this);
                    }
                }
                ret = true;
            }
        }
        return ret;
    }
}
