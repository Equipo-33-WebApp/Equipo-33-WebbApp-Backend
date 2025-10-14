namespace Fintech.Application.DTOs.KycValidation;

public class AnalyzedFacialRecognitionDto
{
    public FaceRectangle Region { get; set; }
    public Age Age { get; set; }
    public Gender Gender { get; set; }
    public Race Race { get; set; }
    public Emotion Emotion { get; set; }
}

public class FaceRectangle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }
}

public class Age
{
    public float AgeValue { get; set; }
}

public class Gender
{
    public string DominantGender { get; set; }
    public Dictionary<string, float> GenderConfidence { get; set; }
}

public class Race
{
    public string DominantRace { get; set; }
    public Dictionary<string, float> RaceConfidence { get; set; }
}

public class Emotion
{
    public string DominantEmotion { get; set; }
    public Dictionary<string, float> EmotionConfidence { get; set; }
}
