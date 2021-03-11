using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    public Transform firstCheckpoint;

    private void Awake()
    {
        instance = this;
    }
    
    [ContextMenu("Test Init")]
    public void Init()
    {
        firstCheckpoint = transform.GetChild(0);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Checkpoint>().nextCheckpoint = transform.GetChild(i + 1);
        }

        transform.GetChild(transform.childCount - 1).gameObject.GetComponent<Checkpoint>().nextCheckpoint = firstCheckpoint;
    }
}
