using System.Linq;
using System.Reflection;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class TutorialBehaviour : MonoBehaviour
{
    [SerializeField] protected int Step;
    [SerializeField] protected string Text;
    [SerializeField] protected GameObject TextPrefab;
    
    protected GameObject tutorialPanel;

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

    protected virtual void Show()
    {
        if ( tutorialPanel != null )
        {
            Debug.LogError( "You already have a tutorial panel going on" );
            return;
        }

        var myCanvas = GetComponentInParent<Canvas>();

        // Create Panel
        tutorialPanel = new GameObject
        {
            name = "Tutorial Panel"
        };
        tutorialPanel.transform.SetParent( myCanvas.transform );
        tutorialPanel.AddComponent<RectTransform>().CoverViewport();

        // TODO: Instantiate Text Prefab with the right text

        GameObject textField = Instantiate( TextPrefab, tutorialPanel.transform);
        textField.SetActive(true);
        textField.GetComponent<TextMeshProUGUI>().text = Text;
    }

    protected virtual void Hide()
    {
        if ( tutorialPanel == null )
        {
            Debug.LogError( "You don't have a tutorial panel going on" );
            return;
        }
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
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        rect.localScale = Vector3.one;
    }
}