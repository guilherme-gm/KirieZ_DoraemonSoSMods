#nullable enable

public class IdHolder<T> where T: class, IIdentifiableConfig
{
	public int Id {
		get {
			if (this.customConfig != null)
				return customConfig.GetId();

			return this.officialId;
		}
		private set {}
	}

	public int officialId { get; internal set; } = -1;

	public T? customConfig { get; internal set; } = default(T);

	public IdHolder(int id)
	{
		this.officialId = id;
	}

	public IdHolder(T config)
	{
		this.customConfig = config;
	}
}
