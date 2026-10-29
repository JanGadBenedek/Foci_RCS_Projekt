using Foci_RCS_Projekt.Data;
using Foci_RCS_Projekt.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Foci_RCS_Projekt.Pages.CsapatPages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Csapat> Csapatok { get; set; } = new List<Csapat>();

    public async Task OnGetAsync()
    {
        Csapatok = await _context.Csapatok
            .AsNoTracking()
            .OrderBy(c => c.Nev)
            .ToListAsync();
    }
}