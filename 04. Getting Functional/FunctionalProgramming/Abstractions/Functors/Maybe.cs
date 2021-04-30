namespace Abstractions.Functors
{
    using System;

    public readonly struct Maybe<T>
    {
        private readonly T value;

        public Maybe(T value) => this.value = value;

        public static Maybe<T> None => new(default);

        public T Value => this.value;

        public bool HasValue => this.value != null;

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func) where TResult : class 
            => this.Value != null ? func(this.Value) : Maybe<TResult>.None;

        public static implicit operator Maybe<T>(T value)
        {
            if (value?.GetType() == typeof(Maybe<T>))
            {
                return (Maybe<T>)(object)value;
            }

            return new Maybe<T>(value);
        }
    }
}
