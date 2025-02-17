using Grpc.Core;
using MessageService;

public class MessageProcessService : MessageProcessService.MessageProcessServiceBase
{
    public override Task<ValidationResponse> ValidateMessage(MessageRequest request, ServerCallContext context)
    {
        // اعتبارسنجی پیام (مثال: بررسی طول پیام)
        bool isValid = !string.IsNullOrEmpty(request.MessageText) && request.MessageText.Length <= 100;

        // ایجاد پاسخ
        var response = new ValidationResponse
        {
            Id = request.Id,
            IsValid = isValid,
            ValidationMessage = isValid ? "Message is valid." : "Message is invalid."
        };

        return Task.FromResult(response);
    }
}