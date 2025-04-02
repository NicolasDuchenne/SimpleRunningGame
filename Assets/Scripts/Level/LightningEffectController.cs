using UnityEngine;

public class LightningEffectController : MonoBehaviour
{

    GameObject lastEncouteredPlayer;
    Color lightningColor = Color.red;
    void Update()
    {
        if(lastEncouteredPlayer is not null & lastEncouteredPlayer?.GetComponent<PlayerBlinkController>().GetCurrentColor()!=lightningColor)
        {
            lastEncouteredPlayer.GetComponent<PlayerBlinkController>().SetMaterialColor(lightningColor);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.parent.gameObject;
        if (player.CompareTag("Player"))
        {
            lastEncouteredPlayer = player;
            player.GetComponent<PlayerLife>().StartLightningEffect();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.transform.parent.gameObject;
        if (player.CompareTag("Player"))
        {
            player.GetComponent<PlayerLife>().StopLightningEffect();
            player.GetComponent<PlayerBlinkController>().SetDefaultColor();
            lastEncouteredPlayer = null;
        }
        
    }
}
