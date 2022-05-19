using UnityEngine;

[CreateAssetMenu(menuName = "Block Config")]

public class BlockConfig : ScriptableObject
{
    public float[] dropSpeed;
    public Sprite[] basicBlockSprites;
    public GameObject explosion;

    public GameObject GetExplosionObject()
    {
        return Instantiate(explosion) as GameObject;
    }
}
