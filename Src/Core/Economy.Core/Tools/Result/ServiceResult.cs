namespace Economy.Core.Tools.Result
{
    public sealed class ServiceResult<T>
    {
        public T? Data { get; }
        public bool IsSuccess { get; }
        public bool HasData
        {
            get
            {
                if (Data == null)
                    return false;
                if (Data is IEnumerable<object> collection)
                    return collection.Any();
                return true;
            }
        }
        public string Message { get; }
        public IReadOnlyList<string> Errors { get; }
        public string? ErrorCode { get; }

        // 🚀 Genişletmeler
        public string? CorrelationId { get; }
        public int StatusCode { get; } = 200; // Varsayılan: OK
        public IReadOnlyDictionary<string, string[]>? ValidationErrors { get; }
        public PaginationInfo? Pagination { get; }
        public IReadOnlyDictionary<string, object>? Metadata { get; }

        // ✅ Yeni eklenen özellik
        public string? RedirectUrl { get; set; }

        private ServiceResult(
            bool isSuccess,
            T? data,
            string message,
            IEnumerable<string>? errors = null,
            string? errorCode = null,
            int statusCode = 200,
            string? correlationId = null,
            IReadOnlyDictionary<string, string[]>? validationErrors = null,
            PaginationInfo? pagination = null,
            IReadOnlyDictionary<string, object>? metadata = null,
            string? redirectUrl=null)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors?.ToList() ?? new List<string>();
            ErrorCode = errorCode;
            StatusCode = statusCode;
            CorrelationId = correlationId;
            ValidationErrors = validationErrors;
            Pagination = pagination;
            Metadata = metadata;
            RedirectUrl = redirectUrl;
        }

        public static ServiceResult<T> Success(
            T data,
            string message = "İşlem Başarılı",
            int statusCode = 200,
            PaginationInfo? pagination = null,
            IReadOnlyDictionary<string, object>? metadata = null,
            string? redirectUrl = null)
            => new(true, data, message, statusCode: statusCode, pagination: pagination, metadata: metadata, redirectUrl: redirectUrl);

        public static ServiceResult<T> Empty(
            string message = "Kayıt bulunamadı.",
            int statusCode = 204)
            => new(true, default, message, statusCode: statusCode);

        public static ServiceResult<T> Failure(
            string message="Başarısız İşlem",
            IEnumerable<string>? errors = null,
            string? errorCode = null,
            int statusCode = 400,
            string? correlationId = null,
            IReadOnlyDictionary<string, string[]>? validationErrors = null,
            string? redirectUrl = null)
            => new(false, default, message, errors, errorCode, statusCode, correlationId, validationErrors, redirectUrl: redirectUrl);

        public static ServiceResult<T> Failure(
          T data,
          string message="Başarısız İşlem",
          IEnumerable<string>? errors = null,
          string? errorCode = null,
          int statusCode = 400,
          string? correlationId = null,
          IReadOnlyDictionary<string, string[]>? validationErrors = null,
          string? redirectUrl = null)
          => new(false, data, message, errors, errorCode, statusCode, correlationId, validationErrors, redirectUrl: redirectUrl);


    }

}
