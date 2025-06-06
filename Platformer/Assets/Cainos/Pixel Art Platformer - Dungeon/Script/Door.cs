using UnityEngine;
using Cainos.LucidEditor;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Cainos.PixelArtPlatformer_Dungeon
{
    public class Door : MonoBehaviour
    {
        [FoldoutGroup("Reference")] public SpriteRenderer spriteRenderer;
        [FoldoutGroup("Reference")] public Sprite spriteOpened;
        [FoldoutGroup("Reference")] public Sprite spriteClosed;

        private Animator Animator
        {
            get
            {
                if (animator == null) animator = GetComponent<Animator>();
                return animator;
            }
        }
        private Animator animator;

        [FoldoutGroup("Runtime"), ShowInInspector]
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                isOpened = value;

#if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    if (Animator.runtimeAnimatorController != null)
                    {
                        Animator.SetBool("IsOpened", isOpened);
                    }
                    else
                    {
                        Debug.LogWarning("Animator controller not assigned on Door!");
                    }
                }
#endif

                if (Application.isPlaying)
                {
                    Animator.SetBool("IsOpened", isOpened);
                }
                else
                {
                    if (spriteRenderer) spriteRenderer.sprite = isOpened ? spriteOpened : spriteClosed;
                }

                Collider2D col = GetComponent<Collider2D>();
                if (col != null)
                {
                    col.isTrigger = isOpened;
                }
            }
        }
        [SerializeField, HideInInspector]
        private bool isOpened;

        private void Start()
        {
            Animator.Play(isOpened ? "Opened" : "Closed");
            IsOpened = isOpened;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player2"))
            {
                Player2Controler player = collision.gameObject.GetComponent<Player2Controler>();

                if (player != null && player.IsDashing())
                {
                    Door.OpenAllDoors();
                }
            }
        }

        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Button"), Button("Open")]
        public void Open()
        {
            IsOpened = true;
        }

        [FoldoutGroup("Runtime"), HorizontalGroup("Runtime/Button"), Button("Close")]
        public void Close()
        {
            IsOpened = false;
        }

        //private IEnumerator AutoClose()
        //{
        //    yield return new WaitForSeconds(5f);
        //    Close();
        //}

        public static void OpenAllDoors()
        {
            Door[] allDoors =  Object.FindObjectsByType<Door>(FindObjectsSortMode.None);
            foreach (var door in allDoors)
            {
                door.Open();
            }

            allDoors[0].StartCoroutine(CloseAllAfterDelay(allDoors, 5f));
        }

        private static IEnumerator CloseAllAfterDelay(Door[] doors, float delay)
        {
            yield return new WaitForSeconds(delay);

            foreach (var door in doors)
            {
                door.Close();
            }
        }
    }
}
