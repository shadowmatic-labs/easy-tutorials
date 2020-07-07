using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : TutorialBehaviour
{
    protected override void Show()
    {
        base.Show();
        var myButton = GetComponent<Button>();
        myButton.onClick.AddListener( PlayerSession.IncrementTutorialStep );
        myButton.onClick.AddListener( Hide );
        
        // Add mask to panel
        var maskGO = new GameObject
        {
            name = "Tutorial Mask"
        };
        maskGO.transform.SetParent( tutorialPanel.transform );
        maskGO.transform.SetAsFirstSibling();
        maskGO.AddComponent<RectTransform>().CoverViewport();
        // Add mask image component
        var maskImage = maskGO.AddComponent<Image>();
        maskImage.raycastTarget = false;
        // Add mask component
        var mask = maskGO.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        // Add unmask to mask, matching this button
        var unmaskGO = new GameObject
        {
            name = "Tutorial Unmask Hole"
        };
        unmaskGO.transform.SetParent( maskGO.transform );
        var unmaskRect = unmaskGO.AddComponent<RectTransform>();
        var unmaskImage = unmaskGO.AddComponent<Image>();
        unmaskImage.raycastTarget = false;
        var unmask = unmaskGO.AddComponent<Unmask>();
        unmaskRect.localScale = Vector3.one * 1.2f;
        unmask.fitTarget = myButton.gameObject.GetComponent<RectTransform>();

        // Add overlay
        var overlayGO = new GameObject
        {
            name = "Tutorial Overlay"
        };
        overlayGO.transform.SetParent( maskGO.transform );
        overlayGO.AddComponent<RectTransform>().CoverViewport();
        // Add overlay image component
        var overlayImage = overlayGO.AddComponent<Image>();
        overlayImage.color = new Color( 0, 0, 0, .5f );
        overlayImage.rectTransform.CoverViewport();
        // Add raycast 'hole' and target the unmask
        var unmaskRaycast = overlayGO.AddComponent<UnmaskRaycastFilter>();
        unmaskRaycast.targetUnmask = unmask;
    }

    protected override void Hide()
    {
        base.Hide();
        var myButton = GetComponent<Button>();
        myButton.onClick.RemoveListener( PlayerSession.IncrementTutorialStep );
        myButton.onClick.RemoveListener( Hide );
    }
}
