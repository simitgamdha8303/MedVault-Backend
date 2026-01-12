using MedVault.Models.Enums;
namespace MedVault.Utilities;

public static class DocumentTypeHelper
{
    public static DocumentType Detect(string fileName)
    {
        string extension = Path.GetExtension(fileName)?.ToLowerInvariant();

        return extension switch
        {
            ".jpg"  => DocumentType.Jpg,
            ".png"  => DocumentType.Png,
            ".pdf"  => DocumentType.Pdf,
        };
    }
}
