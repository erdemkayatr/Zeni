namespace Zeni.Infra.Results
{
    public interface IResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T ResultData { get; }
    }
}