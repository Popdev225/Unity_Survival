using UnityEngine;

public class SamsungBehavior : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    private PlayerController playerController;

    public void Setup(GameObject textPrefab, PlayerController controller)
    {
        floatingTextPrefab = textPrefab;
        playerController = controller;
    }
}
