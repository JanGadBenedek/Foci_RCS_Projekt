using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Csapat Csapat { get; set; } = new();

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

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var csapat = await _context.Csapatok
            .FirstOrDefaultAsync(c => c.Id == id);

        if (csapat == null)
            return NotFound();

        bool vanMerkozese = await _context.Merkozesek
            .AnyAsync(m => m.HazaiCsapatId == id ||
                           m.VendegCsapatId == id);

        if (vanMerkozese)
        {
            TempData["Hiba"] =
                "A csapat nem törölhető, mert már van mérkőzése.";

            return RedirectToPage("./Index");
        }

        _context.Csapatok.Remove(csapat);
        await _context.SaveChangesAsync();

        TempData["Siker"] = "A csapat sikeresen törölve.";

        return RedirectToPage("./Index");
    }
}