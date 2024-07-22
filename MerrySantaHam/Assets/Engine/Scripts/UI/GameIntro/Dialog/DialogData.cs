using System;

[Serializable]
public class DialogData
{
    public enum DIALOGEVENT_TYPE
    {
        DET_NONE, // 0
        DET_FADEIN, DET_FADEOUT, DET_FADEINOUT, DET_FADEOUTIN, // 1 2 3 4
        DET_GAMESTART, // 5

        DET_END
    };

    public DIALOGEVENT_TYPE dialogEvent;
    public string dialogText;

    // 리소스 관련
    public string cutSceneSpr;
}
