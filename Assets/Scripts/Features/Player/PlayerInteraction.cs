using System;
using UnityEngine;
using Zenject;
using Features.Interaction;
using Features.Resources;

namespace Features.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private float interactRadius = 0.5f;

        [SerializeField]
        private LayerMask interactableLayer;

        private Animator _animator;
        private IInteractable _current;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            IInteractable detected = DetectInteractable();

            if (_current != detected)
            {
                if (_current != null)
                {
                    _current.OnPlayerExit();
                }

                if (detected != null)
                {
                    detected.OnPlayerEnter();
                }

                _current = detected;
            }

            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            if (_current == null)
            {
                _animator.SetBool("IsInteracting", false);
                _animator.SetInteger("StationType", -1);
                return;
            }


            _animator.SetBool("IsInteracting", _current.IsActive);

            _animator.SetInteger("StationType", (int)_current.StationType);
        }

        private IInteractable DetectInteractable()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                interactRadius,
                interactableLayer);

            foreach (var hit in hits)
            {
                IInteractable interactable = hit.GetComponentInParent<IInteractable>();
                if (interactable != null)
                {
                    return interactable;
                }
            }

            return null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"OnCollisionEnter2D{other.gameObject.name}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}