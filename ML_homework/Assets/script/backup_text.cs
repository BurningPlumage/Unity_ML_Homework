using UnityEngine;

public class backup_text : MonoBehaviour
{
    public GameObject target_text;

    public void backup_text_display()
    {
        if (target_text.activeSelf)
        {
            target_text.SetActive(false);
        }
        else
        {
            target_text.SetActive(true);
        }
    }
}
