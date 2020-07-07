using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTimed : TutorialBehaviour
{ 
    [SerializeField] float SecondsToWait = 5f;
    protected override void Show()
    {
       
        base.Show();
        
        //Add an object to be the parent
        var maskGO = new GameObject
        {
            name = "Tutorial Mask"
        };
        maskGO.transform.SetParent( tutorialPanel.transform );
        maskGO.transform.SetAsFirstSibling();
        maskGO.AddComponent<RectTransform>().CoverViewport();

        //Add a object to contain the gray efect and the click event
        var overlayGO = new GameObject
        {
            name = "Tutorial Overlay"
        };
        overlayGO.transform.SetParent( maskGO.transform );
        overlayGO.AddComponent<RectTransform>().CoverViewport();
        
        //Add overlay image component
        var overlayImage = overlayGO.AddComponent<Image>();
        overlayImage.color = new Color( 0, 0, 0, .5f );
        overlayImage.rectTransform.CoverViewport();
        
        StartCoroutine( ClosePanel() );
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(SecondsToWait);
        PlayerSession.IncrementTutorialStep();
        Hide();
    }
}
