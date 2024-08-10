using FluentValidation;
using Microsoft.AspNetCore.Http;
using Workwise.Application.Dtos.Messages;

namespace Workwise.Application.Validators.Message
{
    public class MessageUpdateDtoValidation : AbstractValidator<MessageUpdateDto>
    {
        public MessageUpdateDtoValidation()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty().WithMessage("Chat ID is required.");

            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body is required.");

            RuleFor(x => x.File)
                .Must(file => file == null || IsValidFileType(file))
                .WithMessage("Invalid file type.")
                .When(x => x.File != null);
        }

        private bool IsValidFileType(IFormFile file)
        {
            string[] allowedExtensions = new[] { ".jpg", ".png", ".pdf" }; 
            string extension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(extension.ToLower());
        }
    }
}
