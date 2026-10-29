using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.MerkozesPages;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Merkozes Merkozes { get; set; } = new();

    public SelectList CsapatLista { get; set; } = default!;

    public async Task OnGetAsync()
    {
        await CsapatListaBetolteseAsync();
        Merkozes.Datum = DateTime.Today;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Merkozes.HazaiCsapatId == Merkozes.VendegCsapatId)
        {
            ModelState.AddModelError(
                string.Empty,
                "A hazai és a vendégcsapat nem lehet ugyanaz.");
        }

        bool hazaiLetezik = await _context.Csapatok
            .AnyAsync(c => c.Id == Merkozes.HazaiCsapatId);

        bool vendegLetezik = await _context.Csapatok
            .AnyAsync(c => c.Id == Merkozes.VendegCsapatId);

        if (!hazaiLetezik || !vendegLetezik)
        {
            ModelState.AddModelError(
                string.Empty,
                "Válassz ki két érvényes csapatot.");
        }

        bool letezikMar = await _context.Merkozesek.AnyAsync(m =>
            m.HazaiCsapatId == Merkozes.HazaiCsapatId &&
            m.VendegCsapatId == Merkozes.VendegCsapatId);

        if (letezikMar)
        {
            ModelState.AddModelError(
                string.Empty,
                "Ez a hazai–vendég párosítás már szerepel a bajnokságban.");
        }

        ModelState.Remove("Merkozes.HazaiCsapat");
        ModelState.Remove("Merkozes.VendegCsapat");

        if (!ModelState.IsValid)
        {
            await CsapatListaBetolteseAsync();
            return Page();
        }

        // Új mérkőzésnél még nincs rögzített eredmény.
        Merkozes.HazaiGol = null;
        Merkozes.VendegGol = null;

        _context.Merkozesek.Add(Merkozes);
        await _context.SaveChangesAsync();

        TempData["Siker"] = "A mérkőzés sikeresen felvéve.";

        return RedirectToPage("./Index");
    }

    private async Task CsapatListaBetolteseAsync()
    {
        var csapatok = await _context.Csapatok
            .AsNoTracking()
            .OrderBy(c => c.Nev)
            .ToListAsync();

        CsapatLista = new SelectList(
            csapatok,
            nameof(Csapat.Id),
            nameof(Csapat.Nev));
    }
}
