namespace Economy.Domain.Entites
{
	public class NumberSequence
	{
		public Guid Id { get; set; }
		public string EntityName { get; set; }
		public string Prefix { get; set; }
		public string Suffix { get; set; }
		public int LastUsedCount { get; set; }
	}
}
