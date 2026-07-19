namespace LuckyPills;

internal record Duration(float Minimum, float Maximum) {
	public float Random => UnityEngine.Random.Range(Minimum, Maximum);
}
