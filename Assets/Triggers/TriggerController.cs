using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class TriggerController : MonoBehaviour
{

    #region Variables
    [Header("Trigger State")]
    public bool TriggerActivated = false;

    [Header("Trigger Variables")]

    [Tooltip("Trigger will not disable after activation. Will trigger once and then do nothing else.")]
    public bool SingleTrigger;

    [Tooltip("Layers that may activate the trigger.")]
    public LayerMask activatorLayers;

    [Header("Internal")]
    [SerializeField]
    int acivatorCount = 0;
    int previousActivatorCount = 0;
    [SerializeField]
    bool singleTriggerActivated = false;

    [Tooltip(   
                "To create a conditon, write a script derived from [TriggerCondition] and overwrite " +
                "the method [CheckCondition]. Any code may be used as long as a bool value is returned." +
                "Add the script as a component to this object and drag it into the array. Any number of " +
                "conditions may be used, but they must all be true to activate the trigger.")]
                 public TriggerCondition[] triggerConditions;

    [Header("Trigger Events")]
    public UnityEvent onTriggerActivated;
    public UnityEvent onTriggerDeactivated;
    #endregion

    #region Trigger Methods
    private void OnTriggerEnter(Collider other)
    {
        if ((activatorLayers.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            acivatorCount++;
            if (previousActivatorCount == 0)
            {
                if (triggerConditions != null)
                {
                    if (CheckConditions())
                        Activate();
                    else
                        return;
                }
                Activate();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((activatorLayers.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            acivatorCount--;
            if (acivatorCount <= 0)
            {
                Deactivate();
            }

        }
    }
    private bool CheckConditions()
    {
        for (int i = 0; i < triggerConditions.Length; i++)
        {
            if (!triggerConditions[i].CheckCondition())
                return false;
        }
        return true;
    }
    void Activate()
    {
        if (SingleTrigger)
        {
            if (!singleTriggerActivated)
            {
                onTriggerActivated.Invoke();
                TriggerActivated = true;
                singleTriggerActivated = true;
            }
        }
        else
        {
            onTriggerActivated.Invoke();
            TriggerActivated = true;
        }
    }
    void Deactivate()
    {
        if (!SingleTrigger)
        {
            onTriggerDeactivated.Invoke();
            TriggerActivated = false;
        }
    }
    #endregion

    #region Menu Methods
    [MenuItem("Triggers/Create New Trigger [Box]")]
    private static void CreateNewBoxTrigger()
    {
        var g = new GameObject("Trigger");
        g.AddComponent<TriggerController>();
        g.AddComponent<BoxCollider>();
        
        Ray ray = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {         
            g.transform.position = hit.point;           
        }

        Selection.activeGameObject = g;
    }

    [MenuItem("Triggers/Create New Trigger [Sphere]")]
    private static void CreateNewSphereTrigger()
    {
        var g = new GameObject("Trigger");
        g.AddComponent<TriggerController>();
        g.AddComponent<SphereCollider>();

        Ray ray = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            g.transform.position = hit.point;
        }

        Selection.activeGameObject = g;
    }
    #endregion

}
