namespace kzModUtils.TextData
{
	public class TextHelper
	{
		public delegate void OnTextRegistered(int id);

		public static void RegisterText(string text, OnTextRegistered callback)
		{
			TextDataModule.Instance.TextToRegister.Add(new TextDataModule.PendingTextRegister() {
				Text = text,
				Callback = callback,
			});
		}
	}
}
