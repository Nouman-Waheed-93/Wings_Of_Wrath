using UnityEngine;

namespace Locomotion
{
    public class SpeedView : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed;

        [SerializeField]
        private Transform needle;

        [SerializeField]
        private GameObject speedProviderGameObject;

        private ISpeedProvider speedTarget;

        private float currSpeed;

        private void Awake()
        {
            speedTarget = speedProviderGameObject.GetComponent<ISpeedProvider>();
        }

        private void Update()
        {
            currSpeed = speedTarget.CurrSpeed;
            needle.transform.localRotation = Quaternion.Euler(0, 0, -180 * (currSpeed / maxSpeed));
        }
    }
}
