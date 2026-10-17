using UnityEngine;
using UnityEngine.UI;



public class NavSceneButtonView : ButtonView
{
    [SerializeField] private string _sceneName;

    protected override void HandleClick()
    {
        SceneEvents.ChangeScene?.Invoke(_sceneName);
    }
}

