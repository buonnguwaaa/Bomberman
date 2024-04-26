using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExploseContainer : MonoBehaviour
{
    [SerializeField] Transform exploseContainer;
    [SerializeField] Tilemap destructible;
    [SerializeField] GameObject destructiblePrefab;
    [SerializeField] GameObject explosionZone;
    [SerializeField] GameObject[] items;
    float random;

    public void Explosion(Vector2 pos)
    {
        GameObject instant=Instantiate(explosionZone,pos,Quaternion.identity,exploseContainer);
        Explosion newExplose = instant.GetComponent<Explosion>();
        newExplose.exploseCount=1;

        Explose(pos,Vector2.up,GameManager.instant.exploseRadius);
        Explose(pos,Vector2.down,GameManager.instant.exploseRadius);
        Explose(pos,Vector2.left,GameManager.instant.exploseRadius);
        Explose(pos,Vector2.right,GameManager.instant.exploseRadius);
    }

    private void Explose(Vector2 pos, Vector2 direction, int length)
    {
        if(length<=0 ) return;
        pos+=direction;
        if(Physics2D.OverlapBox(pos,Vector2.one/2f,0,LayerMask.GetMask("Destructible","Indestructible")))
        {
            if(destructible) DesroyDetructible(pos);
            return;
        }
        
        Vector3 rotate;
        if(direction==Vector2.up) rotate=new Vector3(0,0,90);
        else if(direction ==Vector2.left) rotate=new Vector3(0,0,180);
        else if(direction==Vector2.down) rotate=new Vector3(0,0,270);
        else rotate=new Vector3(0,0,0);
        GameObject instant=Instantiate(explosionZone,pos,Quaternion.Euler(rotate),exploseContainer);
        Explosion newExplose = instant.GetComponent<Explosion>();
        if(length>1)
        {
            newExplose.exploseCount=2;
        }
        else {
            newExplose.exploseCount=3;
        }
        Explose(pos,direction,length-1);
    }
    void DesroyDetructible(Vector2 pos)
    {
        Vector3Int cell=destructible.WorldToCell(pos);
        TileBase tile=destructible.GetTile(cell);
        if(tile)
        {
            GameObject newDestruct=Instantiate(destructiblePrefab,pos,Quaternion.identity,exploseContainer);
            destructible.SetTile(cell,null);
            Destroy(newDestruct,0.8f);
            random=Random.Range(0,1f);
            if(random<GameManager.instant.rateHasItem)
            {
                Instantiate(items[Random.Range(0,items.Length)],pos,Quaternion.identity);
            }
        }
    }
}
