using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bulletMagazine, totalBullets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBullets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBullets()
    {
        bulletMagazine.text = GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].MagazineBullets;
        totalBullets.text = GameManager.instance.GetGameData.Weapons[GameManager.instance.GetGameData.WeaponIndex].TotalBullets;
    }
}
