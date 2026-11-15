namespace MedievilArchipelago.Helpers
{
    internal static class OptionHandler
    {
        // Slot options normally arrive as numbers, but a room can send something else (e.g. an empty list).
        // Anything that isn't a whole number counts as 0 so the client carries on instead of crashing mid-login.
        public static int Int(Dictionary<string, object>? options, string key)
        {
            var text = options?.GetValueOrDefault(key)?.ToString();
            if (int.TryParse(text, out var value)) return value;
            if (bool.TryParse(text, out var flag)) return flag ? 1 : 0;
            return 0;
        }
    }
}
