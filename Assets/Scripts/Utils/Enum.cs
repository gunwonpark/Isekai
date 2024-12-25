using System;

public enum State
{
	Die,
	Moving,
	Idle,
	Skill,
}

public enum Scene
{
	Unknown,
	TitleScene,
	IntroScene,
	LibraryScene,
	GameScene,
	EndingScene
}

public enum Sound
{
	Bgm,
	Effect,
	MaxCount,
}

public enum UIEvent
{
	Click,
	Drag,
}

public enum MouseEvent
{
	Press,
	PointerDown,
	PointerUp,
	Click,
}

