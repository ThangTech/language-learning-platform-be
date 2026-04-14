namespace Shared.Common.Constants;

/// <summary>
/// Application-wide constants.
/// </summary>
public static class AppConstants
{
    // Account status
    public const string StatusActive = "Active";
    public const string StatusLocked = "Locked";
    public const string StatusInactive = "Inactive";

    // Word levels
    public const string LevelBeginner = "Beginner";
    public const string LevelIntermediate = "Intermediate";
    public const string LevelAdvanced = "Advanced";

    // Word types
    public const string WordTypeNoun = "Noun";
    public const string WordTypeVerb = "Verb";
    public const string WordTypeAdjective = "Adjective";
    public const string WordTypeAdverb = "Adverb";
    public const string WordTypePreposition = "Preposition";
    public const string WordTypeConjunction = "Conjunction";
    public const string WordTypePronoun = "Pronoun";

    // Quiz status
    public const string QuizInProgress = "InProgress";
    public const string QuizCompleted = "Completed";
    public const string QuizAbandoned = "Abandoned";

    // Quiz question types
    public const string QuestionMultipleChoice = "MultipleChoice";
    public const string QuestionFillBlank = "FillBlank";
    public const string QuestionMatching = "Matching";

    // Avatar types
    public const string AvatarTypeUrl = "url";
    public const string AvatarTypeFile = "file";

    // File upload
    public const long MaxAvatarSizeBytes = 5 * 1024 * 1024; // 5MB
    public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
    public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    public static readonly string[] AllowedDocumentExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
}
