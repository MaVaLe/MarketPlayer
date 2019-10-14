namespace MarketPlayer.Core.EngineInterfaces
{
	public interface IUIService
	{
		///<summary>Открывает диалог с выбором файла txt и возвращает имя выбранного файла.</summary>
		///<returns>В случае не выбора файла значение string.Empty</returns>
		string GetFName();

		void ShowMsgForUser(string msg);
	}
}