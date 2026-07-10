using UnityEngine;

namespace Features.Player
{
    public class MinimapCameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float zOffset = -10f;

        private void LateUpdate()
        {
            if (target != null)
            {
                transform.position = new Vector3(target.position.x, target.position.y, zOffset);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}