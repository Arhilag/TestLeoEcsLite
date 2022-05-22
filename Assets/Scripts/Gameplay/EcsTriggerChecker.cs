using UnityEngine;
using Voody.UniLeo.Lite;

public class EcsTriggerChecker : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        WorldHandler.GetMainWorld();
        Debug.Log("Player");
    }
}