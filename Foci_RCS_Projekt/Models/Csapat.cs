using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foci_RCS_Projekt.Models;

public class Csapat
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A csapat neve kötelező.")]
    [StringLength(100)]
    public string Nev { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Varos { get; set; }

    [Range(1850, 2100, ErrorMessage = "Érvényes alapítási évet adj meg.")]
    public int? AlapitasEve { get; set; }

    [InverseProperty(nameof(Merkozes.HazaiCsapat))]
    public ICollection<Merkozes> HazaiMerkozesek { get; set; }
        = new List<Merkozes>();

    [InverseProperty(nameof(Merkozes.VendegCsapat))]
    public ICollection<Merkozes> VendegMerkozesek { get; set; }
        = new List<Merkozes>();
}