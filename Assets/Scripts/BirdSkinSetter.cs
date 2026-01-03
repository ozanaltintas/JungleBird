using UnityEngine;

public class BirdSkinSetter : MonoBehaviour
{
    public Sprite[] birdSprites;
    public RuntimeAnimatorController[] birdAnimators;

    private void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedBird", 0);

        if (selectedIndex < birdSprites.Length)
        {
            // 1. Resmi Değiştir
            GetComponent<SpriteRenderer>().sprite = birdSprites[selectedIndex];

            // 2. Eski Collider'ı Bul ve Sil (Şekil değişeceği için)
            PolygonCollider2D oldCollider = GetComponent<PolygonCollider2D>();
            if (oldCollider != null)
            {
                Destroy(oldCollider);
            }

            // 3. Yeni Collider Ekle (HATANIN DÜZELDİĞİ YER)
            // Başındaki "gameObject." ifadesi hatayı çözer.
            gameObject.AddComponent<PolygonCollider2D>();
        }

        // Animasyon kontrolü (Opsiyonel)
        if (birdAnimators.Length > 0 && selectedIndex < birdAnimators.Length)
        {
            GetComponent<Animator>().runtimeAnimatorController = birdAnimators[selectedIndex];
        }
    }
}