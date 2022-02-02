using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private BoolVariableSO _gamePaused;
    
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _defaultSelectedItem;
    
    [SerializeField] private List<GameObject> _toDisable;

    private EventSystem _myEventSystem;

    private void Start()
    {
        _myEventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>(); // to select menu items

        // we set the game to unpaused by default (if it was for some reason not already unpaused)
        if (_gamePaused.Value)
            Resume();
    }

    private void OnDestroy()
    {
        if (_gamePaused.Value) // we resume the game if the pause menu is destroyed in any way (loading a new scene for example...)
            Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // press escape to pause/unpause
            TogglePause();
    }

    private void TogglePause()
    {
        if (_gamePaused.Value)
            Resume();
        else
            Pause();
        
        // if (!_gamePaused.Value)
        //     Pause();
    }
    
    private void Pause()
    {
        foreach (var obj in _toDisable)
            obj.SetActive(false);
        _panel.SetActive(true);
        _myEventSystem.SetSelectedGameObject(_defaultSelectedItem);
        Time.timeScale = 0f;
        _gamePaused.Value = true;
        // pause sounds?
    }

    public void Resume() // exact opposite of Pause()
    {
        foreach (var obj in _toDisable)
            obj.SetActive(true);
        _panel.SetActive(false);
        _myEventSystem.SetSelectedGameObject(null); // to deselect every menu item
        Time.timeScale = 1f;
        _gamePaused.Value = false;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
