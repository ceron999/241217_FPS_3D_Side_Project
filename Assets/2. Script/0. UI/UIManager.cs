using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    public static T Show<T>(UIList uiType) where T : UIBase
    {
        var uiInstance = Singleton.GetUI<T>(uiType);
        uiInstance?.Show();

        return uiInstance;
    }

    public static T Hide<T>(UIList uiType) where T : UIBase
    {
        var uiInstance = Singleton.GetUI<T>(uiType);
        uiInstance?.Hide();

        return uiInstance;
    }

    private Transform panelRoot;
    private Transform popupRoot;
    
    private Dictionary<UIList, UIBase> panels = new Dictionary<UIList, UIBase>();
    private Dictionary<UIList, UIBase> popups = new Dictionary<UIList, UIBase>();

    protected override void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (panelRoot == null)
        {
            GameObject goPanelRoot = new GameObject("PanelRoot");
            panelRoot = goPanelRoot.transform;
            panelRoot.SetParent(transform);
            panelRoot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            panelRoot.localScale = Vector3.one;
        }

        if (popupRoot == null)
        {
            GameObject goPopupRoot = new GameObject("PopupRoot");
            popupRoot = goPopupRoot.transform;
            popupRoot.SetParent(transform);
            popupRoot.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            popupRoot.localScale = Vector3.one;
        }

        for (int index = (int)UIList.PANEL_START; index < (int)UIList.PANEL_End; index++)
        {
            panels.Add((UIList)index, null);
        }

        for (int index = (int)UIList.POPUP_START; index < (int)UIList.POPUP_END; index++)
        {
            popups.Add((UIList)index, null);
        }
    }

    public T GetUI<T>(UIList uiType, bool reload = false) where T : UIBase
    {
        if(UIList.PANEL_START < uiType && uiType < UIList.PANEL_End)
        {
            if(panels.ContainsKey(uiType))
            {
                if(reload && panels[uiType] != null)
                {
                    Destroy(panels[uiType].gameObject);
                    panels[uiType] = null;
                    return null;
                }

                if(panels[uiType] == null)
                {
                    string path = $"UI/{uiType.ToString()}";
                    
                    UIBase loadedPrefab = Resources.Load<UIBase>(path);
                    if (loadedPrefab == null)
                        return null;

                    T result = loadedPrefab.GetComponent<T>();
                    if (result == null)
                        return null;

                    panels[uiType] = Instantiate(loadedPrefab, panelRoot);
                    panels[uiType].gameObject.SetActive(false);

                    return panels[uiType].GetComponent<T>();

                }
                else
                {
                    return panels[uiType].GetComponent<T>();
                }
            }
        }

        if (UIList.POPUP_START < uiType && uiType < UIList.POPUP_END)
        {
            if (popups.ContainsKey(uiType))
            {
                if (reload && popups[uiType] != null)
                {
                    Destroy(popups[uiType].gameObject);
                    popups[uiType] = null;
                    return null;
                }

                if (popups[uiType] == null)
                {
                    string path = $"UI/{uiType}";
                    UIBase loadedPrefab = Resources.Load<UIBase>(path);
                    if (loadedPrefab == null)
                        return null;

                    T result = loadedPrefab.GetComponent<T>();
                    if (result == null)
                        return null;

                    popups[uiType] = Instantiate(loadedPrefab, popupRoot);
                    popups[uiType].gameObject.SetActive(false);

                    return popups[uiType].GetComponent<T>();
                }
                else
                {
                    return popups[uiType].GetComponent<T>();
                }
            }
        }
        return null;
    }
}
