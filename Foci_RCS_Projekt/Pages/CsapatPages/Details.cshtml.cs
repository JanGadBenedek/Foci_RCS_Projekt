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

        Merkozesek = await _context.Merkozesek
            .AsNoTracking()
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .Where(m => m.HazaiCsapatId == id ||
                        m.VendegCsapatId == id)
            .OrderByDescending(m => m.Datum)
            .ToListAsync();

        return Page();
    }
}