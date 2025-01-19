using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomUI : UIBase
{
    public static ZoomUI Instance => UIManager.Singleton.GetUI<ZoomUI>(UIList.ZoomUI);
}
