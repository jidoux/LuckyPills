namespace LuckyPills;

internal readonly record struct Duration(float Minimum, float Maximum) {
	public float Random => UnityEngine.Random.Range(Minimum, Maximum);
}
