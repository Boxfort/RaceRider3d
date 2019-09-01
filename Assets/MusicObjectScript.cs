using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicObjectScript : MonoBehaviour
{
	private static MusicObjectScript instance = null;
	public static MusicObjectScript Instance
	{
		get { return instance; }
	}
	void Awake()
	{
		if (instance != null & instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
