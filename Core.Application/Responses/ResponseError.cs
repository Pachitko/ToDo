namespace Core.Application.Responses
{
    public class ResponseError
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ResponseError(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}