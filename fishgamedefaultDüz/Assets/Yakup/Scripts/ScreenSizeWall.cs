using UnityEngine;

public class ScreenSizeWall : MonoBehaviour
{
    public Transform solDuvar;
    public Transform sagDuvar;
    public Camera mainCamera; // Cinemachine sanal kameranız

    void Start()
    {
        AyarlaDuvarlar();
    }

    void Update()
    {
        AyarlaDuvarlar();
    }

    void AyarlaDuvarlar()
    {
        // Ekranın sol ve sağ kenarlarını dünya koordinatlarına dönüştür
       /*  Vector3 solKenar = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, -mainCamera.transform.position.z));
        Vector3 sagKenar = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, -mainCamera.transform.position.z)); */


        // Ekran boyutlarına göre duvarların x pozisyonunu sınırla
        float minX = -mainCamera.aspect * mainCamera.orthographicSize;
        float maxX = mainCamera.aspect * mainCamera.orthographicSize;


        // Duvarların yerel pozisyonlarını ayarla
        solDuvar.localPosition = new Vector3(minX, -20f, 0f); // Sol duvarın yerel pozisyonu minX
        sagDuvar.localPosition = new Vector3(maxX, -20f, 0f); // Sağ duvarın yerel pozisyonu maxX


        // Duvarların Z konumunu sabit tut (yerel eksende Z konumu sıfır olarak kalır)
    }
}
