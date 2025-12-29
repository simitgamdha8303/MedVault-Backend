namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Document
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(PatientProfile))]
    public int PatientId { get; set; }

    [Required, MaxLength(255)]
    public string FileName { get; set; } = null!;

    [Required, Url]
    public string FileUrl { get; set; } = null!;

    [Required, ForeignKey(nameof(DocumentType))]
    public int DocumentTypeId { get; set; }

    [Required, ForeignKey(nameof(MedicalTimeline))]
    public int MedicalTimelineId { get; set; }

    [Required]
    public DateOnly DocumentDate { get; set; }

    [Required]
    public DateTime UploadedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public PatientProfile Patient { get; set; } = null!;

    public DocumentType DocumentType { get; set; } = null!;
    
    public MedicalTimeline MedicalTimeline { get; set; } = null!;
}
