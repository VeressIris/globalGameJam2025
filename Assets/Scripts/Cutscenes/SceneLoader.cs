using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float loadDelay = 1.25f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(TransitionToLevel(level));
    }

    private IEnumerator TransitionToLevel(int level)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(level);
    }
}
