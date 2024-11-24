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
	Title,
	Game,
	LibraryScene,
	stage01,
	stage02,
	stage03,
	stage04,
	stage05,
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

