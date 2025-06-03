namespace RaritetBooks.Domain.Common
{
    public class Error
    {
        private const string SEPARATOR = "||";

        public static readonly Error None = new(String.Empty, String.Empty);

        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Serialize()
        {
            return $"{Code}{SEPARATOR}{Message}";
        }

        public static Error Deserialize(string serialized)
        {
            var data = serialized.Split([SEPARATOR], StringSplitOptions.RemoveEmptyEntries);

            if (data.Length < 2)
                return new Error("unexpected", data[0]);

            return new(data[0], data[1]);
        }
    }
}