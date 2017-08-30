using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a localized label with special formatting functions
/// 
/// Created By: Neil
/// </summary>

public class LocalizedLabel : UILocalize {
	

	//override to empty functions
	void OnEnable () {
	
	}
	
	private bool hasFormatted = false;


	public void FormatText(string keyOrText, params object[] values) {
		this.hasFormatted = false;
		this.key = keyOrText;
		this.OnLocalize();

		UILabel lbl = this.GetComponent<UILabel>();

		string newText = string.Format(lbl.text, values);
		lbl.text = newText;

		this.hasFormatted = true;
	}

	public void FormatAppend(string keyOrText, string delimiter, params object[] values) {

		UILabel lbl = this.GetComponent<UILabel>();
		string oldText = lbl.text;

		this.FormatText(keyOrText, values);
		string newText = lbl.text;

		if(oldText == "") {
			lbl.text = newText;
		}
		else {
			lbl.text = oldText + delimiter + newText;
		}
	}

	public void Clear() {
		UILabel lbl = this.GetComponent<UILabel>();
		lbl.text = "";
	}

	protected override void OnLocalize ()
	{
		if(this.hasFormatted) {
			this.hasFormatted = false;
			return;
		}

		base.OnLocalize();
	}
}
