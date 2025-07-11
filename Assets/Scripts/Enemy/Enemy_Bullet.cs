using UnityEngine;
using UnityEngine.Rendering;

public class Enemy_Bullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        CreateImpactFX(collision);
        ReturnBulletToPool();

        Player payer = collision.gameObject.GetComponentInParent<Player>();

        if(payer != null)
        {
            Debug.Log("»÷ÖÐÍæ¼Ò");
        }
    }
}