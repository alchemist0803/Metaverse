using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

using TMPro;

public class DragonEventHamdler : MonoBehaviour
{
  public TMP_Text text;

  [DllImport("__Internal")]
  private static extern void unityStart();

	void Start()
	{
		#if !UNITY_EDITOR && UNITY_WEBGL
      PassReact();
		#endif
    unityStart();
	}

	void Update()
	{
			
	}

  public void resetText() {
    text.text = "Button Clicked";
  }

	public void walletConnect(string address)
  {
    text.text = address;
  }

  public void PassUnity()
  {
		#if !UNITY_EDITOR && UNITY_WEBGL
      WebGLInput.captureAllKeyboardInput = true;
		#endif
  }

  public void PassReact()
  {
		#if !UNITY_EDITOR && UNITY_WEBGL
      WebGLInput.captureAllKeyboardInput = false;
		#endif
  }
}
