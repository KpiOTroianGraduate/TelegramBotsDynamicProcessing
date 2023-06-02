namespace Contracts.Dto;

public class KeyboardMarkupDto<T> where T : class
{
    public string Title { get; set; } = null!;

    public T Content { get; set; } = null!;
}