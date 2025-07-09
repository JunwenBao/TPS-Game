using System.Linq;
using UnityEngine;

public class Enemy_Visuals : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Texture[] colorTextures;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Start()
    {
        InvokeRepeating(nameof(SetupLook), 0, 1.5f);
    }

    public void SetupLook()
    {
        SetupRandomColor();
    }

    // 将角色的Texture设置为随机Texture
    private void SetupRandomColor()
    {
        int randomIndex = Random.Range(0, colorTextures.Length);

        Material newMat = new Material(skinnedMeshRenderer.material);

        newMat.mainTexture = colorTextures[randomIndex];

        skinnedMeshRenderer.material = newMat;
    }
}