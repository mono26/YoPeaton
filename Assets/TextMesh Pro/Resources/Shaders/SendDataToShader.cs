using UnityEngine;

[ExecuteInEditMode]
public class SendDataToShader : MonoBehaviour
{

    [Range(0.1f, 500.0f)]
    public float PlayerRadius = 1.0f;

    void Update()
    {
        //PARA ENVIAR INFORMACION A LOS SHADERS, PA SABER DONDE DESCUBRIR
        Shader.SetGlobalVector("WorldSpacePlayerPosition", this.transform.position);
        Shader.SetGlobalFloat("PlayerRadius", this.PlayerRadius);
    }
}