//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's rotation.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	public Vector3 from;
	public Vector3 to;
	public bool quaternionLerp = false;

	Transform mTrans;

	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	[System.Obsolete("Use 'value' instead")]
	public Quaternion rotation { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Quaternion value { get { return cachedTransform.localRotation; } set { cachedTransform.localRotation = value; } }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
        value = quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor) :
            Quaternion.Euler(new Vector3(
            Mathf.Lerp(from.x, to.x, factor),
            Mathf.Lerp(from.y, to.y, factor),
            Mathf.Lerp(from.z, to.z, factor)));
        //value = quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor) :
        //        Quaternion.Euler(new Vector3(
        //        from.x,
        //        Mathf.Lerp(from.y, to.y, factor),
        //        from.z));
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenRotation Begin (GameObject go, float duration, Quaternion rot)
	{
		TweenRotation comp = UITweener.Begin<TweenRotation>(go, duration);
        Vector3 f = comp.value.eulerAngles;
        Vector3 t = rot.eulerAngles;
        if (f.y - t.y < -180)
        {
            t = new Vector3(t.x, t.y - 360, t.z);
        }
        else if (f.y - t.y > 180)
        {
            f = new Vector3(f.x, f.y - 360, f.z);
        }
		comp.from = f;
		comp.to = t;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value.eulerAngles; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value.eulerAngles; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = Quaternion.Euler(from); }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = Quaternion.Euler(to); }
}
