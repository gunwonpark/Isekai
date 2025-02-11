using TMPro;
using UnityEngine;

public class TextGlitch : MonoBehaviour
{
    public float GlitchStrength = 0.5f;
    public float GlitchFrequency = 0.1f;
    public float SpeedMultiplier = 1.0f;

    private TMP_Text m_TextComponent;
    private bool hasTextChanged;

    private struct VertexAnim
    {
        public float glitchSpeed;
        public Vector3 jitterOffset;
    }

    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }

    void Start()
    {
        StartCoroutine(AnimateTextGlitch());
    }

    void ON_TEXT_CHANGED(Object obj)
    {
        if (obj == m_TextComponent)
            hasTextChanged = true;
    }

    System.Collections.IEnumerator AnimateTextGlitch()
    {
        m_TextComponent.ForceMeshUpdate();
        TMP_TextInfo textInfo = m_TextComponent.textInfo;

        int loopCount = 0;
        hasTextChanged = true;

        // Create an array to store pre-computed glitch animation data.
        VertexAnim[] vertexAnim = new VertexAnim[1024];
        for (int i = 0; i < 1024; i++)
        {
            vertexAnim[i].glitchSpeed = Random.Range(0.5f, 2f);
        }

        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

        while (true)
        {
            if (hasTextChanged)
            {
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
                hasTextChanged = false;
            }

            int characterCount = textInfo.characterCount;

            if (characterCount == 0)
            {
                yield return new WaitForSeconds(0.25f);
                continue;
            }

            for (int i = 0; i < characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                VertexAnim vertAnim = vertexAnim[i];

                int materialIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                // Apply glitch effect by randomly moving the vertices.
                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                // Apply jitter offset to simulate glitch.
                vertAnim.jitterOffset = new Vector3(
                    Random.Range(-GlitchStrength, GlitchStrength),
                    Random.Range(-GlitchStrength, GlitchStrength),
                    0
                );

                // Apply glitch to the vertices of the current character
                destinationVertices[vertexIndex + 0] += vertAnim.jitterOffset;
                destinationVertices[vertexIndex + 1] += vertAnim.jitterOffset;
                destinationVertices[vertexIndex + 2] += vertAnim.jitterOffset;
                destinationVertices[vertexIndex + 3] += vertAnim.jitterOffset;

                // Apply random jitter in different directions based on speed and frequency.
                if (Random.value < GlitchFrequency)
                {
                    vertAnim.jitterOffset = new Vector3(
                        Random.Range(-GlitchStrength, GlitchStrength),
                        Random.Range(-GlitchStrength, GlitchStrength),
                        0
                    );
                }

                vertexAnim[i] = vertAnim;
            }

            // Update the mesh with the new vertex positions.
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            loopCount += 1;
            yield return new WaitForSeconds(SpeedMultiplier);
        }
    }
}