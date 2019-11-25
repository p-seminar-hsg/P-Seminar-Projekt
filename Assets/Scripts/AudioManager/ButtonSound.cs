using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        AudioManager.instance.Play("Button");
    }
}
