namespace Print.Application.Domain;

public enum RasterQualityType
{
    /// <summary>The type of Raster Quality is Low.</summary>
    Low = 72, // 0x00000048
    /// <summary>The type of Raster Quality is Medium.</summary>
    Medium = 150, // 0x00000096
    /// <summary>The type of Raster Quality is High.</summary>
    High = 300, // 0x0000012C
    /// <summary>The type of Raster Quality is Presentation.</summary>
    Presentation = 600, // 0x00000258
}
