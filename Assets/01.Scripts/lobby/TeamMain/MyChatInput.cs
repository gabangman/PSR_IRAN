using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]

public class MyChatInput : MonoBehaviour
{
	public UITextList textList;
	public bool fillWithDummyData = false;



	UIInput mInput;
	bool mIgnoreNextEnter = false;
	
	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>
	
	void Start ()
	{
		mInput = GetComponent<UIInput>();
		
		if (fillWithDummyData && textList != null)
		{
			for (int i = 0; i < 30; ++i)
			{
				textList.Add(((i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]") +
				             "This is an example paragraph for the text list, testing line " + i + "[-]");
			}
		}
	}
	
	/// <summary>
	/// Pressing 'enter' should immediately give focus to the input field.
	/// </summary>
	
	void Update ()
	{
		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (!mIgnoreNextEnter && !mInput.selected)
			{
				mInput.selected = true;
			}
			mIgnoreNextEnter = false;
		}
	}
	
	/// <summary>
	/// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>
	
	void OnSubmit ()
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			//string text1 =string.Format(KoStorage.GetKorString("77231"), System.DateTime.Now, GV.UserNick, mInput.text);
			string text = NGUITools.StripSymbols(mInput.text);
			
			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(text);
				mInput.text = "";
				mInput.selected = false;
			}
		}
		mIgnoreNextEnter = true;
	}
	
	void OnSubmitChat ()
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text1 =string.Format(KoStorage.GetKorString("77231"), System.DateTime.Now, GV.UserNick, mInput.text);
			string text = NGUITools.StripSymbols(text1);
			
			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(text);
				mInput.text = "";
				mInput.selected = false;
			}
		}
		mIgnoreNextEnter = true;
	}
	
	void OnGlobalChatSend(){
		if (!mIgnoreNextEnter && !mInput.selected)
		{
			mInput.selected = true;
		}
		mIgnoreNextEnter = false;
	}
	
	void OnClanChatSend(){
		if (!mIgnoreNextEnter && !mInput.selected)
		{
			mInput.selected = true;
		}
		mIgnoreNextEnter = false;
	}
	
}
