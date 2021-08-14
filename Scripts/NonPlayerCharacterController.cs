using UnityEngine;

namespace Mod3D
{
    [RequireComponent(typeof(Animator))]
    public class NonPlayerCharacterController : MonoBehaviour
    {
        Animator m_Animator;
        void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void Move()
        {
            isWalking(true);
        }

        public void Stop()
        {
            isWalking(false);
        }

        void isWalking(bool walking)
        {
            m_Animator.SetBool("isWalking", walking);
        }
    }
}