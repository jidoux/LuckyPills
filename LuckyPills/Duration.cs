namespace LuckyPills;

internal class Duration {
	private Duration() { }

	public Duration(int minimum, int maximum) {
		Minimum = minimum;
		Maximum = maximum;
	}

	public int Minimum { get; set; }

	public int Maximum { get; set; }

	public int Get() => new Random().Next(Minimum, Maximum);
}
