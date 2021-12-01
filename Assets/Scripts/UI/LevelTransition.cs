using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelTransition : MonoBehaviour
{

    [SerializeField] GameObject overlay;

    public static LevelTransition Instance { get; set; }

    [SerializeField] float transitionLength;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator OnLevelChange()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return sequence.WaitForCompletion();
        LevelManager.Instance.PauseGame();
        yield return new WaitForSecondsRealtime(transitionLength);
        LevelManager.Instance.UnpauseGame();
        sequence.Append(overlay.transform.DOScaleX(0f, 0.4f));
        yield return sequence.WaitForCompletion();
        GameController.Instance.currentState = State.Active;
    }

    public IEnumerator OnStart()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return sequence.WaitForCompletion();
    }

    public IEnumerator OnEnterShop()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return new WaitForSeconds(0.5f);
        GameController.Instance.shopMenu.SetActive(true);
    }

    public IEnumerator OnLevelUp()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return new WaitForSeconds(0.5f);

        GameController.Instance.levelUpMenu.SetActive(true);


    }

    public IEnumerator OnPause()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return new WaitForSeconds(0.5f);

        GameController.Instance.pauseMenu.SetActive(true);
    }

    public IEnumerator OnJoin()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(0f, 0.4f));
        yield return sequence.WaitForCompletion();
    }

    public IEnumerator OnDeath()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return sequence.WaitForCompletion();
        GameController.Instance.gameOverMenu.SetActive(true);
    }

    public IEnumerator OnGameWin()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(1f, 0.4f));
        yield return sequence.WaitForCompletion();
        GameController.Instance.gameWinMenu.SetActive(true);

    }

    public IEnumerator OnUnpause()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(overlay.transform.DOScaleX(0f, 0.4f));
        yield return sequence.WaitForCompletion();
        // GameController.Instance.currentState = State.Active;
        GameController.Instance.pauseMenu.SetActive(false);
    }

}
