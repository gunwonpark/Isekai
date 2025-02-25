using UnityEngine;

public class Test_G : MonoBehaviour
{
   public TMPro.TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        StartCoroutine(m_TextMeshPro.CoBlinkText(3, 0.3f));
    }
}
