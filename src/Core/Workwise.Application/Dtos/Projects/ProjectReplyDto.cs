﻿using Workwise.Domain.Entities;

namespace Workwise.Application.Dtos
{
    public record ProjectReplyDto
    {
        public string Id { get; init; } = null!;
        public string ReplyComment { get; init; } = null!;
        public DateTime CreateAt { get; init; }
        public DateTime? UpdateAt { get; init; }
        public string CreatedBy { get; init; } = null!;

        public string AppUserId { get; init; } = null!;
        public AppUserIncludeDto? AppUser { get; init; }
        public string ProjectCommentId { get; init; } = null!;
    }
}
