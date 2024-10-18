using UnityEngine;

public class BooserDistract : MonoBehaviour
{
    public BooserNPC npc;

    void OnEnable()
    {
        if (npc != null)
        {
            npc.ReactToSound(transform.position);
        }
    }
}
