using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.MerkozesPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Merkozes> Merkozesek { get; set; } = new List<Merkozes>();

    public async Task OnGetAsync()
    {
        Merkozesek = await _context.Merkozesek
            .AsNoTracking()
            .Include(m => m.HazaiCsapat)
            .Include(m => m.VendegCsapat)
            .OrderByDescending(m => m.Datum)
            .ToListAsync();
    }
}
