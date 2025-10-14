namespace Fintech.Application.DTOs.KycValidation;

public class RecognizedTextDto
{
    public List<ParsedResultDto> ParsedResults { get; set; }

    public int OCRExitCode { get; set; }

    public bool IsErroredOnProcessing { get; set; }

    public List<string> ErrorMessage { get; set; }

    public string ProcessingTimeInMilliseconds { get; set; }
}

public class ParsedResultDto
{
    public TextOverlayDto TextOverlay { get; set; }

    public string TextOrientation { get; set; }

    public int FileParseExitCode { get; set; }

    public string ParsedText { get; set; }

    public string ErrorMessage { get; set; }

    public string ErrorDetails { get; set; }
}

public class TextOverlayDto
{
    public List<LineDto> Lines { get; set; }

    public bool HasOverlay { get; set; }

    public string Message { get; set; }
}

public class LineDto
{
    public List<WordDto> Words { get; set; }

    public int MaxHeight { get; set; }

    public int MinTop { get; set; }
}

public class WordDto
{
    public string WordText { get; set; }

    public int Left { get; set; }

    public int Top { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }
}
