namespace OpenLabel.Shared
{
    /// <summary>
    /// Represents the result of an operation, which can either be a success with a value of type <typeparamref name="T"/> or a failure with an error of type <typeparamref name="E"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value in case of success.</typeparam>
    /// <typeparam name="E">The type of the error in case of failure.</typeparam>
    public readonly struct Result<T, E>
    {
        private readonly T? _value;
        private readonly E? _error;

        /// <summary>
        /// Gets a value indicating whether the result is a success.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the result is a failure.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        private Result(T value)
        {
            IsSuccess = true;
            _value = value;
            _error = default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T, E}"/> struct with a success value.
        /// </summary>
        /// <param name="value">The success value.</param>
        private Result(E error)
        {
            IsSuccess = false;
            _value = default;
            _error = error;
        }

        /// <summary>
        /// Creates a successful <see cref="Result{T, E}"/> with the specified value.
        /// </summary>
        /// <param name="value">The success value.</param>
        /// <returns>A new <see cref="Result{T, E}"/> instance representing success.</returns>
        public static Result<T, E> Ok(T value) => new(value);

        /// <summary>
        /// Creates a failed <see cref="Result{T, E}"/> with the specified error.
        /// </summary>
        /// <param name="error">The error value.</param>
        /// <returns>A new <see cref="Result{T, E}"/> instance representing failure.</returns>
        public static Result<T, E> Err(E error) => new(error);

        /// <summary>
        /// Gets the value of the result if it is a success. Throws an exception if the result is a failure.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the result is a failure.</exception>
        public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access Value of an error Result");

        /// <summary>
        /// Gets the error of the result if it is a failure. Throws an exception if the result is a success.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the result is a success.</exception>
        public E Error => IsFailure ? _error! : throw new InvalidOperationException("Cannot access Error of a success Result");

        /// <summary>
        /// Transforms the result into another type in case of success.
        /// </summary>
        /// <typeparam name="U">The type to transform the value into.</typeparam>
        /// <param name="fn">The function to transform the value.</param>
        /// <returns>A new result of type <see cref="Result{U, E}"/>.</returns>
        public Result<U, E> Map<U>(Func<T, U> fn) => IsSuccess ? Result<U, E>.Ok(fn(_value!)) : Result<U, E>.Err(_error!);

        /// <summary>
        /// Transforms the error into another type in case of failure.
        /// </summary>
        /// <typeparam name="F">The type to transform the error into.</typeparam>
        /// <param name="fn">The function to transform the error.</param>
        /// <returns>A new result of type <see cref="Result{T, F}"/>.</returns>
        public Result<T, F> MapErr<F>(Func<E, F> fn) => IsFailure ? Result<T, F>.Err(fn(_error!)) : Result<T, F>.Ok(_value!);
    }

    /// <summary>
    /// Provides helper methods for working with <see cref="Result{T, E}"/>.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Checks if the result represents a successful operation.
        /// </summary>
        /// <typeparam name="T">The type of the success value.</typeparam>
        /// <typeparam name="E">The type of the error value.</typeparam>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if the result represents a success; otherwise, <c>false</c>.</returns>
        public static bool IsOk<T, E>(Result<T, E> result) => result.IsSuccess;

        /// <summary>
        /// Checks if the result represents a failure.
        /// </summary>
        /// <typeparam name="T">The type of the success value.</typeparam>
        /// <typeparam name="E">The type of the error value.</typeparam>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if the result represents a failure; otherwise, <c>false</c>.</returns>
        public static bool IsErr<T, E>(Result<T, E> result) => result.IsFailure;
    }
}
