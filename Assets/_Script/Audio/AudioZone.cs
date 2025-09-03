using UnityEngine;

public class AudioZone : MonoBehaviour
{
    enum AUDIO_TARGET { CAMERA, PLAYER, }

    [SerializeField] Collider col;
    Transform target;

    [SerializeField] AUDIO_TARGET audioTarget;
    [SerializeField] Transform source;

    private void OnDrawGizmos()
    {
        if(!col.TryGetComponent(out BoxCollider box))
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + box.center, box.size); //fails to draw rotated colliders
    }

    private void Awake()
    {
        switch(audioTarget)
        {
            case AUDIO_TARGET.PLAYER:
                //target = player.transform;
            case AUDIO_TARGET.CAMERA:
            default:
                target = Camera.main.transform;
                break;
        }
    }

    private void Update()
    {
        if(target != null) 
            source.position = col.ClosestPoint(target.position);
    }
}
