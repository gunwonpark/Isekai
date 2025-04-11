public enum SoundType
{
    BGM,
    SFX,

    MAX,
}

public enum SoundName
{
    PlayerWalk,
    PlayerClick,
    DialogTyping,

}


public class SoundData
{
    //소리 크기
    public float volume;
    //오디오 이름
    public string name;
    //해당 오디오 클립
    public UnityEngine.AudioClip clip;
    //loop 여부
    public bool isLoop;
}