using System.Linq;
using System.Reflection;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TutorialElement : MonoBehaviour
{
    [SerializeField] protected int Step;
    [SerializeField] protected string Text;
    [SerializeField] protected GameObject TextPrefab;
}

// TODO: Create another TutorialElement for Non-Button tutorial steps (tap to continue)

public class TutorialButton : TutorialElement
{
    GameObject tutorialPanel;

    public void Update()
    {
        if ( tutorialPanel == null && PlayerSession.GetTutorialStep() == Step )
        {
            Show();
        }

        if ( tutorialPanel != null && PlayerSession.GetTutorialStep() != Step )
        {
            Hide();
        }
    }

    void Show()
    {
        if ( tutorialPanel != null )
        {
            Debug.LogError( "You already have a tutorial panel going on" );
            return;
        }

        var myButton = GetComponent<Button>();
        myButton.onClick.AddListener( PlayerSession.IncrementTutorialStep );
        myButton.onClick.AddListener( Hide );
        
        var myCanvas = GetComponentInParent<Canvas>();

        // Create Panel
        tutorialPanel = new GameObject
        {
            name = "Tutorial Panel"
        };
        tutorialPanel.transform.SetParent( myCanvas.transform );
        tutorialPanel.AddComponent<RectTransform>().CoverViewport();

        // Add mask to panel
        var maskGO = new GameObject
        {
            name = "Tutorial Mask"
        };
        maskGO.transform.SetParent( tutorialPanel.transform );
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

        // TODO: Instantiate Text Prefab with the right text

        GameObject textField = Instantiate( TextPrefab, tutorialPanel.transform);
        textField.SetActive(true);
        textField.GetComponent<TextMeshProUGUI>().text = Text;



    }

    void Hide()
    {
        if ( tutorialPanel == null )
        {
            Debug.LogError( "You don't have a tutorial panel going on" );
            return;
        }
        var myButton = GetComponent<Button>();
        myButton.onClick.RemoveListener( PlayerSession.IncrementTutorialStep );
        myButton.onClick.RemoveListener( Hide );

        Destroy( tutorialPanel );
    }
}

public static class ComponentExtensions
{
    public static T GetCopyOf<T>( this Component comp, T other ) where T : Component
    {
        var type = comp.GetType();
        if ( type != other.GetType() )
        {
            return null; // type mis-match
        }

        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                   BindingFlags.Default | BindingFlags.DeclaredOnly;
        var properties = type.GetProperties( flags );
        foreach ( var property in properties.Where( property => property.CanWrite ) )
        {
            try
            {
                property.SetValue( comp, property.GetValue( other, null ), null );
            }
            catch
            {
                // ignored
            }
        }

        var fieldInfos = type.GetFields( flags );
        foreach ( var fieldInfo in fieldInfos )
        {
            fieldInfo.SetValue( comp, fieldInfo.GetValue( other ) );
        }

        return comp as T;
    }

    public static T AddComponent<T>( this GameObject go, T toAdd ) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf( toAdd ) as T;
    }
}

public static class RectTransformExtensions
{
    public static void CoverViewport( this RectTransform rect )
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchoredPosition = Vector2.zero;
    }
}