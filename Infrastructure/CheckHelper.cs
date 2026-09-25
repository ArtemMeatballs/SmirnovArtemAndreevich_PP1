namespace WoodAccountingSystem.Infrastructure
{
    public static class CheckHelper
    {
        public static string Check(string textInput, string textOutput)
        {
            if (textInput == "")
                return $"Введите значение в поле '{textOutput}'!";

            if (!int.TryParse(textInput, out _))
                return $"Значение поля '{textOutput}' должно быть целым числом!";

            if (int.Parse(textInput) <= 0)
                return $"Значение поля '{textOutput}' должно быть больше ноля!";

            return null;
        }
    }
}