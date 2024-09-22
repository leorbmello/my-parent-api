namespace MyParentApi.Application.DTOs.Requests.Family
{
    public record FamilyCreateRequest
    {
        public FamilyCreateRequest(string familyName)
        {
            Name = familyName;
        }

        public string Name { get; init; }
    }
}
