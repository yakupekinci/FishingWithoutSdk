using UnityEngine;

public class WebGLAntiAliasing : MonoBehaviour
{
    public int targetAntiAliasingLevel = 2; // Hedef anti-aliasing seviyesi (örneğin, 2x)

    void Awake()
    {
        // WebGL platformunda çalıştığımızdan emin olun
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // Hedef anti-aliasing seviyesini kontrol edin
            if (QualitySettings.antiAliasing != targetAntiAliasingLevel)
            {
                // Anti-aliasing seviyesini ayarlayın
                QualitySettings.antiAliasing = targetAntiAliasingLevel;
                Debug.Log("Anti-aliasing seviyesi ayarlandı: " + targetAntiAliasingLevel + "x");
            }
            else
            {
                Debug.Log("Anti-aliasing zaten ayarlandı: " + targetAntiAliasingLevel + "x");
            }
        }
        else
        {
            Debug.Log("Bu kod sadece WebGL platformunda çalışır.");
        }
    }
}