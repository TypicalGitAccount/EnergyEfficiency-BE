namespace EnergyEfficiencyBE.Models.ResultPattern
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T? Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        internal Result(T? value, bool isSuccess, string? message)
            : base(isSuccess, message)
        {
            _value = value;
        }
    }
}
