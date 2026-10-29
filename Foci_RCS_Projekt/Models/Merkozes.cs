using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foci_RCS_Projekt.Models;

public class Merkozes
{
    public int Id { get; set; }

    [Required]
    public int HazaiCsapatId { get; set; }

    [ForeignKey(nameof(HazaiCsapatId))]
    [InverseProperty(nameof(Csapat.HazaiMerkozesek))]
    public Csapat HazaiCsapat { get; set; } = null!;

    [Required]
    public int VendegCsapatId { get; set; }

    [ForeignKey(nameof(VendegCsapatId))]
    [InverseProperty(nameof(Csapat.VendegMerkozesek))]
    public Csapat VendegCsapat { get; set; } = null!;

    [Required]
    public DateTime Datum { get; set; }

    [Range(0, 99, ErrorMessage = "A gólok száma 0 és 99 közötti lehet.")]
    public int? HazaiGol { get; set; }

    [Range(0, 99, ErrorMessage = "A gólok száma 0 és 99 közötti lehet.")]
    public int? VendegGol { get; set; }
}