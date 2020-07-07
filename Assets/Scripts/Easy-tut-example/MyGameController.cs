using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MyGameController : MonoBehaviour
{
    // First Panel
    [SerializeField] GameObject FirstPanel;
    [SerializeField] Button ImportantButton;
    [SerializeField] Button LessRelevantButton;
    [SerializeField] Button OtherStuffButton;
    [SerializeField] Button SomeSocialFeaturesButton;
    
    // Important Panel Buttons
    [SerializeField] GameObject SecondPanel;
    [SerializeField] Button ClearPlayerPrefsButton;

    public void Start()
    {
        ImportantButton.onClick.AddListener( () =>
        {
            Debug.Log( "You clicked an important button" );
            FirstPanel.SetActive( false );
            SecondPanel.SetActive( true );
        } );
        
        LessRelevantButton.onClick.AddListener( () =>
        {
            Debug.Log( "This button is so irrelevant that it does nothing" );
        } );
        
        OtherStuffButton.onClick.AddListener( () => throw new NotImplementedException() );
        SomeSocialFeaturesButton.onClick.AddListener( () =>
        {
            Debug.Log( "Go outside" );
        } );
        
        ClearPlayerPrefsButton.onClick.AddListener( () =>
        {
            Debug.Log( "You have cleared you preferences, you'll see the the tutorial again next time you play" );
            StartCoroutine( ClearPrefs() );
        } );
    }

    IEnumerator ClearPrefs()
    {
        yield return new WaitForEndOfFrame(); // We want to delay this so the TutorialButton listeners happen first
        PlayerSession.Reset();
    }
}
