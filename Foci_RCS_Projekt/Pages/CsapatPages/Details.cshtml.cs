
using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Csapat Csapat { get; set; } = new();

    public List<Merkozes> Merkozesek { get; set; } = new();

    public List<FormaEredmeny> UtolsoOtMerkozes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var csapat = await _context.Csapatok
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (csapat == null)
            return NotFound();

        Csapat = csapat;

        // A csapat összes mérkőzése, a még le nem játszottakkal együtt.
        Merkozesek = await _context.Merkozesek
            .AsNoTracking()
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .Where(m => m.HazaiCsapatId == id ||
                        m.VendegCsapatId == id)
            .OrderByDescending(m => m.Datum)
            .ToListAsync();

        // Csak az eredménnyel rendelkező mérkőzések számítanak bele a formába.
        var lejatszottMeccsek = Merkozesek
            .Where(m => m.HazaiGol.HasValue &&
                        m.VendegGol.HasValue)
            .OrderByDescending(m => m.Datum)
            .ThenByDescending(m => m.Id)
            .Take(5);

        foreach (var meccs in lejatszottMeccsek)
        {
            bool hazai = meccs.HazaiCsapatId == Csapat.Id;

            int sajatGol = hazai
                ? meccs.HazaiGol!.Value
                : meccs.VendegGol!.Value;

            int ellenfelGol = hazai
                ? meccs.VendegGol!.Value
                : meccs.HazaiGol!.Value;

            string eredmeny;

            if (sajatGol > ellenfelGol)
                eredmeny = "GY";
            else if (sajatGol == ellenfelGol)
                eredmeny = "D";
            else
                eredmeny = "V";

            UtolsoOtMerkozes.Add(new FormaEredmeny
            {
                Datum = meccs.Datum,
                Ellenfel = hazai
                    ? meccs.VendegCsapat.Nev
                    : meccs.HazaiCsapat.Nev,
                Eredmeny = eredmeny,
                SajatGol = sajatGol,
                EllenfelGol = ellenfelGol
            });
        }

        return Page();
    }

    public class FormaEredmeny
    {
        public DateTime Datum { get; set; }
        public string Ellenfel { get; set; } = string.Empty;
        public string Eredmeny { get; set; } = string.Empty;
        public int SajatGol { get; set; }
        public int EllenfelGol { get; set; }
    }
}
