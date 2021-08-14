using UnityEngine;

public class Interaction : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.interaction = this;
            UIManager.instance.keyText.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.interaction = null;
            UIManager.instance.keyText.enabled = false;
        }
    }

    public virtual void CanInteraction() { }
  
}
